using UnityEngine;

namespace MobileRPG.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Warrior", menuName = "RPG/Classes/Warrior")]
    public class WarriorClass : CharacterClass
    {
        [Header("Warrior Specific")]
        public float physicalDamageBonus = 1.3f;
        public float armorMultiplier = 1.5f;
    }
}
