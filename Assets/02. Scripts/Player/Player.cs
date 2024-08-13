using UnityEngine;
using System.Collections;
using _10._Externals.HeroEditor4D.Common.Scripts.CharacterScripts;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public Character4D Character;
    
    [FormerlySerializedAs("playerData")]
    public PlayerDataSO PlayerData;
    [FormerlySerializedAs("gameManager")]
    public GameManager GameManager;
    [FormerlySerializedAs("scanner")]
    public Scanner Scanner;

    [FormerlySerializedAs("fireMuzzle")]
    public Transform FireMuzzle;

    [FormerlySerializedAs("damage")]
    public float Damage;
    [FormerlySerializedAs("attackSpeed")]
    public float AttackSpeed;
    [FormerlySerializedAs("currentHp")]
    public int CurrentHp;
    public int CurrentDamage { get; private set; }

    [FormerlySerializedAs("criticalPer")]
    public float CriticalPer;
    [FormerlySerializedAs("criticalMultiplier")]
    public float CriticalMultiplier;

    [FormerlySerializedAs("damageTextPool")]
    public DamageTextPool DamageTextPool;

    private bool _isUsingSkill;
    private Coroutine _attackCoroutine;

    private void Awake()
    {

        PlayerData = DataManager.Instance.playerDataSO;
        Scanner = GetComponent<Scanner>();

        AttackSpeed = PlayerData.AttackSpeed;
        CurrentHp = PlayerData.Hp;

        CriticalPer = PlayerData.CriticalPer;
        CriticalMultiplier = PlayerData.CriticalMultiplier;

        UpdateDamage();
    }

    private void Start()
    {
        StartAttacking();
        StartCoroutine(RecoverHp());
    }

    // ���ݱ��
    private IEnumerator Attack()
    {
        while (true)
        {
            // scanner�� nearestTarget�� null�� �ƴ� ��쿡�� nearestTarget�� ������ target���� ���� + ����ü ����
            if (!_isUsingSkill && Scanner.nearestTarget != null)
            {
                GameObject projectile = ProjectilePool.Instance.GetProjectile();
                projectile.transform.position = FireMuzzle.position;
                Projectile projectileScript = projectile.GetComponent<Projectile>();
                projectileScript.target = Scanner.nearestTarget;   // ������ ����ü�� Ÿ�� ����
                projectileScript.SetDirection(Scanner.nearestTarget.transform.position);

                bool isCritical = IsCriticalHit();
                Damage = CurrentDamage;

                if (isCritical)
                {
                    Damage *= CriticalMultiplier;
                }

                projectileScript.damage = Mathf.RoundToInt(Damage);    // ������ ����ü�� ������ ����
                projectileScript.shooterTag = "Player";
                projectileScript.SetColor(Color.blue);
                if (DamageTextPool != null)
                {
                    DamageText damageText = DamageTextPool.GetDamageText();
                    if (damageText != null)
                    {
                        damageText.SetDamage(Mathf.RoundToInt(Damage), isCritical);
                        damageText.transform.position = Scanner.nearestTarget.transform.position;
                    }
                }

            }

            yield return new WaitForSeconds(1 / AttackSpeed); // 1�ʿ� / attackSpeed ��ŭ ����
        }
    }

    private bool IsCriticalHit()
    {
        // ���� �� ���� (0.0���� 100.0 ����)
        float randomValue = Random.Range(0f, 100f);
        // ���� ���� ġ��Ÿ Ȯ������ ������ ġ��Ÿ �߻�
        return randomValue < CriticalPer;
    }

    // ü�� ȸ�� ���
    private IEnumerator RecoverHp()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (CurrentHp < PlayerData.Hp)
            {
                CurrentHp += PlayerData.HpRecovery;
                if (CurrentHp > PlayerData.Hp)
                {
                    CurrentHp = PlayerData.Hp;
                }
            }
        }
    }

    // �ǰ� ���
    public void TakeDamage(int damage)
    {
        CurrentHp -= damage;

        if (CurrentHp <= 0)
        {
            // ���� ��Ȱ��ȭ
            GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
            foreach (GameObject monster in monsters)
            {
                monster.SetActive(false);
            }

            StageReset();
        }
    }

    public void StageReset()
    {
        // �������� ������ �ʱ�ȭ
        GameManager.StagePage = 0;

        // ü�� �ʱ�ȭ
        CurrentHp = PlayerData.Hp;
    }

    public void SetUsingSkill(bool usingSkill)
    {
        _isUsingSkill = usingSkill;
    }

    public void ApplyAttackBuff(int amount)
    {
        Debug.Log($"Applying attack buff : {amount}");
        PlayerData.Damage += amount;
        UpdateDamage();
    }
    private void UpdateDamage()
    {
        Debug.Log($"Updating damage. Base: {PlayerData.Damage}, Buff: {PlayerData.Damage}");
        CurrentDamage = PlayerData.Damage;
        Damage = CurrentDamage;
        Debug.Log($"New damage: {CurrentDamage}");
    }

    public void Heal(int amount) // ���� �÷��̾� �ǰ� ������ ���� ����
    {

    }





    public void StartAttacking()
    {
        if (_attackCoroutine == null)
        {
            _attackCoroutine = StartCoroutine(Attack());
        }
    }

    public void StopAttacking()
    {
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }
    }
}
