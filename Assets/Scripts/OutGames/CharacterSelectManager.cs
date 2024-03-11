using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UniRx;
using OutGameEnum;

/// <summary>
/// �L�����N�^�[�I���V�X�e���̊Ǘ��N���X
/// <br>- ���͂ƃr���[�𒇉�</br>
/// <br>- �L�����N�^�[�̃f�[�^������</br>
/// </summary>
public class CharacterSelectManager : MonoBehaviour
{
    [SerializeField] private Image headerUI = default;
    [SerializeField] private Image[] explanationUI = default;
    [SerializeField] private Image flashedUI = default;
    // �I���\�ȃL�����N�^�[�̃f�[�^�����X�g�ŕێ�
    [SerializeField] private List<CharacterAsset> selectableCharacters = default;
    // TeamColor���X�g
    [SerializeField] private List<TeamColorAsset> teamColorAssets = default;
    // �r���[�̕\���Ɏg��Canvas
    [SerializeField] private Canvas outGameCanvas = default;
    // �v���C���[�ɕR�Â����r���[�̊Ǘ��N���X
    [SerializeField] private CharacterSelectView characterSelectView = default;
    [SerializeField] private RegisterSceneInInspector sceneData = default;

    private PlayerInputManager playerInputManager = default;
    private ITween tween = default;
    private int currentPlayerCount = 0;

    private PlayerInfo[] playerInfos = default;
    private CharacterSelectView[] joinedPlayerView = default;

    // �L�����N�^�[���m�肵���v���C���[�̐�
    private ReactiveProperty<int> submitPlayerCount = new();


    private void Awake()
    {
        headerUI.enabled = false;
        flashedUI.enabled = false;

        // �v���C���[��Join�����Ƃ��iPlayerInput���C���X�^���X�����ꂽ�Ƃ��j�ɔ��΂���C�x���g���w��
        playerInputManager = this.GetComponent<PlayerInputManager>();
    }


    public void OnAwake(int maxPlayerCount)
    {
        headerUI.enabled = true;
        flashedUI.enabled = true;

        // UI��_�ł�����
        tween = new Tween().Flash(flashedUI, 0.75f, 0.1f, 0.8f);

        playerInputManager.onPlayerJoined += OnPlayerJoined;
        playerInputManager.onPlayerLeft += OnPlayerLeft;

        // �v���C���[�S�����L�����N�^�[���m�肵���Ƃ��A�C�x���g�𔭉΂�����
        submitPlayerCount
            .Where(value => value == maxPlayerCount)
            .Subscribe(value => StartGame());

        playerInfos = new PlayerInfo[maxPlayerCount];
        joinedPlayerView = new CharacterSelectView[maxPlayerCount];
    }

    /// <summary>
    /// �v���C���[���Q�[����Join�����Ƃ��ɌĂ΂�鏈��
    /// </summary>
    /// <param name="playerInput"></param>
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        // UI�؂�ւ��i�ŏ��̈�񂾂��j
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

        // �e�v���C���[�ɕR�Â����A�r���[�̑���N���X�iEmpty�j���C���X�^���X��
        CharacterSelectView view = Instantiate(characterSelectView, outGameCanvas.transform);
        joinedPlayerView[playerInput.playerIndex] = view;

        // �r���[�𐶐�����
        // �r���[�ɂ̂ݕK�v�ȏ���n�������̂ŁAOutGameData�^�̃��X�g��V���ɐ������ēn��
        view.CreateView(CreateViewData(), playerInput.playerIndex, teamColorAssets.ToArray());

        // �r���[�̑��쏈���Ƀv���C���[�̓��̓C�x���g���w�ǂ�����
        player.ChangeSelectedCharacterSubject.Subscribe(value => view.ChangeCharacterSelected(value)).AddTo(this);

        // �L�����N�^�[�m�莞�̃v���C���[�̓��̓C�x���g���w��
        player.SubmitCharacterSubject.Subscribe(playerInfo => OnSubmit(playerInfo, view)).AddTo(this);
        // �L�����N�^�[�I���������̃v���C���[�̓��̓C�x���g���w��
        player.CancelSelectedSubject.Subscribe(_ => OnCancel(view)).AddTo(this);

        currentPlayerCount++;
    }

    /// <summary>
    /// �v���C���[���Q�[������Left�����Ƃ��ɌĂ΂�鏈��
    /// </summary>
    /// <param name="playerInput"></param>
    private void OnPlayerLeft(PlayerInput playerInput)
    {
        // �r���[���폜
        Destroy(joinedPlayerView[playerInput.playerIndex].gameObject);
        joinedPlayerView[playerInput.playerIndex] = null;

        currentPlayerCount--;

        // �v���C���[����0�ɂȂ�����AUI��؂�ւ���
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
    /// �r���[�̐����ɕK�v�ȃf�[�^���i�[�����z����쐬����
    /// </summary>
    /// <returns>��������CharacterScriptableObject.OutGameData�^�̔z��</returns>
    private OutGameData[] CreateViewData()
    {
        // �z��̐錾�i�v�f���͊����̃f�[�^���X�g�iCharacterScriptableObject�^�j�Ɠ����j
        var viewData = new OutGameData[selectableCharacters.Count];

        // �����̃f�[�^���X�g�̂����A�K�v�ȃf�[�^���R�s�[
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

        // ���݂�2�l�v���C�̂��߁A�����Ń`�[�����𐶐�
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
    /// �A�E�g�Q�[�����I�����A�C���Q�[���Ɉڍs����
    /// </summary>
    private async void StartGame()
    {
        // �v���C���[�ؒf�C�x���g�̍w�ǂ��~
        playerInputManager.onPlayerLeft -= OnPlayerLeft;

        // �s�v�ȎQ�Ƃ��N���A
        submitPlayerCount.Dispose();
        tween.Dispose();

        // �V�[���J�ڂ��A�J�ڐ�̃R���|�[�l���g���擾����
        var target = await SceneLoader.Load<DeviceLifetimeScope>(sceneData);
        target.SetInformation(playerInfos);

        // �m�肵���f�o�C�X����z��Ɋi�[���ADeviceManager�𐶐�
        InputDevice[] playDevices = new InputDevice[playerInfos.Length];

        for (int i = 0; i < playDevices.Length; i++)
        {
            playDevices[i] = playerInfos[i].Device;
        }

        new DeviceManager(playDevices);
    }
}