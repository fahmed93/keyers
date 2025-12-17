using UnityEngine;

namespace MobileRPG.Core
{
    /// <summary>
    /// Editor helper to assign assets to GameSetup
    /// </summary>
    public class AssignGameSetupAssets : MonoBehaviour
    {
        void Start()
        {
            GameSetup setup = FindObjectOfType<GameSetup>();
            if (setup != null)
            {
                // Load the character classes from Resources
                setup.playerClass = Resources.Load<ScriptableObjects.CharacterClass>("Classes/WarriorClass");
                setup.enemyClass = Resources.Load<ScriptableObjects.CharacterClass>("Classes/MageClass");
                setup.playerStartLevel = 1;
                setup.enemyLevelOffset = 0;
                
                Debug.Log("GameSetup assets assigned successfully!");
                Debug.Log($"Player Class: {setup.playerClass?.name}");
                Debug.Log($"Enemy Class: {setup.enemyClass?.name}");
                
                // Destroy this helper script after assignment
                Destroy(this);
            }
            else
            {
                Debug.LogError("GameSetup not found!");
            }
        }
    }
}
