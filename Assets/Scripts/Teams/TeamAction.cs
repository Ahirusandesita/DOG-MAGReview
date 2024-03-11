using VContainer;
using UnityEngine;
public class TeamAction
{
    public event TeamActionHandler OnTeamAction;
    private TeamInfo teamInfo;

    [Inject]
    public TeamAction(TeamInfo teamInfo)
    {
        this.teamInfo = teamInfo;
    }

    public void Action(TeamInfo teamInfo,Round round)
    {
        TeamEventArgs teamEventArgs = new TeamEventArgs();
        teamEventArgs.round = round;
        OnTeamAction?.Invoke(teamEventArgs);
    }
}
