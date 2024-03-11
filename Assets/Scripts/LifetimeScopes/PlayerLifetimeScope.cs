using VContainer;
using VContainer.Unity;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(TransformAdapter), typeof(PlayerPresenter), typeof(PlayerView))]
[RequireComponent(typeof(PlayerMove), typeof(Shot),typeof(Status))]
public class PlayerLifetimeScope : LifetimeScope
{
    [SerializeField]
    private TransformAdapter transformAdapter;
    [SerializeField]
    private PlayerPresenter playerPresenter;
    [SerializeField]
    private PlayerView playerView;
    [SerializeField]
    private PlayerMove playerMoveLogic;

    private CharacterParamater paramater;
    public CharacterParamater InGameParamater
    {
        set
        {
            paramater = value;
        }
        get
        {
            return paramater;
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
    private InputSystemUseObject inputSystemUseObject;
    public InputSystemUseObject InputSystemUseObject
    {
        set
        {
            inputSystemUseObject = value;
        }
    }
    private IReadOnlyPositionAdapter readOnlyPositionAdapter;
    public IReadOnlyPositionAdapter ReadOnlyPositionAdapter
    {
        set
        {
            readOnlyPositionAdapter = value;
        }
    }

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance<IPositionAdapter, IRotationAdapter, TransformAdapter>(transformAdapter);
        builder.RegisterInstance<CharacterParamater>(paramater);
        builder.RegisterComponent<PlayerPresenter>(playerPresenter);
        builder.RegisterComponent<PlayerView>(playerView);
        builder.Register<IMovable, MoveLogic>(Lifetime.Transient).WithParameter(this.transform);
        builder.RegisterInstance<BulletParamater>(paramater.BulletParamater);
        builder.RegisterInstance<TeamInfo>(teamInfo);
        builder.RegisterComponent<InputSystemUseObject>(inputSystemUseObject);
        builder.RegisterComponent<IShot>(this.GetComponent<Shot>());
        builder.RegisterComponent<PlayerMove>(playerMoveLogic);
        builder.RegisterComponent<Status>(this.GetComponent<Status>());

        //‰¼
        builder.RegisterInstance<IReadOnlyPositionAdapter>(readOnlyPositionAdapter);
    }
}
