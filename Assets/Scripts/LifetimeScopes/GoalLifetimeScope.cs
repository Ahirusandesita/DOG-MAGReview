using VContainer;
using VContainer.Unity;
using UnityEngine;

[RequireComponent(typeof(Goal))]
public class GoalLifetimeScope : TeamInjectLifetimeScope
{


    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);
        builder.RegisterComponent<Goal>(this.GetComponent<Goal>());

        //Debug
        if (TryGetComponent<GoalUI>(out GoalUI goalUI))
        {
            builder.RegisterComponent<GoalUI>(goalUI);
        }
    }
}
