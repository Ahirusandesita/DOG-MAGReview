using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class TeamSystemCreate : MonoBehaviour
{ 
    private TeamInfo teamInfo;
    private GameModeInfo gameModeInfo;
    private TeamAction teamAction;

    private ITeamObjectManager[] teamObjectManagers;

    [Inject]
    public void Inject(TeamInfo teamInfo, GameModeInfo gameModeInfo, TeamAction teamAction, IEnumerable<ITeamObjectManager> teamObjectManagers)
    {
        this.teamInfo = teamInfo;
        this.gameModeInfo = gameModeInfo;
        this.teamAction = teamAction;
        this.teamObjectManagers = (ITeamObjectManager[])teamObjectManagers;
        Build();
    }

    private void Build()
    {
        switch (gameModeInfo.NumberOfTeam)
        {
            case 2:
                foreach(ITeamObjectManager teamObjectManager in teamObjectManagers)
                {
                    teamObjectManager.TeamInformations[(int)teamInfo.TeamType].TeamInfo = teamInfo;
                    teamObjectManager.TeamInformations[(int)teamInfo.TeamType].GameModeInfo = gameModeInfo;
                    teamObjectManager.TeamInformations[(int)teamInfo.TeamType].TeamAction = teamAction;
                    teamObjectManager.TeamInformations[(int)teamInfo.TeamType].Build();
                }
                break;
        }

    }
}
