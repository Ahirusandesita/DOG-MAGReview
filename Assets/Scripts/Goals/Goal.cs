using UnityEngine;
using VContainer;
using System.Collections.Generic;

public class Goal : MonoBehaviour
{
    private IGameMode gameMode;
    private TeamInfo teamInfo;
    private TeamAction teamAction;

    [Inject]
    public void Inject(IGameMode gameMode, TeamInfo teamInfo,TeamAction teamAction)
    {
        this.gameMode = gameMode;
        this.teamInfo = teamInfo;
        this.teamAction = teamAction;
    }

    public void GoalIn(TeamInfo teamInfo)
    {
        Round round = new Round(teamInfo);
        gameMode.CompletedRound(this.teamInfo);
        teamAction.Action(teamInfo,round);
    }

    public void GoalIn(TeamInfo teamInfo,int plusScore)
    {
        if (teamInfo.TeamType == this.teamInfo.TeamType)
        {
            return;
        }

        gameMode.CompletedRound(teamInfo,new Round(plusScore,teamInfo.TeamType));
    }
}
