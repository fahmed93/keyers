using UnityEngine;

namespace MobileRPG.ScriptableObjects
{
    /// <summary>
    /// Base class for all character classes (Mage, Warrior, Priest)
    /// Defines abilities, stats, and class-specific behavior
    /// </summary>
    public abstract class CharacterClass : ScriptableObject
    {
        [Header("Class Info")]
        public string className;
        [TextArea(2, 4)]
        public string description;
        public Sprite classIcon;
        
        [Header("Base Stats")]
        public Core.ScalingStats stats;
        
        [Header("Abilities")]
        public Ability[] abilities;
        
        /// <summary>
        /// Get the class name
        /// </summary>
        public virtual string GetClassName() => className;
        
        /// <summary>
        /// Get class description
        /// </summary>
        public virtual string GetDescription() => description;
    }
}
