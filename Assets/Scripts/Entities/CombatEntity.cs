using System;
using System.Collections.Generic;
using UnityEngine;

namespace MobileRPG.Entities
{
    /// <summary>
    /// Base class for all combat entities (Player, Enemy)
    /// Manages health, stats, abilities, and combat state
    /// </summary>
    public class CombatEntity : MonoBehaviour
    {
        [Header("Configuration")]
        public ScriptableObjects.CharacterClass characterClass;
        public int level = 1;
        
        [Header("Current State")]
        public float currentHealth;
        public float maxHealth;
        public float damage;
        public float defense;
        public bool isAlive = true;
        
        protected List<Combat.AbilityInstance> abilityInstances = new List<Combat.AbilityInstance>();
        
        public event Action<CombatEntity, float> OnHealthChanged;
        public event Action<CombatEntity> OnDeath;
        public event Action<CombatEntity, Combat.AbilityInstance> OnAbilityCast;
        
        protected virtual void Awake()
        {
            InitializeStats();
            InitializeAbilities();
        }
        
        protected virtual void Update()
        {
            UpdateAbilities();
        }
        
        /// <summary>
        /// Initialize stats based on class and level
        /// </summary>
        public virtual void InitializeStats()
        {
            if (characterClass == null) return;
            
            maxHealth = characterClass.stats.GetScaledHealth(level);
            damage = characterClass.stats.GetScaledDamage(level);
            defense = characterClass.stats.GetScaledDefense(level);
            currentHealth = maxHealth;
        }
        
        /// <summary>
        /// Initialize ability instances from character class
        /// </summary>
        public virtual void InitializeAbilities()
        {
            if (characterClass == null || characterClass.abilities == null) return;
            
            abilityInstances.Clear();
            foreach (var ability in characterClass.abilities)
            {
                if (ability != null)
                {
                    var instance = new Combat.AbilityInstance(ability);
                    instance.OnCastComplete += OnAbilityCastComplete;
                    abilityInstances.Add(instance);
                }
            }
        }
        
        /// <summary>
        /// Update all ability cooldowns and casts
        /// </summary>
        protected virtual void UpdateAbilities()
        {
            foreach (var abilityInstance in abilityInstances)
            {
                abilityInstance.Update(Time.deltaTime);
            }
        }
        
        /// <summary>
        /// Take damage and update health
        /// </summary>
        public virtual void TakeDamage(float amount)
        {
            if (!isAlive) return;
            
            float damageAfterDefense = Mathf.Max(0, amount - defense);
            currentHealth -= damageAfterDefense;
            currentHealth = Mathf.Max(0, currentHealth);
            
            OnHealthChanged?.Invoke(this, currentHealth);
            
            if (currentHealth <= 0)
            {
                Die();
            }
        }
        
        /// <summary>
        /// Heal this entity
        /// </summary>
        public virtual void Heal(float amount)
        {
            if (!isAlive) return;
            
            currentHealth += amount;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            
            OnHealthChanged?.Invoke(this, currentHealth);
        }
        
        /// <summary>
        /// Handle death
        /// </summary>
        protected virtual void Die()
        {
            isAlive = false;
            OnDeath?.Invoke(this);
        }
        
        /// <summary>
        /// Cast an ability by index
        /// </summary>
        public virtual bool CastAbility(int abilityIndex)
        {
            if (!isAlive || abilityIndex < 0 || abilityIndex >= abilityInstances.Count)
                return false;
            
            var abilityInstance = abilityInstances[abilityIndex];
            if (abilityInstance.isOnCooldown || abilityInstance.isCasting)
                return false;
            
            abilityInstance.StartCast();
            return true;
        }
        
        /// <summary>
        /// Called when an ability completes casting
        /// </summary>
        protected virtual void OnAbilityCastComplete(Combat.AbilityInstance abilityInstance)
        {
            OnAbilityCast?.Invoke(this, abilityInstance);
        }
        
        /// <summary>
        /// Interrupt current cast
        /// </summary>
        public virtual void InterruptCast()
        {
            foreach (var abilityInstance in abilityInstances)
            {
                if (abilityInstance.isCasting)
                {
                    abilityInstance.InterruptCast();
                }
            }
        }
        
        /// <summary>
        /// Get all ability instances
        /// </summary>
        public List<Combat.AbilityInstance> GetAbilityInstances()
        {
            return abilityInstances;
        }
        
        /// <summary>
        /// Get health percentage
        /// </summary>
        public float GetHealthPercent()
        {
            return maxHealth > 0 ? currentHealth / maxHealth : 0f;
        }
        
        /// <summary>
        /// Reset entity to full health and reinitialize stats
        /// </summary>
        public virtual void ResetToFullHealth()
        {
            InitializeStats();
            currentHealth = maxHealth;
            isAlive = true;
        }
    }
}
