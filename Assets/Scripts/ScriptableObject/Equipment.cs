using SoftMasking.Samples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    [CreateAssetMenu(fileName = "New Equipment", menuName = "Item/Equipment")]
    public class Equipment : ScriptableObject
    {
        public enum Usable
        {
            All,
            Luban,
            Daoshi,
            ThirdChar,
            None
        };
        public string equipmentName;
        public int cost;
        public GameObject equipmentPrefab;
        public Usable usability;
    }
}