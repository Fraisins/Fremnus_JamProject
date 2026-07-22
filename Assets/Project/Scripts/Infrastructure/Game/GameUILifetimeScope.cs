using System;
using Project.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Infrastructure
{
    public class GameUILifetimeScope : LifetimeScope
    {
        [SerializeField] private GameUIView gameUIPrefab;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab(gameUIPrefab, Lifetime.Scoped);
            builder.RegisterEntryPoint<GameUIHandler>(Lifetime.Scoped);
            builder.RegisterEntryPoint<PauseMenuHandler>(Lifetime.Scoped);
        }
    }
}