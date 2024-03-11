using VContainer;
using VContainer.Unity;
using UnityEngine;
[RequireComponent(typeof(TimePresenter))]
public class CountDownLifetimeScope : LifetimeScope
{
    [SerializeField]
    private TimeView timeView;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent<TimePresenter>(this.GetComponent<TimePresenter>());
        builder.RegisterComponent<TimeView>(timeView);
        builder.Register<ICountDownNotificationable, ICountDownable, StartCountDownLogic>(Lifetime.Singleton);
        builder.RegisterEntryPoint<StartTimeCountDownEntryPoint>(Lifetime.Singleton);
    }
}
