using UnityEngine;
using VContainer.Unity;

namespace Project.UI
{
    public class GameUIHandler : IStartable, System.IDisposable
    {
        private readonly GameUIView _gameUI;

        public GameUIHandler(GameUIView gameUI)
        {
            _gameUI = gameUI;
        }

        public void Start()
        {
            _gameUI.gameObject.SetActive(true);
        }
        
        public void Dispose()
        {
            if (_gameUI == null) return;
            _gameUI.gameObject.SetActive(false);
            Object.Destroy(_gameUI.gameObject);
        }
    }
}