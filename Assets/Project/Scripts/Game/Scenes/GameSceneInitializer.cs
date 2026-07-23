using Project.Core.Events;
using Project.Core.Robot;
using Project.Game;
using Project.Game.Robot;
using VContainer.Unity;

namespace Project.Core
{
    public class GameSceneInitializer : IInitializable
    {
        private class Robot
        {
            private readonly IRobotService _robotService;
            private readonly RobotConveyor _robotConveyor;
            //private readonly GameLevelSettingsSO _levelSettings;

            public Robot(IRobotService robotService, RobotConveyor conveyor)
            {
                _robotService = robotService;
                _robotConveyor = conveyor;
                //_levelSettings = levelSettings;
            }
            
            public void Initialize()
            {
                _robotService.SetConveyor(_robotConveyor);
                //_robotService.SetLevelSettings(_levelSettings);
            }
        }

        private readonly IEventBus _eventBus;
        private readonly GameService _gameService;
        private readonly Robot _robot;

        public GameSceneInitializer(IRobotService robotService, RobotConveyor conveyor,
            GameService gameService)
        {
            _gameService = gameService;
            _robot = new Robot(robotService, conveyor);
        }
        
        public void Initialize()
        {
            _robot.Initialize();
            _gameService.InitScene();
        }
    }
}