using UnityEngine;
using VContainer;
using VContainer.Unity;

using Project.Core;
using Project.Core.Events;
using Project.Core.Game;
using Project.Core.Pause;
using Project.Core.SaveSystem;
using Project.Data;
using Project.Game;
using Project.UI;

namespace Project.Infrastructure
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private ProjectConfigSO projectConfigSo;
        [SerializeField] private UILifetimeScope uiScope;
        [SerializeField] private SoundLifetimeScope soundScope;
        [SerializeField] private GameSettingsSO gameSettingsSo;
        
        protected override void Configure(IContainerBuilder builder)
        {
            // REGISTER EVENT BUS
            builder.Register<EventBus>(Lifetime.Singleton).AsImplementedInterfaces();
            
            // REGISTER SOUND
            soundScope.Install(builder);
            
            // REGISTER LOADING UI
            uiScope.Install(builder);
            
            // REGISTER PROJECT
            builder.RegisterInstance(projectConfigSo);
            builder.Register<SceneService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ProjectService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ProjectInitializer>(Lifetime.Singleton).AsImplementedInterfaces();
            
            // REGISTER PAUSE
            builder.Register<PauseService>(Lifetime.Singleton).AsImplementedInterfaces();
            
            // REGISTER INPUT
            new InputLifetimeScope().Install(builder);
            
            // LEVEL CHOICE HOLDER
            builder.Register<GameLevelChoiceHolder>(Lifetime.Singleton);
            
            // REGISTER SAVE
            builder.Register<SaveService>(Lifetime.Singleton).AsImplementedInterfaces();
            
            // REGISTER GAME DATA
            builder.RegisterInstance(gameSettingsSo);
        }
    }
}