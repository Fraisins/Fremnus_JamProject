using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace Project.Data
{
    [CreateAssetMenu(menuName = "GameConfigs/Project/ProjectConfig", fileName = "ProjectConfig")]
    public class ProjectConfigSO : ScriptableObject
    {
        [Header("Scenes")]
        [SerializeField] private string mainMenuSceneName;
        [SerializeField] private string gameSceneName;
        
        [Header("Game")]
        [SerializeField] private LifetimeScope gameScopePrefab;
        
        public string MainMenuSceneName => mainMenuSceneName;
        public string GameSceneName => gameSceneName;
        public LifetimeScope GameScopePrefab => gameScopePrefab;
    }
}