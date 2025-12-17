using UnityEngine;

namespace MobileRPG.Entities
{
    public enum EnemyIntent
    {
        Attack,
        Defend,
        Heal,
        Special
    }
    
    /// <summary>
    /// Enemy combat entity with AI behavior
    /// Shows intent to telegraph next action
    /// </summary>
    public class EnemyEntity : CombatEntity
    {
        [Header("Enemy Specific")]
        public EnemyIntent currentIntent = EnemyIntent.Attack;
        public float aiThinkTime = 2f;
        private float aiTimer = 0f;
        
        [Header("Loot")]
        public int experienceReward = 50;
        public int goldReward = 10;
        
        protected override void Update()
        {
            base.Update();
            
            if (!isAlive) return;
            
            // Simple AI timer
            aiTimer += Time.deltaTime;
            if (aiTimer >= aiThinkTime)
            {
                aiTimer = 0f;
                PerformAction();
            }
        }
        
        /// <summary>
        /// Perform AI action based on current intent
        /// </summary>
        private void PerformAction()
        {
            // Find player in scene
            var player = FindObjectOfType<PlayerEntity>();
            if (player == null || !player.isAlive) return;
            
            switch (currentIntent)
            {
                case EnemyIntent.Attack:
                    // Cast damage ability
                    for (int i = 0; i < abilityInstances.Count; i++)
                    {
                        if (abilityInstances[i].ability.type == ScriptableObjects.AbilityType.Damage)
                        {
                            CastAbility(i);
                            break;
                        }
                    }
                    break;
                    
                case EnemyIntent.Heal:
                    // Cast heal ability
                    for (int i = 0; i < abilityInstances.Count; i++)
                    {
                        if (abilityInstances[i].ability.type == ScriptableObjects.AbilityType.Heal)
                        {
                            CastAbility(i);
                            break;
                        }
                    }
                    break;
                    
                case EnemyIntent.Defend:
                    // Cast defensive ability
                    for (int i = 0; i < abilityInstances.Count; i++)
                    {
                        if (abilityInstances[i].ability.type == ScriptableObjects.AbilityType.Defensive)
                        {
                            CastAbility(i);
                            break;
                        }
                    }
                    break;
            }
            
            // Change intent for next action (simple random)
            ChangeIntent();
        }
        
        /// <summary>
        /// Change enemy intent (simple AI)
        /// </summary>
        private void ChangeIntent()
        {
            float healthPercent = GetHealthPercent();
            
            if (healthPercent < 0.3f)
            {
                // Low health, consider healing or defending
                currentIntent = Random.value > 0.5f ? EnemyIntent.Heal : EnemyIntent.Defend;
            }
            else
            {
                // Mostly attack
                float roll = Random.value;
                if (roll > 0.8f)
                    currentIntent = EnemyIntent.Defend;
                else if (roll > 0.6f)
                    currentIntent = EnemyIntent.Special;
                else
                    currentIntent = EnemyIntent.Attack;
            }
        }
    }
}
