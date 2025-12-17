using UnityEngine;
using System.Collections.Generic;

namespace MobileRPG.UI
{
    /// <summary>
    /// Main UI manager for combat interface
    /// Manages ability buttons, health bars, and enemy intent
    /// </summary>
    public class CombatUI : MonoBehaviour
    {
        [Header("Player UI")]
        public HealthBar playerHealthBar;
        public Transform abilityButtonContainer;
        public AbilityButton abilityButtonPrefab;
        
        [Header("Enemy UI")]
        public HealthBar enemyHealthBar;
        public EnemyIntentDisplay enemyIntentDisplay;
        
        [Header("References")]
        public Entities.PlayerEntity player;
        public Entities.EnemyEntity enemy;
        
        private List<AbilityButton> abilityButtons = new List<AbilityButton>();
        
        private void Start()
        {
            InitializeUI();
        }
        
        /// <summary>
        /// Initialize all UI elements
        /// </summary>
        private void InitializeUI()
        {
            // Initialize player health bar
            if (playerHealthBar != null && player != null)
            {
                playerHealthBar.Initialize(player);
            }
            
            // Initialize enemy health bar
            if (enemyHealthBar != null && enemy != null)
            {
                enemyHealthBar.Initialize(enemy);
            }
            
            // Initialize enemy intent display
            if (enemyIntentDisplay != null && enemy != null)
            {
                enemyIntentDisplay.Initialize(enemy);
            }
            
            // Initialize ability buttons
            CreateAbilityButtons();
        }
        
        /// <summary>
        /// Create ability buttons for player abilities
        /// </summary>
        private void CreateAbilityButtons()
        {
            if (player == null || abilityButtonContainer == null || abilityButtonPrefab == null)
                return;
            
            // Clear existing buttons
            foreach (var button in abilityButtons)
            {
                if (button != null)
                    Destroy(button.gameObject);
            }
            abilityButtons.Clear();
            
            // Create button for each ability
            var abilities = player.GetAbilityInstances();
            for (int i = 0; i < abilities.Count; i++)
            {
                var button = Instantiate(abilityButtonPrefab, abilityButtonContainer);
                button.Initialize(abilities[i], i, OnAbilityButtonClicked);
                abilityButtons.Add(button);
            }
        }
        
        /// <summary>
        /// Handle ability button click
        /// </summary>
        private void OnAbilityButtonClicked(int abilityIndex)
        {
            if (player != null)
            {
                player.CastAbility(abilityIndex);
            }
        }
    }
}
