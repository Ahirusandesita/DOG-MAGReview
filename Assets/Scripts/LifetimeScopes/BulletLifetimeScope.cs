using VContainer;
using VContainer.Unity;
using UnityEngine;

[RequireComponent(typeof(Bullet), typeof(TransformAdapter),typeof(TransformAdapterInject))]
public class BulletLifetimeScope : LifetimeScope
{
    [SerializeField]
    private Bullet bullet;
    [SerializeField]
    private TransformAdapter transformAdapter;

    private BulletParamater bulletParamater;
    public BulletParamater Paramater
    {
        set
        {
            bulletParamater = value;
        }
        get
        {
            return bulletParamater;
        }
    }
    private TeamInfo teamInfo;
    public TeamInfo TeamInfo
    {
        set
        {
            teamInfo = value;
        }
    }

    private bool canBuild = true;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent<Bullet>(bullet).WithParameter(this.GetComponent<TransformAdapterInject>());
        builder.RegisterComponent<ITransformAdapter>(transformAdapter);
        builder.RegisterComponent<IRotationAdapter>(transformAdapter);
        builder.RegisterComponent<IPositionAdapter>(transformAdapter);
        builder.RegisterInstance<BulletParamater>(bulletParamater);
        builder.Register<IMovable, MoveLogic>(Lifetime.Singleton);
        builder.Register<IReflectable, ReflectLogic>(Lifetime.Singleton);
        builder.RegisterInstance<TeamInfo>(teamInfo);
    }

    public new void Build()
    {
        if (canBuild)
        {
            base.Build();
        }
        canBuild = false;
    }
}
