using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class MainSceneSkillManager : MonoBehaviour
{
    [FormerlySerializedAs("player")]
    [SerializeField]
    private Player _player;

    [FormerlySerializedAs("skillManager")]
    [SerializeField]
    private SkillManager _skillManager;

    [FormerlySerializedAs("skillButtons")]
    [SerializeField]
    private List<Button> _skillButtons;

    [FormerlySerializedAs("cooldownImages")]
    [SerializeField]
    private List<Image> _cooldownImages;

    [FormerlySerializedAs("cooldownTexts")]
    [SerializeField]
    private List<TextMeshProUGUI> _cooldownTexts;

    [FormerlySerializedAs("playerTransform")]
    [SerializeField]
    private Transform _playerTransform;

    [FormerlySerializedAs("aoeEffectSpawnPoint")]
    [SerializeField]
    private Transform _aoeEffectSpawnPoint;

    [FormerlySerializedAs("buffEffectSpawnPoint")]
    [SerializeField]
    private Transform _buffEffectSpawnPoint;
   
    [SerializeField]
    private Sprite _defaultSkillSprite;

    private readonly Dictionary<SkillDataSO, Coroutine> _cooldownCoroutines = new();

    private void Start()
    {
        InitializeSkillButtons();
        UpdateSkillButtons();
    }

    private void InitializeSkillButtons()
    {
        for (var i = 0; i < _skillButtons.Count; i++)
        {
            var index = i;
            _skillButtons[i].onClick.AddListener(() => UseSkill(index));
            SetupEmptySkillButton(_skillButtons[i], _cooldownImages[i], _cooldownTexts[i]);
        }
    }

    private static void SetupEmptySkillButton(Button button, Image cooldownImage, TextMeshProUGUI cooldownText)
    {
        button.interactable = false;
        cooldownImage.gameObject.SetActive(false);
        cooldownText.gameObject.SetActive(false);
    }

    public void UpdateSkillButtons()
    {
        for (var i = 0; i < _skillButtons.Count; i++)
        {
            var button = _skillButtons[i];
            var buttonImage = button.GetComponent<Image>();
            var cooldownImage = _cooldownImages[i];
            var cooldownText = _cooldownTexts[i];

            if (i < DataManager.Instance.PlayerDataSo.EquippedSkills.Count && DataManager.Instance.PlayerDataSo.EquippedSkills[i] != null)
            {
                var skill = DataManager.Instance.PlayerDataSo.EquippedSkills[i];
                buttonImage.sprite = skill.Icon;
                buttonImage.color = Color.white;
                button.interactable = true;

                // ��ٿ� UI ����
                cooldownImage.gameObject.SetActive(true);
                cooldownText.gameObject.SetActive(true);
                RestartCooldownCoroutine(cooldownImage, cooldownText, skill);
            }
            else
            {
                buttonImage.sprite = _defaultSkillSprite;
                button.interactable = false;

                cooldownImage.gameObject.SetActive(false);
                cooldownText.gameObject.SetActive(false);
            }

            // ��ư Ŭ�� �̺�Ʈ �缳��
            var index = i;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => UseSkill(index));
        }
    }

    private void SetupSkillButton(Button button, Image cooldownImage, TextMeshProUGUI cooldownText, SkillDataSO skill)
    {
        button.image.sprite = skill.Icon;
        button.interactable = true;

        cooldownImage.gameObject.SetActive(true);
        cooldownImage.fillAmount = 0f;

        cooldownText.gameObject.SetActive(true);
        cooldownText.text = "";

        RestartCooldownCoroutine(cooldownImage, cooldownText, skill);
    }

    private void RestartCooldownCoroutine(Image cooldownImage, TextMeshProUGUI cooldownText, SkillDataSO skill)
    {
        if (_cooldownCoroutines.TryGetValue(skill, out var existingCoroutine))
        {
            if (existingCoroutine != null)
            {
                StopCoroutine(existingCoroutine);
            }

            _cooldownCoroutines.Remove(skill);
        }

        var newCoroutine = StartCoroutine(UpdateCooldown(cooldownImage, cooldownText, skill));
        _cooldownCoroutines[skill] = newCoroutine;
    }

    public void UseSkill(int index)
    {
        if (index < 0 || index >= DataManager.Instance.PlayerDataSo.EquippedSkills.Count) return;

        var skill = DataManager.Instance.PlayerDataSo.EquippedSkills[index];
        if (!(_skillManager.GetSkillCooldown(skill) <= 0)) return;
        _skillManager.SetSkillOnCooldown(skill);
        _player.SetUsingSkill(true);
        _player.StopAttacking();

        SpawnSkillEffect(skill);

        if (skill.SkillSound != null)
        {
            SoundManager.Instance.PlaySkillSound(skill.SkillSound);
        }

        StartCoroutine(ResumeAttackAfterSkill(skill.Duration));
        RestartCooldownCoroutine(_cooldownImages[index], _cooldownTexts[index], skill);
    }

    private IEnumerator ResumeAttackAfterSkill(float duration)
    {
        yield return new WaitForSeconds(duration);
        _player.SetUsingSkill(false);
        _player.StartAttacking();
    }

    private IEnumerator UpdateCooldown(Image cooldownImage, TextMeshProUGUI cooldownText, SkillDataSO skill)
    {
        while (true)
        {
            var remainingCooldown = _skillManager.GetSkillCooldown(skill);
            if (remainingCooldown > 0)
            {
                cooldownImage.fillAmount = remainingCooldown / skill.Cooldown;
                cooldownText.text = remainingCooldown.ToString("F0");
            }
            else
            {
                cooldownImage.fillAmount = 0f;
                cooldownText.text = "";
                yield break;
            }

            yield return null;
        }
    }

    private void SpawnSkillEffect(SkillDataSO skill)
    {
        if (skill.EffectPrefab is null) return;

        GameObject effectInstance = null;
        Vector3 spawnPosition;

        switch (skill.SkillType)
        {
            case Define.SkillType.AttackBuff:
            case Define.SkillType.HealBuff:
                spawnPosition = _buffEffectSpawnPoint.position;
                effectInstance = Instantiate(skill.EffectPrefab, spawnPosition, Quaternion.identity,
                    _buffEffectSpawnPoint);
                InitializeBuffSkill(effectInstance, skill);
                StartCoroutine(DestroyEffectAfterDelay(effectInstance, 1f));
                break;

            case Define.SkillType.Projectile:
                if (_player.Scanner.NearestTarget != null)
                {
                    spawnPosition = _playerTransform.position +
                                    (_player.Scanner.NearestTarget.position - _playerTransform.position).normalized *
                                    0.5f;
                    GameObject projectileObject = Instantiate(skill.EffectPrefab, spawnPosition, Quaternion.identity);
                    InitializeProjectileSkill(projectileObject, skill);
                }

                break;

            case Define.SkillType.AreaOfEffect:
                spawnPosition = _aoeEffectSpawnPoint.position;
                effectInstance = Instantiate(skill.EffectPrefab, spawnPosition, Quaternion.identity);
                InitializeAreaEffectSkill(effectInstance, skill);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (effectInstance is not null && skill.SkillType != Define.SkillType.AttackBuff &&
            skill.SkillType != Define.SkillType.HealBuff)
        {
            Destroy(effectInstance, skill.Duration);
        }
    }

    private void InitializeBuffSkill(GameObject effectInstance, SkillDataSO skill)
    {
        var buffSkill = effectInstance.GetComponent<BuffSkill>();
        if (buffSkill is not null)
        {
            buffSkill.Initialize(skill, _player);
        }
        else
        {
            Debug.LogWarning("Buff effect prefab doesn't have BuffSkill component");
            Destroy(effectInstance);
        }
    }

    private void InitializeProjectileSkill(GameObject projectileObject, SkillDataSO skill)
    {
        var projectile = projectileObject.GetComponent<SkillProjectile>();
        if (projectile is not null)
        {
            projectile.Initialize(skill, _player.Scanner.NearestTarget.position);
        }
        else
        {
            Debug.LogWarning("Projectile effect doesn't have SkillProjectile component!");
            Destroy(projectileObject);
        }
    }

    private static void InitializeAreaEffectSkill(GameObject effectInstance, SkillDataSO skill)
    {
        var aoeSkill = effectInstance.GetComponent<AreaEffectSkill>();
        if (aoeSkill is not null)
        {
            aoeSkill.Initialize(skill);
        }
        else
        {
            Debug.LogWarning("Area effect prefab doesn't have AreaEffectSkill component!");
            Destroy(effectInstance);
        }
    }

    private static IEnumerator DestroyEffectAfterDelay(GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (effect is not null)
        {
            Destroy(effect);
        }
    }
}