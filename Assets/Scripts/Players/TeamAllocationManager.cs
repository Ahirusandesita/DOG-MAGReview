using System.Collections.Generic;
using UnityEngine;
using VContainer;
using OutGameEnum;

/// <summary>
/// チームの割りあて
/// </summary>
public class TeamAllocationManager : MonoBehaviour
{
    [SerializeField]
    private TeamLifetimeScope teamLifetimeScope;

    private StageManager stageManager;
    private IPlayerInfo[] playerInfos;
    private GameModeInfo gameModeInfo;

    private List<TeamType> teamTypes = new List<TeamType>();

    [Inject]
    public void Inject(IPlayerInfo[] playerInfos,GameModeInfo gameModeInfo,StageManager stageManager)
    {
        this.playerInfos = playerInfos;
        this.gameModeInfo = gameModeInfo;
        this.stageManager = stageManager;

        //ここ実行するとチーム割り当て
        TeamAllocation();
    }

    /// <summary>
    /// チームを割り当てる
    /// </summary>
    public void TeamAllocation()
    {
        bool canTeamSelect = true;
        List<IPlayerInfo> playerInforByTeams = new List<IPlayerInfo>();

        foreach (IPlayerInfo playerInfo in playerInfos)
        {
            foreach (TeamType teamType in teamTypes)
            {
                if (playerInfo.TeamInfo.TeamType == teamType)
                {
                    canTeamSelect = false;
                    break;
                }
            }


            if (canTeamSelect)
            {
                for(int i = 0; i < playerInfos.Length; i++)
                {
                    if(playerInfo.TeamInfo.TeamType == playerInfos[i].TeamInfo.TeamType)
                    {
                        playerInforByTeams.Add(this.playerInfos[i]);
                    }
                }

                TeamLifetimeScope teamLifetimeScope = Instantiate(this.teamLifetimeScope);
                teamLifetimeScope.TeamInfo = playerInfo.TeamInfo;
                teamLifetimeScope.GameModeInfo = gameModeInfo;
                teamLifetimeScope.TeamObjectManagers = stageManager.TeamObjectManagers;

                teamLifetimeScope.PlayerInfos = playerInforByTeams.ToArray();
                teamLifetimeScope.PositionOnStage = stageManager;
                teamLifetimeScope.Build();
                teamTypes.Add(playerInfo.TeamInfo.TeamType);
                playerInforByTeams.Clear();
            }
        }
    }
}
