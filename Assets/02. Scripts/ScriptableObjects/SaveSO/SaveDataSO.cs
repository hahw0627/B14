using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SaveDataSO", menuName = "ScriptableObjects/SaveDataSO", order = 9)]

public class SaveDataSO : ScriptableObject
{
    [FormerlySerializedAs("Skills")]
    public List<SkillDataSO> Skills;

    [FormerlySerializedAs("companions")]
    public List<CompanionDataSO> Companions;

    [FormerlySerializedAs("Monster")]
    public MonsterDataSO MonsterInfo;


}
