using System.Collections;
using UnityEngine;

namespace MobileRPG.Combat
{
    /// <summary>
    /// Manages combat flow, ability execution, and fight-loot-repeat loop
    /// </summary>
    public class CombatManager : MonoBehaviour
    {
        [Header("References")]
        public Entities.PlayerEntity player;
        public Entities.EnemyEntity currentEnemy;
        
        [Header("Combat Settings")]
        public int enemyLevelScaling = 1; // Enemy level = player level + scaling
        public float damageScaling = 10f; // Base damage multiplier for ability calculations
        
        public static CombatManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            SetupCombat();
        }
        
        /// <summary>
        /// Initialize combat
        /// </summary>
        private void SetupCombat()
        {
            if (player != null)
            {
                player.OnAbilityCast += OnPlayerAbilityCast;
                player.OnDeath += OnPlayerDeath;
            }
            
            if (currentEnemy != null)
            {
                currentEnemy.OnAbilityCast += OnEnemyAbilityCast;
                currentEnemy.OnDeath += OnEnemyDeath;
                ScaleEnemyToPlayer();
            }
        }
        
        /// <summary>
        /// Scale enemy level based on player level
        /// </summary>
        private void ScaleEnemyToPlayer()
        {
            if (currentEnemy != null && player != null)
            {
                currentEnemy.level = player.level + enemyLevelScaling;
                currentEnemy.ResetToFullHealth();
            }
        }
        
        /// <summary>
        /// Handle player ability cast completion
        /// </summary>
        private void OnPlayerAbilityCast(Entities.CombatEntity caster, AbilityInstance ability)
        {
            if (currentEnemy == null || !currentEnemy.isAlive) return;
            
            switch (ability.ability.type)
            {
                case ScriptableObjects.AbilityType.Damage:
                    currentEnemy.TakeDamage(ability.ability.value * (player.damage / damageScaling));
                    Debug.Log($"Player dealt {ability.ability.value} damage to enemy");
                    break;
                    
                case ScriptableObjects.AbilityType.Heal:
                    player.Heal(ability.ability.value);
                    Debug.Log($"Player healed for {ability.ability.value}");
                    break;
                    
                case ScriptableObjects.AbilityType.Interrupt:
                    currentEnemy.InterruptCast();
                    Debug.Log("Player interrupted enemy cast");
                    break;
                    
                case ScriptableObjects.AbilityType.Defensive:
                    // Apply defensive buff (simplified)
                    player.defense += ability.ability.value;
                    StartCoroutine(RemoveDefensiveBuff(player, ability.ability.value, ability.ability.duration));
                    Debug.Log($"Player gained {ability.ability.value} defense");
                    break;
            }
        }
        
        /// <summary>
        /// Handle enemy ability cast completion
        /// </summary>
        private void OnEnemyAbilityCast(Entities.CombatEntity caster, AbilityInstance ability)
        {
            if (player == null || !player.isAlive) return;
            
            switch (ability.ability.type)
            {
                case ScriptableObjects.AbilityType.Damage:
                    player.TakeDamage(ability.ability.value * (currentEnemy.damage / damageScaling));
                    Debug.Log($"Enemy dealt {ability.ability.value} damage to player");
                    break;
                    
                case ScriptableObjects.AbilityType.Heal:
                    currentEnemy.Heal(ability.ability.value);
                    Debug.Log($"Enemy healed for {ability.ability.value}");
                    break;
                    
                case ScriptableObjects.AbilityType.Defensive:
                    currentEnemy.defense += ability.ability.value;
                    StartCoroutine(RemoveDefensiveBuff(currentEnemy, ability.ability.value, ability.ability.duration));
                    Debug.Log($"Enemy gained {ability.ability.value} defense");
                    break;
            }
        }
        
        /// <summary>
        /// Remove defensive buff after duration
        /// </summary>
        private IEnumerator RemoveDefensiveBuff(Entities.CombatEntity entity, float amount, float duration)
        {
            yield return new WaitForSeconds(duration);
            entity.defense -= amount;
        }
        
        /// <summary>
        /// Handle enemy death - give loot and spawn new enemy
        /// </summary>
        private void OnEnemyDeath(Entities.CombatEntity enemy)
        {
            var enemyEntity = enemy as Entities.EnemyEntity;
            if (enemyEntity != null && player != null)
            {
                // Award experience
                player.GainExperience(enemyEntity.experienceReward);
                Debug.Log($"Gained {enemyEntity.experienceReward} experience");
                
                // Loot loop - spawn new enemy after delay
                StartCoroutine(SpawnNewEnemy(2f));
            }
        }
        
        /// <summary>
        /// Handle player death - game over
        /// </summary>
        private void OnPlayerDeath(Entities.CombatEntity playerEntity)
        {
            Debug.Log("Player died! Game Over");
            // In a full game, this would show game over screen
        }
        
        /// <summary>
        /// Spawn new enemy for fight-loot-repeat loop
        /// </summary>
        private IEnumerator SpawnNewEnemy(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            if (currentEnemy != null)
            {
                // Reset enemy for new fight
                currentEnemy.Reset();
                ScaleEnemyToPlayer();
                Debug.Log("New enemy spawned!");
            }
        }
    }
}
