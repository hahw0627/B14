using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Monster  // ���� ��ũ��Ʈ ���
{
    public GameManager gameManager;
    public BossTimer bossTimer;

    // ���� ���� Ȱ��ȭ ��
    protected override void OnEnable()
    {
        Hp = monsterData.Hp * 3; // ���� ���ʹ� HP�� 2��� ����
        damage = monsterData.Damage * 2; // ���� ���ʹ� �������� 2��� ����
        attackSpeed = monsterData.AttackSpeed;
        moveTime = 0.0f;
        isAttacking = false;

        bossTimer.ActivateTimer();
    }

    // ���� ���� �ǰ�
    public override void TakeDamage(int damage, bool isSkillDamage = false)
    {
        if (damageTextPool != null)
        {
            DamageText damageText = damageTextPool.GetDamageText();
            if (damageText != null)
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

        if (Hp <= 0)
        {
            this.Die();
            BossDeath();
            gameObject.SetActive(false);

            bossTimer.DeactivateTimer();
        }
    }

    // ���� ���
    public void BossDeath()
    {
        // BossMonster�� HP�� 0 ���ϰ� �Ǹ� StagePage�� 0�� �ȴ�.
        gameManager.stagePage = 0;
        // BossMonster�� HP�� 0 ���ϰ� �Ǹ� Stage�� 1 ������Ų��.
        gameManager.stage++;
        monsterData.stage = gameManager.stage;  // ����SO�� �������� ���� ����?
        // BossMonster�� HP�� 0 ���ϰ� �Ǹ� MonsterDataSO_Test�� ���� 1.2f ���ϰ� ��Ʈ������ ��ȯ�ؼ� ����
        monsterData.Hp = Mathf.RoundToInt(monsterData.Hp * 1.2f);
        monsterData.Damage = Mathf.RoundToInt(monsterData.Damage * 1.2f);
        
        PlayerSpeechBubble.Instance.ShowMessage(PlayerSpeech.Instance.SpeechContents, SpeechLength.SHORT);
    }
}
