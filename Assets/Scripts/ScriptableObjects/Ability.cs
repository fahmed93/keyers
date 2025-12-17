using UnityEngine;

namespace MobileRPG.ScriptableObjects
{
    public enum AbilityType
    {
        Damage,
        Heal,
        Defensive,
        Interrupt
    }
    
    public enum AbilityTarget
    {
        Enemy,
        Self,
        Ally
    }
    
    /// <summary>
    /// Represents a spell/ability that can be cast by characters
    /// Includes cooldown, interrupt mechanics, and cast time
    /// </summary>
    [CreateAssetMenu(fileName = "New Ability", menuName = "RPG/Ability")]
    public class Ability : ScriptableObject
    {
        [Header("Ability Info")]
        public string abilityName;
        [TextArea(2, 3)]
        public string description;
        public Sprite icon;
        public AbilityType type;
        public AbilityTarget target;
        
        [Header("Mechanics")]
        public float cooldown = 5f;
        public float castTime = 1f;
        public bool canBeInterrupted = true;
        
        [Header("Effects")]
        public float value = 20f; // Damage, heal amount, or shield value
        public float duration = 0f; // For defensive abilities
        
        [Header("Visual")]
        public Color buttonColor = Color.white;
    }
}
