using System;
using Project.Sound;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Infrastructure
{
    [Serializable]
    public class SoundLifetimeScope : IInstaller
    {
        [SerializeField] private SoundSources sources;
        [SerializeField] private MusicSettingsSO musicSettings;
        
        public void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(sources);
            builder.RegisterInstance(musicSettings);
            
            builder.Register<SoundService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<MusicHandler>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}