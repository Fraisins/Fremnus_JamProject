using Project.Core;
using Project.Game;
using Project.Game.Numbers;
using Project.Game.Robot;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Infrastructure
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameSettingsSO settings;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(settings);
            
            builder.Register<GameService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            
            builder.Register<HpNumbersService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ModMachineService>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<RobotService>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}