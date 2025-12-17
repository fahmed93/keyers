using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MobileRPG.UI
{
    /// <summary>
    /// Displays health bar for a combat entity
    /// </summary>
    public class HealthBar : MonoBehaviour
    {
        [Header("References")]
        public Image fillImage;
        public TextMeshProUGUI healthText;
        public Gradient colorGradient;
        
        private Entities.CombatEntity entity;
        
        /// <summary>
        /// Initialize health bar for an entity
        /// </summary>
        public void Initialize(Entities.CombatEntity combatEntity)
        {
            entity = combatEntity;
            
            if (entity != null)
            {
                entity.OnHealthChanged += OnHealthChanged;
                UpdateHealthBar();
            }
        }
        
        private void OnDestroy()
        {
            if (entity != null)
            {
                entity.OnHealthChanged -= OnHealthChanged;
            }
        }
        
        /// <summary>
        /// Handle health change event
        /// </summary>
        private void OnHealthChanged(Entities.CombatEntity e, float newHealth)
        {
            UpdateHealthBar();
        }
        
        /// <summary>
        /// Update health bar visuals
        /// </summary>
        private void UpdateHealthBar()
        {
            if (entity == null) return;
            
            float healthPercent = entity.GetHealthPercent();
            
            // Update fill amount
            if (fillImage != null)
            {
                fillImage.fillAmount = healthPercent;
                
                // Update color based on health
                if (colorGradient != null)
                {
                    fillImage.color = colorGradient.Evaluate(healthPercent);
                }
            }
            
            // Update text
            if (healthText != null)
            {
                healthText.text = $"{Mathf.RoundToInt(entity.currentHealth)}/{Mathf.RoundToInt(entity.maxHealth)}";
            }
        }
    }
}
