using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの入退室の管理クラス（アウトゲーム）
/// </summary>
public class PlayerJoinManager : MonoBehaviour
{
    // プレイヤーがゲームにJoinするためのInputAction
    [SerializeField] private InputAction playerJoinInputAction = default;
    // プレイヤーがゲームからLeftするためのInputAction
    [SerializeField] private InputAction playerLeftInputAction = default;
    // アウトゲーム用のプレイヤー（キャラ選択をするための仮想のプレイヤー）
    [SerializeField] private VirtualPlayer virtualPlayer = default;

    // Join済みのデバイス情報
    private InputDevice[] joinedDevices = default;
    private PlayerInput[] joinedPlayers = default;
    // 現在のプレイヤー数
    private int currentPlayerCount = 0;

    private int maxPlayerCount = default;


    private void OnEnable()
    {
        playerJoinInputAction.Enable();
        playerLeftInputAction.Enable();
    }

    private void OnDisable()
    {
        playerJoinInputAction.Disable();
        playerLeftInputAction.Disable();
    }


    public void OnAwake(int maxPlayerCount)
    {
        this.maxPlayerCount = maxPlayerCount;

        // 最大参加可能数で配列を初期化
        joinedDevices = new InputDevice[this.maxPlayerCount];
        joinedPlayers = new PlayerInput[this.maxPlayerCount];

        playerJoinInputAction.performed += OnJoin;
        playerLeftInputAction.performed += OnLeft;
    }


    /// <summary>
    /// デバイスによってJoin要求が発火したときに呼ばれる処理
    /// </summary>
    private void OnJoin(InputAction.CallbackContext context)
    {
        // プレイヤー数が最大数に達していたら、処理を終了
        if (currentPlayerCount >= maxPlayerCount)
        {
            return;
        }

        // Join要求元のデバイスが既に参加済みのとき、処理を終了
        foreach (var device in joinedDevices)
        {
            if (context.control.device == device)
            {
                return;
            }
        }

        // 配列の空きを探索（切断した場合、配列に空きが出るため）
        while (true)
        {
            if (joinedDevices[currentPlayerCount] is null)
            {
                break;
            }

            currentPlayerCount++;
        }

        // PlayerInputを所持した仮想のプレイヤーをインスタンス化
        // ※Join要求元のデバイス情報を紐づけてインスタンスを生成する
        var player = PlayerInput.Instantiate(
            prefab: virtualPlayer.gameObject,
            playerIndex: currentPlayerCount,
            pairWithDevice: context.control.device
            );

        // インスタンス化したプレイヤーにデバイス情報を送る
        player.GetComponent<VirtualPlayer>().InitPlayer(player.playerIndex, context.control.device);

        // Joinしたデバイス情報を保存
        joinedDevices[currentPlayerCount] = context.control.device;
        joinedPlayers[currentPlayerCount] = player;

        currentPlayerCount++;
    }

    /// <summary>
    /// デバイスによってLeft要求が発火したときに呼ばれる処理
    /// </summary>
    private void OnLeft(InputAction.CallbackContext context)
    {
        // Join済みのデバイス情報を探索
        for (int i = 0; i < joinedDevices.Length; i++)
        {
            // Left要求の送信元デバイスと一致した場合、Leftを許可
            if (context.control.device == joinedDevices[i])
            {
                // キャラクター確定後のLeft要求だったら、処理を終了
                // プレイヤーの操作と入力が一部被っているため、その場合の対策
                if (joinedPlayers[i].GetComponent<VirtualPlayer>().IsSubmit)
                {
                    return;
                }

                // 配列の情報をクリアし、プレイヤーを削除
                joinedDevices[i] = null;
                Destroy(joinedPlayers[i].gameObject);
                joinedPlayers[i] = null;

                // 削除後のIndex操作
                for (int k = 0; k < joinedDevices.Length; k++)
                {
                    if (joinedDevices[k] is null)
                    {
                        currentPlayerCount = k;
                        break;
                    }
                }

                break;
            }
        }
    }
}
