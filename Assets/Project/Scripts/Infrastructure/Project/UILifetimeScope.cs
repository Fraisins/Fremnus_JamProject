using System;
using Project.Scripts.UI.Data;
using Project.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Infrastructure
{
    [Serializable]
    public class UILifetimeScope : IInstaller
    {
        [SerializeField] private PersistentUIContainer persistentUIContainer;
        [SerializeField] private LoadScreenSettings loadScreenSettings;
        
        public void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(persistentUIContainer);

            if (loadScreenSettings.UseLoadScreen)
            {
                builder.RegisterComponentInNewPrefab(loadScreenSettings.LoadScreenPrefab, Lifetime.Scoped)
                    .UnderTransform(persistentUIContainer.Container);
                builder.Register<LoadScreenService>(Lifetime.Singleton).AsImplementedInterfaces();
            }
        }
    }
}