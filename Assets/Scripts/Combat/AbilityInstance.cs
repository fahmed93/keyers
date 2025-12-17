using System;
using UnityEngine;

namespace MobileRPG.Combat
{
    /// <summary>
    /// Tracks the state of an ability instance including cooldown
    /// </summary>
    public class AbilityInstance
    {
        public ScriptableObjects.Ability ability;
        public float cooldownRemaining;
        public bool isOnCooldown => cooldownRemaining > 0f;
        public bool isCasting;
        public float castTimeRemaining;
        
        public event Action<AbilityInstance> OnCooldownComplete;
        public event Action<AbilityInstance> OnCastComplete;
        public event Action<AbilityInstance> OnCastInterrupted;
        
        public AbilityInstance(ScriptableObjects.Ability ability)
        {
            this.ability = ability;
            this.cooldownRemaining = 0f;
            this.isCasting = false;
            this.castTimeRemaining = 0f;
        }
        
        /// <summary>
        /// Start casting this ability
        /// </summary>
        public void StartCast()
        {
            if (isOnCooldown || isCasting) return;
            
            isCasting = true;
            castTimeRemaining = ability.castTime;
        }
        
        /// <summary>
        /// Interrupt the current cast
        /// </summary>
        public void InterruptCast()
        {
            if (!isCasting || !ability.canBeInterrupted) return;
            
            isCasting = false;
            castTimeRemaining = 0f;
            OnCastInterrupted?.Invoke(this);
        }
        
        /// <summary>
        /// Update cast time and cooldown
        /// </summary>
        public void Update(float deltaTime)
        {
            // Update cooldown
            if (cooldownRemaining > 0f)
            {
                cooldownRemaining -= deltaTime;
                if (cooldownRemaining <= 0f)
                {
                    cooldownRemaining = 0f;
                    OnCooldownComplete?.Invoke(this);
                }
            }
            
            // Update cast time
            if (isCasting)
            {
                castTimeRemaining -= deltaTime;
                if (castTimeRemaining <= 0f)
                {
                    CompleteCast();
                }
            }
        }
        
        /// <summary>
        /// Complete the cast and put ability on cooldown
        /// </summary>
        private void CompleteCast()
        {
            isCasting = false;
            castTimeRemaining = 0f;
            cooldownRemaining = ability.cooldown;
            OnCastComplete?.Invoke(this);
        }
        
        /// <summary>
        /// Get cooldown percentage (0-1)
        /// </summary>
        public float GetCooldownPercent()
        {
            if (ability.cooldown <= 0f) return 0f;
            return cooldownRemaining / ability.cooldown;
        }
        
        /// <summary>
        /// Get cast percentage (0-1)
        /// </summary>
        public float GetCastPercent()
        {
            if (ability.castTime <= 0f) return 1f;
            return 1f - (castTimeRemaining / ability.castTime);
        }
    }
}
