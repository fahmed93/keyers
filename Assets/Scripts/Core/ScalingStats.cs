using System;
using UnityEngine;

namespace MobileRPG.Core
{
    /// <summary>
    /// Base stats for characters that scale with level
    /// Formula: FinalStat = BaseStat * (Level ^ ScaleFactor)
    /// </summary>
    [Serializable]
    public class ScalingStats
    {
        [Header("Base Stats")]
        public float baseHealth = 100f;
        public float baseDamage = 10f;
        public float baseDefense = 5f;
        
        [Header("Scaling Factors")]
        [Range(0.5f, 2f)]
        public float healthScaleFactor = 1.1f;
        [Range(0.5f, 2f)]
        public float damageScaleFactor = 1.05f;
        [Range(0.5f, 2f)]
        public float defenseScaleFactor = 1.05f;
        
        /// <summary>
        /// Calculate scaled stat value based on level
        /// </summary>
        public float GetScaledHealth(int level)
        {
            return baseHealth * Mathf.Pow(level, healthScaleFactor);
        }
        
        public float GetScaledDamage(int level)
        {
            return baseDamage * Mathf.Pow(level, damageScaleFactor);
        }
        
        public float GetScaledDefense(int level)
        {
            return baseDefense * Mathf.Pow(level, defenseScaleFactor);
        }
    }
}
