using UnityEngine;

namespace MobileRPG.Entities
{
    /// <summary>
    /// Player-controlled combat entity
    /// </summary>
    public class PlayerEntity : CombatEntity
    {
        [Header("Player Specific")]
        public int experience = 0;
        public int experienceToNextLevel = 100;
        
        protected override void Awake()
        {
            base.Awake();
        }
        
        /// <summary>
        /// Gain experience and potentially level up
        /// </summary>
        public void GainExperience(int amount)
        {
            experience += amount;
            
            while (experience >= experienceToNextLevel)
            {
                LevelUp();
            }
        }
        
        /// <summary>
        /// Level up the player
        /// </summary>
        private void LevelUp()
        {
            experience -= experienceToNextLevel;
            level++;
            experienceToNextLevel = Mathf.RoundToInt(experienceToNextLevel * 1.5f);
            
            // Recalculate stats
            InitializeStats();
            currentHealth = maxHealth; // Full heal on level up
            
            Debug.Log($"Level Up! Now level {level}");
        }
    }
}
