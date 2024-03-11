using VContainer;
using VContainer.Unity;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(TeamSystemCreate))]
public class TeamLifetimeScope : LifetimeScope
{
    private TeamInfo teamInfo;
    public TeamInfo TeamInfo
    {
        set
        {
            teamInfo = value;
        }
    }
    private GameModeInfo gameModeInfo;
    public GameModeInfo GameModeInfo
    {
        set
        {
            gameModeInfo = value;
        }
    }
    private ITeamObjectManager[] teamObjectManagers;
    public ITeamObjectManager[] TeamObjectManagers
    {
        set
        {
            teamObjectManagers = value;
        }
    }
    private IPlayerInfo[] playerInfos;
    public IPlayerInfo[] PlayerInfos
    {
        set
        {
            playerInfos = value;
        }
    }
    private IPositionOnStage positionOnStage;
    public IPositionOnStage PositionOnStage
    {
        set
        {
            positionOnStage = value;
        }
    }

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance<TeamInfo>(teamInfo);
        builder.RegisterInstance<GameModeInfo>(gameModeInfo);
        builder.RegisterComponent<TeamSystemCreate>(this.GetComponent<TeamSystemCreate>());
        builder.Register<TeamAction>(Lifetime.Singleton);

        foreach(ITeamObjectManager teamObjectManager in teamObjectManagers)
        {
            builder.RegisterInstance<ITeamObjectManager>(teamObjectManager);
        }

        builder.RegisterInstance<IPlayerInfo[]>(playerInfos);
        builder.RegisterInstance<IPositionOnStage>(positionOnStage);
        builder.RegisterComponent<PlayerDeviceManager>(this.GetComponent<PlayerDeviceManager>());
    }
}
