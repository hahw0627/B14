using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainSceneSkillManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private List<Button> skillButtons;
    [SerializeField] private List<Image> cooldownImages;
    [SerializeField] private List<TextMeshProUGUI> cooldownTexts;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform aoeEffectSpawnPoint;
    [SerializeField] private Transform buffEffectSpawnPoint;

    private Dictionary<SkillDataSO, Coroutine> cooldownCoroutines = new Dictionary<SkillDataSO, Coroutine>();

    private void Start()
    {
        InitializeSkillButtons();
        UpdateSkillButtons();
    }

    private void InitializeSkillButtons()
    {
        for (int i = 0; i < skillButtons.Count; i++)
        {
            int index = i;
            skillButtons[i].onClick.AddListener(() => UseSkill(index));
            SetupEmptySkillButton(skillButtons[i], cooldownImages[i], cooldownTexts[i]);
        }
    }

    private void SetupEmptySkillButton(Button button, Image cooldownImage, TextMeshProUGUI cooldownText)
    {
        button.interactable = false;
        cooldownImage.gameObject.SetActive(false);
        cooldownText.gameObject.SetActive(false);
    }

    public void UpdateSkillButtons()
    {
        List<SkillDataSO> equippedSkills = skillManager.equippedSkills;

        for (int i = 0; i < skillButtons.Count; i++)
        {
            if (i < equippedSkills.Count && equippedSkills[i] != null)
            {
                SetupSkillButton(skillButtons[i], cooldownImages[i], cooldownTexts[i], equippedSkills[i]);
            }
            else
            {
                SetupEmptySkillButton(skillButtons[i], cooldownImages[i], cooldownTexts[i]);
            }
        }
    }

    private void SetupSkillButton(Button button, Image cooldownImage, TextMeshProUGUI cooldownText, SkillDataSO skill)
    {
        button.image.sprite = skill.icon;
        button.interactable = true;

        cooldownImage.gameObject.SetActive(true);
        cooldownImage.fillAmount = 0f;

        cooldownText.gameObject.SetActive(true);
        cooldownText.text = "";

        RestartCooldownCoroutine(cooldownImage, cooldownText, skill);
    }

    private void RestartCooldownCoroutine(Image cooldownImage, TextMeshProUGUI cooldownText, SkillDataSO skill)
    {
        if (cooldownCoroutines.TryGetValue(skill, out Coroutine existingCoroutine))
        {
            if (existingCoroutine != null)
            {
                StopCoroutine(existingCoroutine);
            }
            cooldownCoroutines.Remove(skill);
        }

        Coroutine newCoroutine = StartCoroutine(UpdateCooldown(cooldownImage, cooldownText, skill));
        cooldownCoroutines[skill] = newCoroutine;
    }

    public void UseSkill(int index)
    {
        if (index < 0 || index >= skillManager.equippedSkills.Count) return;

        SkillDataSO skill = skillManager.equippedSkills[index];
        if (skillManager.GetSkillCooldown(skill) <= 0)
        {
            skillManager.SetSkillOnCooldown(skill);
            player.SetUsingSkill(true);
            player.StopAttacking();

            SpawnSkillEffect(skill);

            StartCoroutine(ResumeAttackAfterSkill(skill.duration));
            RestartCooldownCoroutine(cooldownImages[index], cooldownTexts[index], skill);
        }
    }

    private IEnumerator ResumeAttackAfterSkill(float duration)
    {
        yield return new WaitForSeconds(duration);
        player.SetUsingSkill(false);
        player.StartAttacking();
    }

    private IEnumerator UpdateCooldown(Image cooldownImage, TextMeshProUGUI cooldownText, SkillDataSO skill)
    {
        while (true)
        {
            float remainingCooldown = skillManager.GetSkillCooldown(skill);
            if (remainingCooldown > 0)
            {
                cooldownImage.fillAmount = remainingCooldown / skill.cooldown;
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
        if (skill.effectPrefab == null) return;

        GameObject effectInstance = null;
        Vector3 spawnPosition = Vector3.zero;

        switch (skill.skillType)
        {
            case Define.SkillType.AttackBuff:
            case Define.SkillType.HealBuff:
                spawnPosition = buffEffectSpawnPoint.position;
                effectInstance = Instantiate(skill.effectPrefab, spawnPosition, Quaternion.identity, buffEffectSpawnPoint);
                InitializeBuffSkill(effectInstance, skill);
                StartCoroutine(DestroyEffectAfterDelay(effectInstance, 1f));
                break;

            case Define.SkillType.Projectile:
                if (player.scanner.nearestTarget != null)
                {
                    spawnPosition = playerTransform.position + (player.scanner.nearestTarget.position - playerTransform.position).normalized * 0.5f;
                    GameObject projectileObject = Instantiate(skill.effectPrefab, spawnPosition, Quaternion.identity);
                    InitializeProjectileSkill(projectileObject, skill);
                }
                break;

            case Define.SkillType.AreaOfEffect:
                spawnPosition = aoeEffectSpawnPoint.position;
                effectInstance = Instantiate(skill.effectPrefab, spawnPosition, Quaternion.identity);
                InitializeAreaEffectSkill(effectInstance, skill);
                break;
        }

        if (effectInstance != null && skill.skillType != Define.SkillType.AttackBuff && skill.skillType != Define.SkillType.HealBuff)
        {
            Destroy(effectInstance, skill.duration);
        }
    }

    private void InitializeBuffSkill(GameObject effectInstance, SkillDataSO skill)
    {
        BuffSkill buffSkill = effectInstance.GetComponent<BuffSkill>();
        if (buffSkill != null)
        {
            buffSkill.Initialize(skill, player);
        }
        else
        {
            Debug.LogWarning("Buff effect prefab doesn't have BuffSkill component");
            Destroy(effectInstance);
        }
    }

    private void InitializeProjectileSkill(GameObject projectileObject, SkillDataSO skill)
    {
        SkillProjectile projectile = projectileObject.GetComponent<SkillProjectile>();
        if (projectile != null)
        {
            projectile.Initialize(skill, player.scanner.nearestTarget.position);
        }
        else
        {
            Debug.LogWarning("Projectile effect doesn't have SkillProjectile component!");
            Destroy(projectileObject);
        }
    }

    private void InitializeAreaEffectSkill(GameObject effectInstance, SkillDataSO skill)
    {
        AreaEffectSkill aoeSkill = effectInstance.GetComponent<AreaEffectSkill>();
        if (aoeSkill != null)
        {
            aoeSkill.Initialize(skill);
        }
        else
        {
            Debug.LogWarning("Area effect prefab doesn't have AreaEffectSkill component!");
            Destroy(effectInstance);
        }
    }

    private IEnumerator DestroyEffectAfterDelay(GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (effect != null)
        {
            Destroy(effect);
        }
    }
}