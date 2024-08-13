using UnityEngine;
using UnityEngine.Serialization;

public class Boss : Monster  // ���� ��ũ��Ʈ ���
{
    [FormerlySerializedAs("gameManager")]
    public GameManager GameManager;
    [FormerlySerializedAs("bossTimer")]
    public BossTimer BossTimer;

    // ���� ���� Ȱ��ȭ ��
    protected override void OnEnable()
    {
        Hp = monsterData.Hp * 3; // ���� ���ʹ� HP�� 2��� ����
        damage = monsterData.Damage * 2; // ���� ���ʹ� �������� 2��� ����
        attackSpeed = monsterData.AttackSpeed;
        moveTime = 0.0f;
        isAttacking = false;

        BossTimer.ActivateTimer();
    }

    // ���� ���� �ǰ�
    public override void TakeDamage(int damage, bool isSkillDamage = false)
    {
        if (damageTextPool is not null)
        {
            var damageText = damageTextPool.GetDamageText();
            if (damageText is not null)
            {
                damageText.transform.position = hudPos.position;
                damageText.SetDamage(damage);
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
        Hp -= damage;

        if (Hp > 0) return;
        Die();
        BossDeath();
        gameObject.SetActive(false);

        BossTimer.DeactivateTimer();
    }

    // ���� ���
    private void BossDeath()
    {
        // BossMonster�� HP�� 0 ���ϰ� �Ǹ� StagePage�� 0�� �ȴ�.
        GameManager.StagePage = 0;
        // BossMonster�� HP�� 0 ���ϰ� �Ǹ� Stage�� 1 ������Ų��.
        GameManager.Stage++;
        monsterData.stage = GameManager.Stage;  // ����SO�� �������� ���� ����?
        // BossMonster�� HP�� 0 ���ϰ� �Ǹ� MonsterDataSO_Test�� ���� 1.2f ���ϰ� ��Ʈ������ ��ȯ�ؼ� ����
        monsterData.Hp = Mathf.RoundToInt(monsterData.Hp * 1.2f);
        monsterData.Damage = Mathf.RoundToInt(monsterData.Damage * 1.2f);
        
        PlayerSpeechBubble.Instance.ShowMessage(PlayerSpeech.Instance.SpeechContents, SpeechLength.Short);
    }
}
