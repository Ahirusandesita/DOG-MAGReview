using UnityEngine;
using UnityEngine.InputSystem;
using OutGameEnum;

public class DebugCreatePlayer : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private CharacterAsset defaultCharacter = default;
    private InputAction playerJoinInputAction = default;
    private DeviceLifetimeScope deviceLifetimeScope = default;

    private void Awake()
    {
        playerJoinInputAction = new InputAction(binding: "<Gamepad>/buttonEast");
        playerJoinInputAction.AddBinding("<Keyboard>/space");
        playerJoinInputAction.Enable();
        playerJoinInputAction.performed += OnJoin;
    }

    private void Start()
    {
        deviceLifetimeScope = FindObjectOfType<DeviceLifetimeScope>();
        if (deviceLifetimeScope.PlayerInfos is not null)
        {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        playerJoinInputAction.Dispose();
    }


    private void OnJoin(InputAction.CallbackContext context)
    {
        var playerInfo = new PlayerInfo(0, context.control.device, defaultCharacter);
        playerInfo.SetTeamInfo(new TeamInfo(TeamType.Red, ScriptableObject.CreateInstance<TeamColorAsset>()));

        var playerInfo2 = new PlayerInfo(1,null, defaultCharacter);
        playerInfo2.SetTeamInfo(new TeamInfo(TeamType.Blue, ScriptableObject.CreateInstance<TeamColorAsset>()));

        deviceLifetimeScope.SetInformation(new IPlayerInfo[] { playerInfo, playerInfo2 });

        new DeviceManager(new InputDevice[] { context.control.device });
        Destroy(this);
    }
#endif
}
