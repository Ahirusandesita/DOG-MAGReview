using VContainer;
using VContainer.Unity;
using UnityEngine;


[RequireComponent(typeof(Pack),typeof(TransformAdapter))]
public class PackLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<IMovable, MoveLogic>(Lifetime.Singleton);
        builder.Register<IReflectable, ReflectLogic>(Lifetime.Singleton);
        builder.RegisterComponent<Pack>(this.GetComponent<Pack>());
        builder.RegisterInstance<IPositionAdapter, IRotationAdapter, TransformAdapter>(this.GetComponent<TransformAdapter>());
    }
}
