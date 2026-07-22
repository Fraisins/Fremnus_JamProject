using System;
using Project.Core.Pause;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace Project.Core.Input
{
    public class GlobalInputHandler : IStartable, IDisposable
    {
        private readonly IProjectService _projectService;
        private readonly IPauseService _pauseService;
        private readonly IInputProvider _inputProvider;

        private InputActions Actions => _inputProvider.InputActions;

        public GlobalInputHandler(IProjectService projectService, IPauseService pauseService, 
            IInputProvider inputProvider)
        {
            _projectService = projectService;
            _pauseService = pauseService;
            _inputProvider = inputProvider;
        }
        
        public void Start()
        {
            Actions.Global.Enable();
            Actions.Global.Pause.performed += HandlePause;
        }
        
        public void Dispose()
        {
            Actions.Global.Pause.performed -= HandlePause;
            Actions.Global.Disable();
        }

        private void HandlePause(InputAction.CallbackContext context)
        {
            if (_projectService.State != GameState.Game)
                return;
            _pauseService.TogglePause();
        }
    }
}