using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _02._Scripts.Manager
{
    [Serializable]
    public struct EquipmentItem
    {
        [FormerlySerializedAs("image")]
        public Sprite Image;
        [FormerlySerializedAs("name")]
        public string Name;
        [FormerlySerializedAs("effectNumber")]
        public float EffectNumber; 
        [FormerlySerializedAs("level")]
        public long Level;
    }

    public class EquipmentManager : MonoBehaviour
    {
        [SerializeField]
        private List<EquipmentItem> _weapons, _armors;
        
    }
}