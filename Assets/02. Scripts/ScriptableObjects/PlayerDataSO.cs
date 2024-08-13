using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "ScriptableObjects/PlayerDataSO", order = 1)]
public class PlayerDataSO : ScriptableObject
{
    [FormerlySerializedAs("skills")]
    public List<SkillDataSO> Skills; //�κ��丮 ó�� ���⿡ ��ų ������ ���Ե�?

    [FormerlySerializedAs("companions")]
    public List<CompanionDataSO> Companions; //�� ����

    [FormerlySerializedAs("weapons")]
    public List<EquipmentDataSO> Weapons; //���� 

    [FormerlySerializedAs("armors")]
    public List<EquipmentDataSO> Armors; //���?
    //�߰�

    [FormerlySerializedAs("playerName")]
    public string PlayerName;

    // �÷��̾� �̸�
    // �Խ�Ʈ �α������� �÷����ϸ� Guest-Login ���� ǥ���ϸ� �� ����
    // �Ҽ� �α������� �÷����ϸ� ?
    public int Gold; // ���� ��ȭ (��ȭ��)
    public int Gem;
    public int Diamond; // ���� ��ȭ (�̱��?

    public int TotalPower; // ���� ������
    public int Damage; // ���ݷ�
    public int Def; // ����
    public float AttackSpeed; // ���ݼӵ�

    public int Hp; // ü��
    public int HpRecovery; // ü��ȸ�� (1�ʴ� ȸ��)

    // ġ��Ÿ�� �ִ� ��ȭ ������ 500����?
    public float CriticalPer; // ġ��Ÿ Ȯ�� 1���� ���� ( ��ȭ���� 0.2 �����ϸ� �� ���� )
    public float CriticalMultiplier; // ġ��Ÿ ���� 1���� ���� ( 0���� 150�ۼ�Ʈ ������ ���� + ������ 0.3%����? )


    public EquipmentDataSO CurrentWeaponEquip;
    public EquipmentDataSO CurrentArmorEquip;
}