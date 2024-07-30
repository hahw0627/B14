using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "ScriptableObjects/PlayerDataSO", order = 1)]
public class PlayerDataSO : ScriptableObject
{
    
    public List<SkillDataSO> skills;    //�κ��丮 ó�� ���⿡ ��ų ������ ���Ե�.
    public List<PetDataSO> pets;        //�� ����

    public List<EquipmentDataSO> weapons; //���� 
    public List<EquipmentDataSO> armors;  //��

    public List<AchievementDataSO> achievements;
    //�߰�

    public string playerName;
    // �÷��̾� �̸�
    // �Խ�Ʈ �α������� �÷����ϸ� Guest-Login ���� ǥ���ϸ� �� ����
    // �Ҽ� �α������� �÷����ϸ� ?
    public int Gold;                // ���� ��ȭ (��ȭ��)
    public int Diamond;             // ���� ��ȭ (�̱��)
  
    public int TotalPower;          // ���� ������
    public int Damage;              // ���ݷ�
    public int Def;                 // ����
    public float AttackSpeed;       // ���ݼӵ�
    
    public int Hp;                  // ü��
    public int HpRecovery;          // ü��ȸ�� (1�ʴ� ȸ��)

    // ġ��Ÿ�� �ִ� ��ȭ ������ 500����?
    public float CriticalPer;       // ġ��Ÿ Ȯ�� 0���� ���� ( ��ȭ���� 0.2 �����ϸ� �� ���� )
    public float CriticalDamage;    // ġ��Ÿ ���ݷ� 0���� ���� ( 0���� 150�ۼ�Ʈ ������ ���� + ������ 0.3%����? )

    public int CountOfMonstersKilled; // 누적 몬스터 처치 횟수
    public int CountOfReinforcementSucceed; // 누적 장비 강화 성공 횟수
}

