using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneSkillManager : MonoBehaviour
{
    public Player player;
    public SkillManager skillManager;
    public List<Button> skillButtons;
    public AutoSkillManager autoSkillManager;
    public List<Image> cooldownImages;
    public List<TextMeshProUGUI> cooldownTexts;
    public Transform playerTransform;
    public Transform enemyTransform;
    public Transform aoeEffectSpawnPoint;
    public Transform buffEffectSpawnPoint;
    

    private Dictionary<SkillDataSO, Coroutine> cooldownCoroutines = new Dictionary<SkillDataSO, Coroutine>();

    private void Awake()
    {
        InitializeSkillButtons();
    }

    private void Start()
    {
        UpdateSkillButtons();
    }

    private void InitializeSkillButtons()
    {
        for(int i = 0; i < skillButtons.Count; i++)
        {
            SetupEmptySkillButton(skillButtons[i], cooldownImages[i], cooldownTexts[i]);
        }
    }

    private void SetupEmptySkillButton(Button button, Image cooldownImage, TextMeshProUGUI cooldownText)
    {
        button.gameObject.SetActive(true);
        button.onClick.RemoveAllListeners();
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
        button.gameObject.SetActive(true);
        button.image.sprite = skill.icon;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => UseSkill(skill));
        button.interactable = true; // ��ų�� ������ ��ư�� Ŭ�� �����ϵ��� ����

        // ��ٿ� �̹��� �ʱ�ȭ
        cooldownImage.gameObject.SetActive(true);
        cooldownImage.fillAmount = 0f;

        cooldownText.gameObject.SetActive(true);
        cooldownText.text = "";

        if (cooldownCoroutines.TryGetValue(skill, out Coroutine existingCoroutine))
        {
            if (existingCoroutine != null)
            {
                StopCoroutine(existingCoroutine);
            }
            cooldownCoroutines.Remove(skill);
        }

        // �� �ڷ�ƾ ���� �� ��ųʸ��� �߰�
        Coroutine newCoroutine = StartCoroutine(UpdateCooldown(cooldownImage, cooldownText, skill));
        cooldownCoroutines[skill] = newCoroutine;
    }

    private void DisableSkillButton(Button button, Image cooldownImage, TextMeshProUGUI cooldownText)
    {
        button.gameObject.SetActive(false);
        cooldownImage.gameObject.SetActive(false);
        cooldownText.gameObject.SetActive(false);
    }

    public void UseSkill(SkillDataSO skill)
    {
        if (autoSkillManager.GetSkillCooldown(skill) <= 0)
        {
            Debug.Log($"Using skill : {skill.skillName}");
            autoSkillManager.SetSkillOnCooldown(skill);

            player.SetUsingSkill(true); // ��ų ��� ����
            player.StopAttacking(); // ���� ����

            SpawnSkillEffect(skill);

            // ��ų ���� �ð� �� ���� �簳
            StartCoroutine(ResumeAttackAfterSkill(skill.duration));

            // ��Ÿ�� �ð�ȭ �ڷ�ƾ �����
            int index = skillManager.equippedSkills.IndexOf(skill);
            if (index != -1 && index < cooldownImages.Count)
            {
                // ���� �ڷ�ƾ�� �ִٸ� ����
                if (cooldownCoroutines.TryGetValue(skill, out Coroutine existingCoroutine))
                {
                    if (existingCoroutine != null)
                    {
                        StopCoroutine(existingCoroutine);
                    }
                    cooldownCoroutines.Remove(skill);
                }

                // �� �ڷ�ƾ ���� �� ��ųʸ��� �߰�
                Coroutine newCoroutine = StartCoroutine(UpdateCooldown(cooldownImages[index], cooldownTexts[index], skill));
                cooldownCoroutines[skill] = newCoroutine;
            }
        }
        else
        {
            Debug.Log($"Skill {skill.skillName} is on cooldown");
        }
    }
    private IEnumerator ResumeAttackAfterSkill(float duration)
    {
        yield return new WaitForSeconds(duration);
        player.SetUsingSkill(false); // ��ų ��� ����
        player.StartAttacking(); // ���� �簳
    }


    private IEnumerator UpdateCooldown(Image cooldownImage, TextMeshProUGUI cooldownText, SkillDataSO skill)
    {
        while (true)
        {
            float remainingCooldown = autoSkillManager.GetSkillCooldown(skill);
            float totalCooldown = skill.cooldown;
            if (remainingCooldown > 0)
            {
                cooldownImage.fillAmount = remainingCooldown / totalCooldown;
                cooldownText.text = remainingCooldown.ToString("F0");
            }
            else
            {
                cooldownImage.fillAmount = 0f;
                cooldownText.text = "";
                yield break; //��Ÿ���� ������ �ڷ�ƾ ����
            }

            yield return null; // �� �����Ӹ��� ������Ʈ
        }
    }

    private void SpawnSkillEffect(SkillDataSO skill)
    {
        if (skill.effectPrefab == null) return;

        GameObject effectInstance = null;
        Vector3 spawnPosition = Vector3.zero;

        switch (skill.skillType)
        {
            case SkillType.AttackBuff:
            case SkillType.HealBuff:
                spawnPosition = buffEffectSpawnPoint.position;
                effectInstance = Instantiate(skill.effectPrefab, spawnPosition, Quaternion.identity);
                effectInstance.transform.SetParent(buffEffectSpawnPoint);
                BuffSkill buffSkill = effectInstance.GetComponent<BuffSkill>();
                if(buffSkill != null)
                {
                    buffSkill.Initialize(skill,player);
                }
                else
                {
                    Debug.LogWarning("Buff effect prefab doesn't have BuffSkill component");
                    Destroy(effectInstance);
                }
                // ���� ��ų ����Ʈ�� ª�� �ð� �Ŀ� ����
                StartCoroutine(DestroyEffectAfterDelay(effectInstance, 1f)); // 1�� �� ����
                break;

            case SkillType.Projectile:
                if(player.scanner.nearestTarget != null)
                {
                    spawnPosition = playerTransform.position;
                    Vector3 targetPosition = player.scanner.nearestTarget.position;
                    Vector3 direction = (targetPosition - spawnPosition).normalized;

                    // ������Ÿ���� �÷��̾� �� �ʿ� ����
                    spawnPosition += direction * 0.5f;

                    GameObject projectileObject = Instantiate(skill.effectPrefab,spawnPosition,Quaternion.identity);

                    SkillProjectile projectile = projectileObject.GetComponent<SkillProjectile>();  
                    if(projectile != null)
                    {
                        projectile.Initialize(skill, targetPosition, player.damage);
                        Debug.Log($"Skill projectile '{skill.skillName}' spawned at {spawnPosition} and moving towards target at position {targetPosition}");
                    }
                    else
                    {   
                        Debug.LogWarning("Projectile effect doesn't have SkillProjectile component!");
                        Destroy(projectileObject);
                    }
                }
                else
                {
                    Debug.Log("No target found for projectile skill.");
                }
                break;

            case SkillType.AreaOfEffect:
                spawnPosition = aoeEffectSpawnPoint.position;
                effectInstance = Instantiate(skill.effectPrefab, spawnPosition, Quaternion.identity);
                AreaEffectSkill aoeSkill = effectInstance.GetComponent<AreaEffectSkill>();
                if(aoeSkill != null)
                {
                    aoeSkill.Initialize(skill, player.damage);
                }
                else
                {
                    Debug.LogWarning("Area effect prefab doesn't have AreaEffectSkill component!");
                    Destroy(effectInstance);
                }
                break;
        }

        if (effectInstance != null && skill.skillType != SkillType.AttackBuff && skill.skillType != SkillType.HealBuff)
        {
            Destroy(effectInstance, skill.duration);
        }
    }

    private IEnumerator DestroyEffectAfterDelay(GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        if(effect != null)
        {
            Destroy(effect);
        }
    }

    private IEnumerator MoveProjectile(GameObject projectile, Vector3 direction, float speed)
    {
        float distance = 0f;
        while (distance < 20.0f && projectile != null)
        {
            if(projectile==null) yield break; // ������Ÿ���� �ı��Ǿ��ٸ� �ڷ�ƾ ����
            projectile.transform.Translate(direction * speed * Time.deltaTime, Space.World);
            distance += speed * Time.deltaTime;
            yield return null;
        }
        if(projectile != null)
        {
            Destroy(projectile);
        }
    }
}
