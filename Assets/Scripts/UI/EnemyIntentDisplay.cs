using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MobileRPG.UI
{
    /// <summary>
    /// Displays enemy intent to telegraph next action
    /// </summary>
    public class EnemyIntentDisplay : MonoBehaviour
    {
        [Header("References")]
        public Image intentIcon;
        public TextMeshProUGUI intentText;
        
        [Header("Intent Icons")]
        public Sprite attackIcon;
        public Sprite defendIcon;
        public Sprite healIcon;
        public Sprite specialIcon;
        
        private Entities.EnemyEntity enemy;
        
        /// <summary>
        /// Initialize with an enemy
        /// </summary>
        public void Initialize(Entities.EnemyEntity enemyEntity)
        {
            enemy = enemyEntity;
        }
        
        /// <summary>
        /// Update intent display
        /// </summary>
        private void Update()
        {
            if (enemy == null || !enemy.isAlive)
            {
                gameObject.SetActive(false);
                return;
            }
            
            gameObject.SetActive(true);
            UpdateIntent();
        }
        
        /// <summary>
        /// Update intent visuals
        /// </summary>
        private void UpdateIntent()
        {
            switch (enemy.currentIntent)
            {
                case Entities.EnemyIntent.Attack:
                    if (intentIcon != null) intentIcon.sprite = attackIcon;
                    if (intentText != null) intentText.text = "ATTACK";
                    break;
                    
                case Entities.EnemyIntent.Defend:
                    if (intentIcon != null) intentIcon.sprite = defendIcon;
                    if (intentText != null) intentText.text = "DEFEND";
                    break;
                    
                case Entities.EnemyIntent.Heal:
                    if (intentIcon != null) intentIcon.sprite = healIcon;
                    if (intentText != null) intentText.text = "HEAL";
                    break;
                    
                case Entities.EnemyIntent.Special:
                    if (intentIcon != null) intentIcon.sprite = specialIcon;
                    if (intentText != null) intentText.text = "SPECIAL";
                    break;
            }
        }
    }
}
