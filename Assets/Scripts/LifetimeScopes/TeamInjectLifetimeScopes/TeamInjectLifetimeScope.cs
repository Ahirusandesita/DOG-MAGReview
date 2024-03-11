using VContainer;
using VContainer.Unity;

public class TeamInjectLifetimeScope : LifetimeScope, ITeamInformation
{
    private TeamInfo teamInfo;
    private GameModeInfo gameModeInfo;
    private TeamAction teamAction;
    public TeamInfo TeamInfo
    {
        set
        {
            this.teamInfo = value;
        }
        get
        {
            return teamInfo;
        }
    }
    public GameModeInfo GameModeInfo
    {
        set
        {
            this.gameModeInfo = value;
        }
        get
        {
            return gameModeInfo;
        }
    }
    public TeamAction TeamAction
    {
        set
        {
            this.teamAction = value;
        }
        get
        {
            return teamAction;
        }
    }
    protected override void Configure(IContainerBuilder builder)
    {
        InformationConfigure(builder);
    }

    private void InformationConfigure(IContainerBuilder builder)
    {
        builder.RegisterInstance<TeamInfo>(teamInfo);
        builder.RegisterInstance<GameModeInfo>(gameModeInfo);
        builder.RegisterInstance<TeamAction>(teamAction);
    }
}
