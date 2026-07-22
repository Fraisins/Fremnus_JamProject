using Cysharp.Threading.Tasks;
using Project.Core.Pause;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;
using Project.Data;

namespace Project.Core
{
    public class SceneService : ISceneService
    {
        private readonly ProjectConfigSO _config;
        private readonly LifetimeScope _rootScope;
        private readonly IProjectService _projectService;
        private readonly IPauseService _pauseService;
        
        private string _currentSceneName;
        private LifetimeScope _gameScope;
        
        public SceneService(ProjectConfigSO config, IProjectService projectService, LifetimeScope rootScope,
            IPauseService pauseService)
        {
            _rootScope = rootScope;
            _config = config;
            _projectService = projectService;
            _pauseService = pauseService;
        }

        public async UniTask OpenMenu()
        {
            if (_projectService.State == GameState.Menu || _projectService.State == GameState.Loading) 
                return;
            
            _projectService.SetState(GameState.Loading);
            //await UniTask.WaitForSeconds(5, ignoreTimeScale: true);
            await LoadAdditive(_config.MainMenuSceneName);
            
            if (_gameScope != null)
            {
                Object.Destroy(_gameScope.gameObject);
                _gameScope = null;
            }
            
            await UnloadAdditive(_config.GameSceneName);
            _projectService.SetState(GameState.Menu);
            
            _pauseService.Resume(); // resume if paused
        }

        public async UniTask OpenGame()
        {
            _projectService.SetState(GameState.Loading);
            await LoadAdditive(_config.GameSceneName);
            
            if (_config.GameScopePrefab == null)
                throw new System.NullReferenceException("[SceneService] GameScopePrefab is missing!");
            
            _gameScope = _rootScope.CreateChildFromPrefab(_config.GameScopePrefab);
            
            await UnloadAdditive(_config.MainMenuSceneName);
            _projectService.SetState(GameState.Game);
        }

        #region Scene Load / Unload
        
        private async UniTask LoadAdditive(string sceneName)
        {
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            
            Scene scene = SceneManager.GetSceneByName(sceneName);
            SceneManager.SetActiveScene(scene);

            _currentSceneName = sceneName;
        }

        private async UniTask UnloadAdditive(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName) || !IsSceneLoaded(sceneName)) 
                return;
            
            await SceneManager.UnloadSceneAsync(sceneName);
        }

        private static bool IsSceneLoaded(string sceneName)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            return scene.IsValid() && scene.isLoaded;
        }
        
        #endregion
    }
}