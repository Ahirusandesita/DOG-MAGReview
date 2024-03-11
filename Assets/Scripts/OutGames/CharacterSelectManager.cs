using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UniRx;
using OutGameEnum;

/// <summary>
/// キャラクター選択システムの管理クラス
/// <br>- 入力とビューを仲介</br>
/// <br>- キャラクターのデータを所持</br>
/// </summary>
public class CharacterSelectManager : MonoBehaviour
{
    [SerializeField] private Image headerUI = default;
    [SerializeField] private Image[] explanationUI = default;
    [SerializeField] private Image flashedUI = default;
    // 選択可能なキャラクターのデータをリストで保持
    [SerializeField] private List<CharacterAsset> selectableCharacters = default;
    // TeamColorリスト
    [SerializeField] private List<TeamColorAsset> teamColorAssets = default;
    // ビューの表示に使うCanvas
    [SerializeField] private Canvas outGameCanvas = default;
    // プレイヤーに紐づいたビューの管理クラス
    [SerializeField] private CharacterSelectView characterSelectView = default;
    [SerializeField] private RegisterSceneInInspector sceneData = default;

    private PlayerInputManager playerInputManager = default;
    private ITween tween = default;
    private int currentPlayerCount = 0;

    private PlayerInfo[] playerInfos = default;
    private CharacterSelectView[] joinedPlayerView = default;

    // キャラクターを確定したプレイヤーの数
    private ReactiveProperty<int> submitPlayerCount = new();


    private void Awake()
    {
        headerUI.enabled = false;
        flashedUI.enabled = false;

        // プレイヤーがJoinしたとき（PlayerInputがインスタンス化されたとき）に発火するイベントを購読
        playerInputManager = this.GetComponent<PlayerInputManager>();
    }


    public void OnAwake(int maxPlayerCount)
    {
        headerUI.enabled = true;
        flashedUI.enabled = true;

        // UIを点滅させる
        tween = new Tween().Flash(flashedUI, 0.75f, 0.1f, 0.8f);

        playerInputManager.onPlayerJoined += OnPlayerJoined;
        playerInputManager.onPlayerLeft += OnPlayerLeft;

        // プレイヤー全員がキャラクターを確定したとき、イベントを発火させる
        submitPlayerCount
            .Where(value => value == maxPlayerCount)
            .Subscribe(value => StartGame());

        playerInfos = new PlayerInfo[maxPlayerCount];
        joinedPlayerView = new CharacterSelectView[maxPlayerCount];
    }

    /// <summary>
    /// プレイヤーがゲームにJoinしたときに呼ばれる処理
    /// </summary>
    /// <param name="playerInput"></param>
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        // UI切り替え（最初の一回だけ）
        if (currentPlayerCount == 0)
        {
            foreach (var item in explanationUI)
            {
                item.enabled = true;
            }
            tween.Stop();
            flashedUI.enabled = false;
        }

        VirtualPlayer player = playerInput.GetComponent<VirtualPlayer>();

        // 各プレイヤーに紐づいた、ビューの操作クラス（Empty）をインスタンス化
        CharacterSelectView view = Instantiate(characterSelectView, outGameCanvas.transform);
        joinedPlayerView[playerInput.playerIndex] = view;

        // ビューを生成する
        // ビューにのみ必要な情報を渡したいので、OutGameData型のリストを新たに生成して渡す
        view.CreateView(CreateViewData(), playerInput.playerIndex, teamColorAssets.ToArray());

        // ビューの操作処理にプレイヤーの入力イベントを購読させる
        player.ChangeSelectedCharacterSubject.Subscribe(value => view.ChangeCharacterSelected(value)).AddTo(this);

        // キャラクター確定時のプレイヤーの入力イベントを購読
        player.SubmitCharacterSubject.Subscribe(playerInfo => OnSubmit(playerInfo, view)).AddTo(this);
        // キャラクター選択解除時のプレイヤーの入力イベントを購読
        player.CancelSelectedSubject.Subscribe(_ => OnCancel(view)).AddTo(this);

        currentPlayerCount++;
    }

    /// <summary>
    /// プレイヤーがゲームからLeftしたときに呼ばれる処理
    /// </summary>
    /// <param name="playerInput"></param>
    private void OnPlayerLeft(PlayerInput playerInput)
    {
        // ビューを削除
        Destroy(joinedPlayerView[playerInput.playerIndex].gameObject);
        joinedPlayerView[playerInput.playerIndex] = null;

        currentPlayerCount--;

        // プレイヤー数が0になったら、UIを切り替える
        if (currentPlayerCount == 0)
        {
            foreach (var item in explanationUI)
            {
                item.enabled = false;
            }
            flashedUI.enabled = true;
            tween.Restart();
        }
    }

    /// <summary>
    /// ビューの生成に必要なデータを格納した配列を作成する
    /// </summary>
    /// <returns>生成したCharacterScriptableObject.OutGameData型の配列</returns>
    private OutGameData[] CreateViewData()
    {
        // 配列の宣言（要素数は既存のデータリスト（CharacterScriptableObject型）と同じ）
        var viewData = new OutGameData[selectableCharacters.Count];

        // 既存のデータリストのうち、必要なデータをコピー
        for (int i = 0; i < selectableCharacters.Count; i++)
        {
            viewData[i] = selectableCharacters[i].CharacterUI;
        }

        return viewData;
    }

    private void OnSubmit(PlayerInfo playerInfo, CharacterSelectView view)
    {
        submitPlayerCount.Value++;

        var selectedCharacter = selectableCharacters[view.SubmitCharacter()];
        playerInfo.SetCharacter(selectedCharacter);

        // 現在は2人プレイのため、ここでチーム情報を生成
        // Debug
        int teamIndex = playerInfo.PlayerIndex;

        if (teamIndex > 1)
        {
            teamIndex -= 2;
        }
        playerInfo.SetTeamInfo(new TeamInfo((TeamType)teamIndex, teamColorAssets[teamIndex]));
        playerInfos[playerInfo.PlayerIndex] = playerInfo;
    }

    private void OnCancel(CharacterSelectView view)
    {
        view.CanceledCharacter();
        submitPlayerCount.Value--;
    }

    /// <summary>
    /// アウトゲームを終了し、インゲームに移行する
    /// </summary>
    private async void StartGame()
    {
        // プレイヤー切断イベントの購読を停止
        playerInputManager.onPlayerLeft -= OnPlayerLeft;

        // 不要な参照をクリア
        submitPlayerCount.Dispose();
        tween.Dispose();

        // シーン遷移し、遷移先のコンポーネントを取得する
        var target = await SceneLoader.Load<DeviceLifetimeScope>(sceneData);
        target.SetInformation(playerInfos);

        // 確定したデバイス情報を配列に格納し、DeviceManagerを生成
        InputDevice[] playDevices = new InputDevice[playerInfos.Length];

        for (int i = 0; i < playDevices.Length; i++)
        {
            playDevices[i] = playerInfos[i].Device;
        }

        new DeviceManager(playDevices);
    }
}