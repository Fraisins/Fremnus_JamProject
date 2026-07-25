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
            builder.RegisterFactory<GameUIView>(resolver =>
            {
                return () =>
                {
                    GameUIView instance = Instantiate(gameUIPrefab);
                    resolver.InjectGameObject(instance.gameObject);
                    return instance;
                };
            }, Lifetime.Singleton);
            
            builder.RegisterEntryPoint<GameUIHandler>();
        }
    }
}