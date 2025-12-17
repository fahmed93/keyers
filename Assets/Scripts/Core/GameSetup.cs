using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MobileRPG.Entities;
using MobileRPG.Combat;

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
            // Set up camera for 2D
            SetupCamera();
            
            // Auto-load character classes from Resources if not assigned
            if (playerClass == null)
            {
                playerClass = Resources.Load<ScriptableObjects.CharacterClass>("Classes/WarriorClass");
                Debug.Log($"Auto-loaded playerClass: {playerClass?.name}");
            }
            
            if (enemyClass == null)
            {
                enemyClass = Resources.Load<ScriptableObjects.CharacterClass>("Classes/MageClass");
                Debug.Log($"Auto-loaded enemyClass: {enemyClass?.name}");
            }
            
            if (autoCreateEntities)
            {
                SetupGame();
            }
        }
        
        /// <summary>
        /// Configure camera for 2D view
        /// </summary>
        private void SetupCamera()
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                mainCamera.orthographic = true;
                mainCamera.orthographicSize = 5f;
                mainCamera.transform.position = new Vector3(0f, 0f, -10f);
                Debug.Log("Camera configured for 2D view");
            }
        }
        
        /// <summary>
        /// Create a simple square sprite for visual representation
        /// </summary>
        private Sprite CreateSquareSprite()
        {
            // Create a 1x1 white texture
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            
            // Create sprite from texture
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
            return sprite;
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
                
                // Add sprite renderer for visual representation
                SpriteRenderer playerSprite = playerObj.GetComponent<SpriteRenderer>();
                if (playerSprite == null)
                {
                    playerSprite = playerObj.AddComponent<SpriteRenderer>();
                }
                
                // Create a simple square sprite
                playerSprite.sprite = CreateSquareSprite();
                playerSprite.color = new Color(0.3f, 0.6f, 1.0f); // Blue for player
                
                player.characterClass = playerClass;
                player.level = playerStartLevel;
                playerObj.transform.position = new Vector3(-3f, 0f, 0f);
                
                // Manually initialize stats after assigning character class
                player.InitializeStats();
                player.InitializeAbilities();
                
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
                
                // Add sprite renderer for visual representation
                SpriteRenderer enemySprite = enemyObj.GetComponent<SpriteRenderer>();
                if (enemySprite == null)
                {
                    enemySprite = enemyObj.AddComponent<SpriteRenderer>();
                }
                
                // Create a simple square sprite
                enemySprite.sprite = CreateSquareSprite();
                enemySprite.color = new Color(1.0f, 0.3f, 0.3f); // Red for enemy
                
                enemy.characterClass = enemyClass;
                enemy.level = playerStartLevel + enemyLevelOffset;
                enemyObj.transform.position = new Vector3(3f, 0f, 0f);
                
                // Manually initialize stats after assigning character class
                enemy.InitializeStats();
                enemy.InitializeAbilities();
                
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
            else if (uiPrefab == null)
            {
                // Create simple UI programmatically if no prefab
                CreateSimpleUI(player, enemy);
            }
            
            Debug.Log("Game setup complete!");
            LogGameState(player, enemy);
        }
        
        private void CreateSimpleUI(PlayerEntity player, EnemyEntity enemy)
        {
            // Find or create Canvas
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("No Canvas found in scene! Please add a Canvas GameObject.");
                return;
            }
            
            // Create health bars at top
            CreateHealthBar(canvas, player, true);  // Left side for player
            CreateHealthBar(canvas, enemy, false);  // Right side for enemy
            
            // Create ability buttons at bottom
            CreateAbilityButtons(canvas, player);
            
            Debug.Log("Simple UI created successfully");
        }
        
        private void CreateHealthBar(Canvas canvas, CombatEntity entity, bool isPlayer)
        {
            // Create container
            GameObject healthBarObj = new GameObject(isPlayer ? "PlayerHealthBar" : "EnemyHealthBar");
            healthBarObj.transform.SetParent(canvas.transform, false);
            
            RectTransform rectTransform = healthBarObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(150, 25);
            
            // Position at top (left for player, right for enemy)
            rectTransform.anchorMin = new Vector2(isPlayer ? 0 : 1, 1);
            rectTransform.anchorMax = new Vector2(isPlayer ? 0 : 1, 1);
            rectTransform.pivot = new Vector2(isPlayer ? 0 : 1, 1);
            rectTransform.anchoredPosition = new Vector2(isPlayer ? 10 : -10, -10);
            
            // Background
            GameObject bgObj = new GameObject("Background");
            bgObj.transform.SetParent(healthBarObj.transform, false);
            Image bgImage = bgObj.AddComponent<Image>();
            bgImage.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
            RectTransform bgRect = bgObj.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;
            
            // Fill bar
            GameObject fillObj = new GameObject("Fill");
            fillObj.transform.SetParent(healthBarObj.transform, false);
            Image fillImage = fillObj.AddComponent<Image>();
            fillImage.color = isPlayer ? new Color(0, 1, 0, 1f) : new Color(1, 0, 0, 1f);
            fillImage.type = Image.Type.Filled;
            fillImage.fillMethod = Image.FillMethod.Horizontal;
            fillImage.fillAmount = 1f;
            RectTransform fillRect = fillObj.GetComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.offsetMin = new Vector2(2, 2);
            fillRect.offsetMax = new Vector2(-2, -2);
            
            // Text label (on top of the bar)
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(healthBarObj.transform, false);
            Text text = textObj.AddComponent<Text>();
            text.text = $"{entity.currentHealth:F0}/{entity.maxHealth:F0}";
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 12;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
            
            Debug.Log($"Created health bar for {(isPlayer ? "Player" : "Enemy")}");
        }
        
        private void CreateAbilityButtons(Canvas canvas, PlayerEntity player)
        {
            List<AbilityInstance> abilities = player.GetAbilityInstances();
            int buttonCount = Mathf.Min(4, abilities.Count);
            
            for (int i = 0; i < buttonCount; i++)
            {
                CreateAbilityButton(canvas, abilities[i], i, buttonCount);
            }
        }
        
        private void CreateAbilityButton(Canvas canvas, AbilityInstance ability, int index, int totalButtons)
        {
            GameObject buttonObj = new GameObject($"AbilityButton{index}");
            buttonObj.transform.SetParent(canvas.transform, false);
            
            RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(150, 50);
            
            // Position at bottom, evenly spaced
            rectTransform.anchorMin = new Vector2(0.5f, 0);
            rectTransform.anchorMax = new Vector2(0.5f, 0);
            rectTransform.pivot = new Vector2(0.5f, 0);
            
            float spacing = 160;
            float totalWidth = (totalButtons - 1) * spacing;
            float xPos = -totalWidth / 2 + (index * spacing);
            rectTransform.anchoredPosition = new Vector2(xPos, 10);
            
            // Add Button component
            Button button = buttonObj.AddComponent<Button>();
            Image buttonImage = buttonObj.AddComponent<Image>();
            buttonImage.color = new Color(0.3f, 0.3f, 0.8f, 0.8f);
            
            // Add Text
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);
            Text text = textObj.AddComponent<Text>();
            text.text = ability.ability.abilityName;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 14;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
            
            // Add click handler
            button.onClick.AddListener(() => {
                if (!ability.isOnCooldown && !ability.isCasting)
                {
                    ability.StartCast();
                    Debug.Log($"Casting ability: {ability.ability.abilityName}");
                }
                else
                {
                    Debug.Log($"Ability {ability.ability.abilityName} is on cooldown or casting");
                }
            });
            
            Debug.Log($"Created button for ability: {ability.ability.abilityName}");
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
