using VContainer;
using VContainer.Unity;
using UnityEngine;

[RequireComponent(typeof(ScorePresenter), typeof(ScoreView))]
public class ScoreLifetimeScope : TeamInjectLifetimeScope
{

    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);
        builder.RegisterComponent<ScorePresenter>(this.GetComponent<ScorePresenter>());
        builder.RegisterComponent<ScoreView>(this.GetComponent<ScoreView>());
        builder.Register<IScoreCountable,ScoreCounter>(Lifetime.Singleton);
    }
}
