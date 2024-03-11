using VContainer;
using VContainer.Unity;

public class RootLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<GameProgressManagement>(Lifetime.Singleton);
        builder.Register<IGameMode, GameModeNormal>(Lifetime.Singleton);
    }
}
