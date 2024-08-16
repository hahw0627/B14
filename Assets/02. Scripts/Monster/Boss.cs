using UnityEngine;
using UnityEngine.Serialization;

public class Boss : Monster // ���� ��ũ��Ʈ ���
{
    [FormerlySerializedAs("bossTimer")]
    public BossTimer BossTimer;

    // ���� ���� Ȱ��ȭ ��
    protected override void OnEnable()
    {
        CurrentHp = MonsterData.MaxHp * 3; // ���� ���ʹ� HP�� 2��� ����
        Damage = MonsterData.Damage * 2; // ���� ���ʹ� �������� 2��� ����
        AttackSpeed = MonsterData.AttackSpeed;
        MoveTime = 0.0f;
        IsAttacking = false;

        BossTimer.ActivateTimer();
    }

    // ���� ���� �ǰ�
    public override void TakeDamage(int damage, bool isSkillDamage = false, bool isPetAttack = false)
    {
        if (DamageTextPool is not null)
        {
            var damageText = DamageTextPool.GetDamageText();
            if (damageText is not null)
            {
                damageText.transform.position = HUDPos.position;
                bool isCritical = UnityEngine.Random.Range(0f, 100f) < DataManager.Instance.PlayerDataSo.CriticalPer;

                if (isPetAttack)
                {
                    damageText.SetDamage(damage, false, Color.yellow, 0.8f);
                }
                else if (isCritical)
                {
                    damage = Mathf.RoundToInt(damage * DataManager.Instance.PlayerDataSo.CriticalMultiplier);
                    damageText.SetDamage(damage, true);
                }
                else
                {
                    damageText.SetDamage(damage);
                }
            }
            else
            {
                Debug.LogWarning("Failed to get DamageText from pool.");
            }
        }
        else
        {
            Debug.LogWarning("DamageTextPool is not initialized.");
        }

        CurrentHp -= damage;

        if (CurrentHp > 0) return;
        Die();
        BossDeath();
        gameObject.SetActive(false);

        BossTimer.DeactivateTimer();
    }

    // ���� ���
    private void BossDeath()
    {
        StageManager.Instance.StageDataSO.StagePage = 0;
        StageManager.Instance.ChangeStage(++StageManager.Instance.StageDataSO.Stage,
            StageManager.Instance.StageDataSO.StagePage);
        //MonsterData.stage = StageManager.Instance.StageDataSO.Stage; // ����SO�� �������� ���� ����?

        // BossMonster�� HP�� 0 ���ϰ� �Ǹ� MonsterDataSO_Test�� ���� 1.2f ���ϰ� ��Ʈ������ ��ȯ�ؼ� ����
        MonsterData.MaxHp = Mathf.RoundToInt(MonsterData.MaxHp * 1.2f);
        MonsterData.Damage = Mathf.RoundToInt(MonsterData.Damage * 1.2f);

        PlayerSpeechBubble.Instance.ShowMessage(PlayerSpeech.Instance.SpeechContents, SpeechLength.Short);
    }
}