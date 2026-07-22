using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

using Project.Core;

namespace Project.Game
{
    public class GameService
    {
        private readonly ISceneService _sceneService;
        
        public GameService(ISceneService sceneService)
        {
            _sceneService = sceneService;
        }
    }
}