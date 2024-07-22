using System;
using System.Collections.Generic;
using UnityEngine;

namespace _02._Scripts.Management
{
    [Serializable]
    public struct EquipmentItem
    {
        public Sprite image;
        public string name;
        public float effectNumber; 
        public long level;
    }

    public class EquipmentManager : MonoBehaviour
    {
        [SerializeField]
        private List<EquipmentItem> _items;
    
        
    }
}