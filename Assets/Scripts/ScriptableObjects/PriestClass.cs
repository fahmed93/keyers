using UnityEngine;

namespace MobileRPG.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Priest", menuName = "RPG/Classes/Priest")]
    public class PriestClass : CharacterClass
    {
        [Header("Priest Specific")]
        public float healingPowerMultiplier = 1.4f;
        public float shieldStrengthBonus = 1.2f;
    }
}
