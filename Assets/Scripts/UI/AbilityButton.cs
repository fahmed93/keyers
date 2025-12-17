using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MobileRPG.UI
{
    /// <summary>
    /// UI display for a single ability button
    /// Shows icon, cooldown, and cast progress
    /// Uses event-driven updates for better performance
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
        
        [Header("Update Settings")]
        [Tooltip("Update UI every N frames (1 = every frame, 5 = every 5 frames)")]
        public int updateInterval = 3;
        
        private Combat.AbilityInstance abilityInstance;
        private System.Action<int> onClickCallback;
        private int abilityIndex;
        private int frameCounter = 0;
        
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
            
            // Subscribe to ability events for immediate updates
            if (abilityInstance != null)
            {
                abilityInstance.OnCooldownComplete += OnCooldownComplete;
                abilityInstance.OnCastComplete += OnCastComplete;
                abilityInstance.OnCastInterrupted += OnCastInterrupted;
            }
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            if (abilityInstance != null)
            {
                abilityInstance.OnCooldownComplete -= OnCooldownComplete;
                abilityInstance.OnCastComplete -= OnCastComplete;
                abilityInstance.OnCastInterrupted -= OnCastInterrupted;
            }
        }
        
        /// <summary>
        /// Update button visuals based on ability state (reduced frequency)
        /// </summary>
        private void Update()
        {
            if (abilityInstance == null) return;
            
            // Only update every N frames to reduce overhead
            frameCounter++;
            if (frameCounter < updateInterval)
                return;
            
            frameCounter = 0;
            UpdateVisuals();
        }
        
        /// <summary>
        /// Update all visual elements
        /// </summary>
        private void UpdateVisuals()
        {
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
        /// Event handler for when cooldown completes
        /// </summary>
        private void OnCooldownComplete(Combat.AbilityInstance instance)
        {
            UpdateVisuals();
        }
        
        /// <summary>
        /// Event handler for when cast completes
        /// </summary>
        private void OnCastComplete(Combat.AbilityInstance instance)
        {
            UpdateVisuals();
        }
        
        /// <summary>
        /// Event handler for when cast is interrupted
        /// </summary>
        private void OnCastInterrupted(Combat.AbilityInstance instance)
        {
            UpdateVisuals();
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
