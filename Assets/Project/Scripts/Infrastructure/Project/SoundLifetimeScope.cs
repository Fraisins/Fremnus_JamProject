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
        
        public void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(sources);
            builder.Register<SoundService>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}