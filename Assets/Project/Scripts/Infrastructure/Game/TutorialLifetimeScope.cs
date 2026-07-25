using Project.UI.Tutorial;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Infrastructure
{
    public class TutorialLifetimeScope : LifetimeScope
    {
        [SerializeField] private TutorialLevelManager tutorialLevelManager;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(tutorialLevelManager);
            builder.RegisterEntryPoint<TutorialHandler>();
        }
    }
}