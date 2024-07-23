using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "ScriptableObjects/PlayerDataSO", order = 1)]
public class PlayerDataSO : ScriptableObject
{
    
    public List<SkillDataSO> skills;
    public string playerName;
    // 플레이어 이름
    // 게스트 로그인으로 플레이하면 Guest-Login 으로 표시하면 될 듯함
    // 소셜 로그인으로 플레이하면 ?
    public int Gold;                // 게임 재화 (강화용)
    public int Diamond;             // 현금 재화 (뽑기용)
  
    public int TotalPower;          // 종합 전투력
    public int Damage;              // 공격력
    public int Def;                 // 방어력
    public float AttackSpeed;       // 공격속도
    
    public int Hp;                  // 체력
    public int HpRecovery;          // 체력회복 (1초당 회복)

    // 치명타는 최대 강화 레벨을 500으로?
    public float CriticalPer;       // 치명타 확률 0레벨 시작 ( 강화마다 0.2 증가하면 될 듯함 )
    public float CriticalDamage;    // 치명타 공격력 0레벨 시작 ( 0레벨 150퍼센트 데미지 시작 + 레벨당 0.3%증가? )
}