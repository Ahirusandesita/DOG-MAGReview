using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

public class PlayerDeviceManager : MonoBehaviour
{
    [SerializeField] private PlayerInput inGamePlayer = default;

    private IPlayerInfo[] playerDatas;
    private IPositionOnStage stageManager;

    [Inject]
    public void Inject(IPlayerInfo[] playerDatas,IPositionOnStage stageManager)
    {
        this.playerDatas = playerDatas;
        this.stageManager = stageManager;

        SortingGeneration();
    }

    private void SortingGeneration()
    {
        foreach (PlayerInfo playerData in playerDatas)
        {
            InputSystemUseObject inputSystemUseObject = new InputSystemUseNullObject();

            if (playerData.Device is not null)
            {
                PlayerInput playerInput = PlayerInput.Instantiate(
                    prefab: inGamePlayer.gameObject,
                    playerIndex: playerData.PlayerIndex,
                    pairWithDevice: playerData.Device
                    );
                inputSystemUseObject = new InputSystemUseObject(playerInput);
            }

            //PlayerDataÇÇ‡Ç∆Ç…characterAssetÇ©ÇÁPrefabê∂ê¨
            GameObject player = Instantiate(playerData.CharacterData.CharacterPrefab);
            PlayerLifetimeScope playerLifetimeScope = player.GetComponent<PlayerLifetimeScope>();
            playerLifetimeScope.InGameParamater = playerData.CharacterData.Paramater;
            playerLifetimeScope.TeamInfo = playerData.TeamInfo;
            playerLifetimeScope.InputSystemUseObject = inputSystemUseObject;
            playerLifetimeScope.ReadOnlyPositionAdapter = stageManager[playerData.TeamInfo.TeamType];
            playerLifetimeScope.Build();
        }
    }
}
