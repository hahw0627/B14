using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Monster  // 몬스터 스크립트 상속
{
    public GameManager gameManager;
    public BossTimer bossTimer;

    // 보스 몬스터 활성화 시
    protected override void OnEnable()
    {
        Hp = monsterData.Hp * 3; // 보스 몬스터는 HP를 2배로 설정
        damage = monsterData.Damage * 2; // 보스 몬스터는 데미지도 2배로 설정
        attackSpeed = monsterData.AttackSpeed;
        moveTime = 0.0f;
        isAttacking = false;

        bossTimer.ActivateTimer();
    }

    // 보스 몬스터 피격
    public override void TakeDamage(int damage, bool isSkillDamage = false)
    {
        GameObject hudText = Instantiate(hudDamgeText);
        hudText.transform.position = hudPos.position;

        DamageText damageTextComponent = hudText.GetComponent<DamageText>();

        if (damageTextComponent != null)
        {
            damageTextComponent.SetDamage(damage);
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

    // 보스 사망
    public void BossDeath()
    {
        // BossMonster의 HP가 0 이하가 되면 StagePage는 0이 된다.
        gameManager.stagePage = 0;
        // BossMonster의 HP가 0 이하가 되면 Stage를 1 증가시킨다.
        gameManager.stage++;
        monsterData.stage = gameManager.stage;  // 몬스터SO의 스테이지 정보 저장?
        // BossMonster의 HP가 0 이하가 되면 MonsterDataSO_Test의 값을 1.2f 곱하고 인트형으로 변환해서 저장
        monsterData.Hp = Mathf.RoundToInt(monsterData.Hp * 1.2f);
        monsterData.Damage = Mathf.RoundToInt(monsterData.Damage * 1.2f);
    }
}
