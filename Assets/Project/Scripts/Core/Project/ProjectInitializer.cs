using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Core;
using UnityEngine;
using VContainer.Unity;

namespace Project.Game
{
    public class ProjectInitializer : IAsyncStartable
    {
        private readonly ISceneService _sceneService;
        
        public ProjectInitializer(ISceneService sceneService)
        {
            _sceneService = sceneService;
        }
        
        public async UniTask StartAsync(CancellationToken cancellation = new())
        {
            //await _sceneService.OpenGameFromMenu();
            await _sceneService.OpenMenuFromGame();
        }
    }
}