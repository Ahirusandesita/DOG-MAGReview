using VContainer;
using VContainer.Unity;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(TeamAllocationManager))]
public class DeviceLifetimeScope : LifetimeScope
{
    [SerializeField]
    private List<StageManager> stageManagers = new List<StageManager>();

    private IPlayerInfo[] playerInfos;
    public IPlayerInfo[] PlayerInfos
    {
        set
        {
            playerInfos = value;
        }
        get
        {
            return playerInfos;
        }
    }
    private GameModeInfo gameModeInfo;

    public void SetInformation(IPlayerInfo[] playerInfos,GameModeInfo gameModeInfo)
    {
        this.playerInfos = playerInfos;
        this.gameModeInfo = gameModeInfo;

        Build();
    }
    public void SetInformation(IPlayerInfo[] playerInfos)
    {
        this.playerInfos = playerInfos;
        Build();
    }
    public GameModeInfo GameModeInfo
    {
        set
        {
            gameModeInfo = value;
        }
    }

    private bool canBuild = true;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance<IPlayerInfo[]>(playerInfos);
        //builder.RegisterComponent<PlayerDeviceManager>(this.GetComponent<PlayerDeviceManager>());
        builder.RegisterComponent<TeamAllocationManager>(this.GetComponent<TeamAllocationManager>());
        builder.RegisterInstance<GameModeInfo>(gameModeInfo);

        switch (gameModeInfo.NumberOfTeam)
        {
            case 2:
                builder.RegisterInstance<IPositionOnStage,StageManager>(Instantiate(stageManagers[0]));
                break;
        }
    }
    private void Start()
    {
        if (PlayerInfos is not null)
        {

        }
        else
        {
            Debug.LogError("PlayerData[]Ç™NullÇ≈Ç∑ÅB");
        }
    }
    public new void Build()
    {
        if (canBuild)
        {
            if(gameModeInfo is null)
            {
                gameModeInfo = new GameModeInfo(2);
            }

            base.Build();
            canBuild = false;
        }
    }
}
