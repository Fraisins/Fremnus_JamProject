using Project.Core;
using Project.Core.Events;
using Project.Game;
using Project.Game.Numbers;
using Project.Game.Robot;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Infrastructure
{
    public class GameDevLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameSettingsSO settings;
        [SerializeField] private GameLevelSettingsSO levelSettings;
        
        [SerializeField] private RobotConveyor conveyor;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<EventBus>(Lifetime.Scoped).AsImplementedInterfaces();
            
            builder.RegisterInstance(settings);
            builder.RegisterInstance(levelSettings);
            builder.RegisterInstance(conveyor);
            
            builder.Register<HpNumbersService>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<ModMachineService>(Lifetime.Scoped).AsImplementedInterfaces();
            
            builder.Register<RobotService>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.RegisterEntryPoint<GameSceneInitializer>(Lifetime.Scoped);
            
            builder.Register<GameService>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
        }
    }
}