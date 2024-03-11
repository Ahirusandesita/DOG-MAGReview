using VContainer;
using VContainer.Unity;
using UnityEngine;

[RequireComponent(typeof(TimeCountDownPresenter))]
public class TimeCountDownLifetimeScope : LifetimeScope
{
    [SerializeField]
    private NumberDigitManager numberDigitManager;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent<TimeCountDownPresenter>(this.GetComponent<TimeCountDownPresenter>());
        builder.RegisterComponent<NumberDigitManager>(numberDigitManager);
        builder.Register<ICountDownNotificationable, ICountDownable, TimeCountDownLogic>(Lifetime.Singleton);
        builder.RegisterEntryPoint<TimeCountDownEntryPoint>(Lifetime.Singleton);
    }
}
