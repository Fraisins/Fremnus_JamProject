using Project.Core.Input;
using VContainer;
using VContainer.Unity;

namespace Project.Infrastructure
{
    public class InputLifetimeScope : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.Register<InputProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<GlobalInputHandler>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}