using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace MobileRPG.UI
{
    /// <summary>
    /// Simple UI controller to display health bars and stats
    /// </summary>
    public class SimpleUIController : MonoBehaviour
    {
        [Header("Health UI")]
        public Image playerHealthBar;
        public TextMeshProUGUI playerHealthText;
        public Image enemyHealthBar;
        public TextMeshProUGUI enemyHealthText;
        
        private Entities.PlayerEntity player;
        private Entities.EnemyEntity enemy;
        
        void Start()
        {
            // Find player and enemy
            player = FindObjectOfType<Entities.PlayerEntity>();
            enemy = FindObjectOfType<Entities.EnemyEntity>();
            
            // Subscribe to health changes
            if (player != null)
            {
                player.OnHealthChanged += OnPlayerHealthChanged;
            }
            
            if (enemy != null)
            {
                enemy.OnHealthChanged += OnEnemyHealthChanged;
            }
            
            // Initial update
            UpdateUI();
        }
        
        void Update()
        {
            UpdateUI();
        }
        
        void UpdateUI()
        {
            if (player != null)
            {
                float healthPercent = player.maxHealth > 0 ? player.currentHealth / player.maxHealth : 0;
                if (playerHealthBar != null)
                    playerHealthBar.fillAmount = healthPercent;
                if (playerHealthText != null)
                    playerHealthText.text = $"Player HP: {Mathf.RoundToInt(player.currentHealth)}/{Mathf.RoundToInt(player.maxHealth)}";
            }
            
            if (enemy != null)
            {
                float healthPercent = enemy.maxHealth > 0 ? enemy.currentHealth / enemy.maxHealth : 0;
                if (enemyHealthBar != null)
                    enemyHealthBar.fillAmount = healthPercent;
                if (enemyHealthText != null)
                    enemyHealthText.text = $"Enemy HP: {Mathf.RoundToInt(enemy.currentHealth)}/{Mathf.RoundToInt(enemy.maxHealth)}";
            }
        }
        
        void OnPlayerHealthChanged(Entities.CombatEntity entity, float newHealth)
        {
            UpdateUI();
        }
        
        void OnEnemyHealthChanged(Entities.CombatEntity entity, float newHealth)
        {
            UpdateUI();
        }
        
        void OnDestroy()
        {
            if (player != null)
                player.OnHealthChanged -= OnPlayerHealthChanged;
            if (enemy != null)
                enemy.OnHealthChanged -= OnEnemyHealthChanged;
        }
    }
}
