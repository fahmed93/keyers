using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MobileRPG.UI
{
    /// <summary>
    /// UI display for a single ability button
    /// Shows icon, cooldown, and cast progress
    /// </summary>
    public class AbilityButton : MonoBehaviour
    {
        [Header("References")]
        public Button button;
        public Image iconImage;
        public Image cooldownOverlay;
        public Image castBar;
        public TextMeshProUGUI cooldownText;
        public Image backgroundImage;
        
        private Combat.AbilityInstance abilityInstance;
        private System.Action<int> onClickCallback;
        private int abilityIndex;
        
        /// <summary>
        /// Initialize the button with an ability
        /// </summary>
        public void Initialize(Combat.AbilityInstance instance, int index, System.Action<int> callback)
        {
            abilityInstance = instance;
            abilityIndex = index;
            onClickCallback = callback;
            
            // Setup visuals
            if (iconImage != null && instance.ability.icon != null)
            {
                iconImage.sprite = instance.ability.icon;
            }
            
            if (backgroundImage != null)
            {
                backgroundImage.color = instance.ability.buttonColor;
            }
            
            // Setup button click
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(OnButtonClick);
            }
            
            // Initialize overlays
            if (cooldownOverlay != null)
            {
                cooldownOverlay.fillAmount = 0f;
            }
            
            if (castBar != null)
            {
                castBar.fillAmount = 0f;
            }
        }
        
        /// <summary>
        /// Update button visuals based on ability state
        /// </summary>
        private void Update()
        {
            if (abilityInstance == null) return;
            
            // Update cooldown overlay
            if (cooldownOverlay != null)
            {
                float cooldownPercent = abilityInstance.GetCooldownPercent();
                cooldownOverlay.fillAmount = cooldownPercent;
                
                if (cooldownText != null)
                {
                    if (cooldownPercent > 0f)
                    {
                        cooldownText.text = Mathf.Ceil(abilityInstance.cooldownRemaining).ToString();
                        cooldownText.gameObject.SetActive(true);
                    }
                    else
                    {
                        cooldownText.gameObject.SetActive(false);
                    }
                }
            }
            
            // Update cast bar
            if (castBar != null)
            {
                if (abilityInstance.isCasting)
                {
                    castBar.fillAmount = abilityInstance.GetCastPercent();
                    castBar.gameObject.SetActive(true);
                }
                else
                {
                    castBar.gameObject.SetActive(false);
                }
            }
            
            // Update button interactivity
            if (button != null)
            {
                button.interactable = !abilityInstance.isOnCooldown && !abilityInstance.isCasting;
            }
        }
        
        /// <summary>
        /// Handle button click
        /// </summary>
        private void OnButtonClick()
        {
            onClickCallback?.Invoke(abilityIndex);
        }
    }
}
