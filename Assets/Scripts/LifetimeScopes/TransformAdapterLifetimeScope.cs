using VContainer;
using VContainer.Unity;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(TransformAdapter))]
public class TransformAdapterLifetimeScope : LifetimeScope
{
    [SerializeField]
    private List<TransformAdapterInject> transformAdapterInjects = new List<TransformAdapterInject>();
    public TransformAdapterInject TransformAdapterInject
    {
        set
        {
            transformAdapterInjects.Add(value);
        }
    }



    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance<IPositionAdapter, IRotationAdapter, TransformAdapter>(this.GetComponent<TransformAdapter>());
        foreach (TransformAdapterInject transformAdapterInject in transformAdapterInjects)
        {
            builder.RegisterInstance<TransformAdapterInject>(transformAdapterInject);
        }
    }
}
