using UnityEngine;
using UnityEngine.Serialization;

public class Boss : Monster
{
    [FormerlySerializedAs("bossTimer")]
    public BossTimer BossTimer;

    protected override void OnEnable()
    {
        CurrentHp = MonsterData.MaxHp * 3;
        Damage = MonsterData.Damage * 2;
        AttackSpeed = MonsterData.AttackSpeed;
        MoveTime = 0.0f;
        IsAttacking = false;

        BossTimer.ActivateTimer();
    }

    public override void TakeDamage(int damage, bool isSkillDamage = false, bool isPetAttack = false)
    {
        if (damage > 0)
        {
            var damageText = DamageTextPool.GetDamageText();
            if (damageText != null)
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
        }

        CurrentHp -= damage;
        _animator.SetTrigger("Hit");

        if (CurrentHp > 0) return;
        Die();
        BossDeath();
        gameObject.SetActive(false);

        BossTimer.DeactivateTimer();
    }

    // ���� ���
    private void BossDeath()
    {
        DataManager.Instance.AddGem(100);
        DataManager.Instance.AddGold(100 * StageManager.Instance.StageDataSO.Stage * StageManager.Instance.StageDataSO.StagePage);
        StageManager.Instance.StageDataSO.StagePage = 0;
        StageManager.Instance.ChangeStage(++StageManager.Instance.StageDataSO.Stage,
            StageManager.Instance.StageDataSO.StagePage);
        MonsterData.MaxHp = Mathf.RoundToInt(MonsterData.MaxHp * 1.2f);
        MonsterData.Damage = Mathf.RoundToInt(MonsterData.Damage * 1.2f);

        PlayerSpeechBubble.Instance.ShowMessage(PlayerSpeech.Instance.SpeechContents, SpeechLength.Short);
    }
}