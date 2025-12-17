using UnityEngine;

namespace MobileRPG.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Mage", menuName = "RPG/Classes/Mage")]
    public class MageClass : CharacterClass
    {
        [Header("Mage Specific")]
        public float spellPowerMultiplier = 1.5f;
        public float manaCostReduction = 0.9f;
    }
}
