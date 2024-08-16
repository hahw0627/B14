using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{

    [FormerlySerializedAs("equippedSkills")]
    public List<SkillDataSO> EquippedSkills = new();

    [FormerlySerializedAs("maxEquippedSkills")]
    public int MaxEquippedSkills = 5;

    public event Action onEquippedSkillsChanged;

    [FormerlySerializedAs("mainSceneSkillManager")]
    [Header("Auto Skill Management")]
    public MainSceneSkillManager MainSceneSkillManager;

    [FormerlySerializedAs("autoUseInterval")]
    public float AutoUseInterval = 0.5f;

    [SerializeField]
    private Image _autoSkillButtonImage;

    [SerializeField]
    private Sprite _autoOnImage;
    
    [SerializeField]
    private Sprite _autoOffImage;

    [FormerlySerializedAs("player")]
    public Player Player;

    private bool _isAutoMode;
    private Coroutine _autoUseCoroutine;
    private Dictionary<SkillDataSO, float> _skillCooldowns = new();

    private Coroutine _rotateCoroutine;

    private void Awake()
    {
        UpdateEquippedSkills();
        onEquippedSkillsChanged?.Invoke();
    }

    private void Update()
    {
        UpdateCooldowns();
    }

    public void UpdateEquippedSkills()
    {
        EquippedSkills.Clear();
        if (DataManager.Instance.PlayerDataSo.Skills != null)
        {
            EquippedSkills.AddRange(DataManager.Instance.PlayerDataSo.Skills);
        }
    }

    public void EquipSkill(SkillDataSO skill)
    {
        if (EquippedSkills.Count >= MaxEquippedSkills || EquippedSkills.Contains(skill) ||
            !DataManager.Instance.AllSkillsDataSo.Contains(skill)) return;
        EquippedSkills.Add(skill);
        onEquippedSkillsChanged?.Invoke();
    }

    public void UnEquipSkill(SkillDataSO skill)
    {
        var index = EquippedSkills.IndexOf(skill);
        if (index == -1) return;
        EquippedSkills[index] = null;
        onEquippedSkillsChanged?.Invoke();
    }

    public void ReplaceSkill(int index, SkillDataSO newSkill)
    {
        if (index < 0 || index >= EquippedSkills.Count) return;
        EquippedSkills[index] = newSkill;
        onEquippedSkillsChanged?.Invoke();
    }

    public void ToggleAutoMode()
    {
        _isAutoMode = !_isAutoMode;
        if (_isAutoMode)
        {
            _autoSkillButtonImage.sprite = _autoOnImage;
            StartAutoUse();
        }
        else
        {
            _autoSkillButtonImage.sprite = _autoOffImage;
            StopAutoUse();
        }
    }
    
    private void StartAutoUse()
    {
        _autoUseCoroutine ??= StartCoroutine(AutoUseSkills());
    }

    private void StopAutoUse()
    {
        if (_autoUseCoroutine == null) return;
        StopCoroutine(_autoUseCoroutine);
        _autoUseCoroutine = null;
    }

    private IEnumerator AutoUseSkills()
    {
        while (_isAutoMode)
        {
            if (Player.Scanner.NearestTarget is not null)
            {
                var skillToUse = FindUsableSkill();
                if (skillToUse is not null)
                {
                    var skillIndex = EquippedSkills.IndexOf(skillToUse);
                    if (skillIndex != -1)
                    {
                        MainSceneSkillManager.UseSkill(skillIndex);
                    }

                    yield return new WaitForSeconds(1.0f);
                }
                else
                {
                    yield return new WaitForSeconds(AutoUseInterval);
                }
            }
            else
            {
                yield return new WaitForSeconds(AutoUseInterval);
            }
        }
    }


    private SkillDataSO FindUsableSkill()
    {
        return EquippedSkills.Find(skill =>
            skill is not null && (!_skillCooldowns.ContainsKey(skill) || _skillCooldowns[skill] <= 0));
    }

    public void SetSkillOnCooldown(SkillDataSO skill)
    {
        _skillCooldowns[skill] = skill.Cooldown;
    }

    public float GetSkillCooldown(SkillDataSO skill)
    {
        return _skillCooldowns.TryGetValue(skill, out var cooldown) ? Mathf.Max(0, cooldown) : 0f;
    }

    private void UpdateCooldowns()
    {
        var deltaTime = Time.deltaTime;
        var updatedCooldowns = new Dictionary<SkillDataSO, float>();

        foreach (KeyValuePair<SkillDataSO, float> pair in _skillCooldowns)
        {
            var newCooldown = Mathf.Max(0, pair.Value - deltaTime);
            if (newCooldown > 0)
            {
                updatedCooldowns[pair.Key] = newCooldown;
            }
        }

        _skillCooldowns = updatedCooldowns;
    }

    public void EquipSkillAtIndex(int index, SkillDataSO newSkill)
    {
        if (index < 0 || index >= MaxEquippedSkills)
        {
            Debug.LogWarning("Invalid index for equipping skill.");
            return;
        }

        if (!DataManager.Instance.AllSkillsDataSo.Contains(newSkill))
        {
            Debug.LogWarning("Skill not found in allSkillsDataSO.");
            return;
        }

        // �� ��ų�� �̹� �����Ǿ� �ִ��� Ȯ��
        var existingIndex = EquippedSkills.FindIndex(s => s != null && s.SkillName == newSkill.SkillName);

        // �̹� ������ ��ų�� �ְ�, �� ��ġ�� ���� �����Ϸ��� ��ġ�� �ٸ��ٸ�
        if (existingIndex != -1 && existingIndex != index)
        {
            // �̹� ������ ��ų�� ����
            EquippedSkills[existingIndex] = null;
        }

        // �� ��ų�� ������ ��ġ�� �̹� �ٸ� ��ų�� �ִٸ� ����
        if (index < EquippedSkills.Count && EquippedSkills[index] != null)
        {
            EquippedSkills[index] = null;
        }

        // �� ��ų ����
        while (EquippedSkills.Count <= index)
        {
            EquippedSkills.Add(null);
        }

        EquippedSkills[index] = newSkill;
        onEquippedSkillsChanged?.Invoke();
    }
}