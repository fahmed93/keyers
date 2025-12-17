using UnityEngine;

namespace MobileRPG.Core
{
    /// <summary>
    /// Helper script to quickly set up a basic game instance
    /// Attach to a GameObject in the scene to auto-initialize the game
    /// </summary>
    public class GameSetup : MonoBehaviour
    {
        [Header("Scene Setup")]
        [Tooltip("Automatically create player and enemy if not assigned")]
        public bool autoCreateEntities = true;
        
        [Header("Player Setup")]
        public ScriptableObjects.CharacterClass playerClass;
        public int playerStartLevel = 1;
        
        [Header("Enemy Setup")]
        public ScriptableObjects.CharacterClass enemyClass;
        public int enemyLevelOffset = 0;
        
        [Header("Prefab References (Optional)")]
        public GameObject playerPrefab;
        public GameObject enemyPrefab;
        public GameObject uiPrefab;
        
        private void Start()
        {
            if (autoCreateEntities)
            {
                SetupGame();
            }
        }
        
        /// <summary>
        /// Set up the basic game scene
        /// </summary>
        private void SetupGame()
        {
            // Create or find player
            Entities.PlayerEntity player = FindObjectOfType<Entities.PlayerEntity>();
            if (player == null && playerClass != null)
            {
                GameObject playerObj = playerPrefab != null ? Instantiate(playerPrefab) : new GameObject("Player");
                player = playerObj.GetComponent<Entities.PlayerEntity>();
                if (player == null)
                    player = playerObj.AddComponent<Entities.PlayerEntity>();
                
                player.characterClass = playerClass;
                player.level = playerStartLevel;
                playerObj.transform.position = new Vector3(-3f, 0f, 0f);
                
                Debug.Log("Player created successfully");
            }
            
            // Create or find enemy
            Entities.EnemyEntity enemy = FindObjectOfType<Entities.EnemyEntity>();
            if (enemy == null && enemyClass != null)
            {
                GameObject enemyObj = enemyPrefab != null ? Instantiate(enemyPrefab) : new GameObject("Enemy");
                enemy = enemyObj.GetComponent<Entities.EnemyEntity>();
                if (enemy == null)
                    enemy = enemyObj.AddComponent<Entities.EnemyEntity>();
                
                enemy.characterClass = enemyClass;
                enemy.level = playerStartLevel + enemyLevelOffset;
                enemyObj.transform.position = new Vector3(3f, 0f, 0f);
                
                Debug.Log("Enemy created successfully");
            }
            
            // Create or find combat manager
            Combat.CombatManager combatManager = FindObjectOfType<Combat.CombatManager>();
            if (combatManager == null)
            {
                GameObject managerObj = new GameObject("CombatManager");
                combatManager = managerObj.AddComponent<Combat.CombatManager>();
                combatManager.player = player;
                combatManager.currentEnemy = enemy;
                combatManager.enemyLevelScaling = enemyLevelOffset;
                
                Debug.Log("Combat Manager created successfully");
            }
            
            // Create UI if prefab provided
            if (uiPrefab != null && FindObjectOfType<UI.CombatUI>() == null)
            {
                GameObject uiObj = Instantiate(uiPrefab);
                UI.CombatUI combatUI = uiObj.GetComponent<UI.CombatUI>();
                if (combatUI != null)
                {
                    combatUI.player = player;
                    combatUI.enemy = enemy;
                }
                
                Debug.Log("Combat UI created successfully");
            }
            
            Debug.Log("Game setup complete!");
            LogGameState(player, enemy);
        }
        
        /// <summary>
        /// Log current game state for debugging
        /// </summary>
        private void LogGameState(Entities.PlayerEntity player, Entities.EnemyEntity enemy)
        {
            if (player != null)
            {
                Debug.Log($"Player - Level: {player.level}, HP: {player.maxHealth}, Damage: {player.damage}, Defense: {player.defense}");
                Debug.Log($"Player Abilities: {player.GetAbilityInstances().Count}");
            }
            
            if (enemy != null)
            {
                Debug.Log($"Enemy - Level: {enemy.level}, HP: {enemy.maxHealth}, Damage: {enemy.damage}, Defense: {enemy.defense}");
                Debug.Log($"Enemy Abilities: {enemy.GetAbilityInstances().Count}");
            }
        }
    }
}
