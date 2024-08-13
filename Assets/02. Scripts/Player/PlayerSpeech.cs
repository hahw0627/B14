using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerSpeech : SingletonDestroyable<PlayerSpeech>
{
    [FormerlySerializedAs("_speechContents")]
    [Header("대사 설정")]
    [SerializeField, TextArea]
    public List<string> SpeechContents;
    
}
