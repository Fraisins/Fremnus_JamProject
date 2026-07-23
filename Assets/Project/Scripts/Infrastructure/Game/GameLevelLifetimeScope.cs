using Project.Core;
using Project.Game;
using Project.Game.Robot;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Infrastructure
{
    public class GameLevelLifetimeScope : LifetimeScope
    {
        [SerializeField] private RobotConveyor conveyor;
        
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.RegisterInstance(conveyor);
            builder.RegisterEntryPoint<GameSceneInitializer>(Lifetime.Scoped);
        }
    }
}