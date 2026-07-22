using Project.Core;
using UnityEngine;
using VContainer;
using Cysharp.Threading.Tasks;

namespace Project.UI
{
    public class MainMenuController : MonoBehaviour
    {
        private ISceneService _sceneService;
        
        [Inject]
        private void Construct(ISceneService sceneService)
        {
            _sceneService = sceneService;
        }

        public void StartGame()
        {
            _sceneService.OpenGame().Forget();
        }
    }
}