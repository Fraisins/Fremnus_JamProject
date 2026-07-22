using Project.Game;
using VContainer;
using VContainer.Unity;

namespace Project.Infrastructure
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<GameService>(Lifetime.Scoped).AsImplementedInterfaces();
        }
    }
}