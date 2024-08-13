using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeech : MonoBehaviour
{
    [Header("대사 설정")]
    [SerializeField, TextArea]
    private List<string> _speechContents;
    
}
