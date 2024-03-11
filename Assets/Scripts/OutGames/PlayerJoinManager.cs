using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �v���C���[�̓��ގ��̊Ǘ��N���X�i�A�E�g�Q�[���j
/// </summary>
public class PlayerJoinManager : MonoBehaviour
{
    // �v���C���[���Q�[����Join���邽�߂�InputAction
    [SerializeField] private InputAction playerJoinInputAction = default;
    // �v���C���[���Q�[������Left���邽�߂�InputAction
    [SerializeField] private InputAction playerLeftInputAction = default;
    // �A�E�g�Q�[���p�̃v���C���[�i�L�����I�������邽�߂̉��z�̃v���C���[�j
    [SerializeField] private VirtualPlayer virtualPlayer = default;

    // Join�ς݂̃f�o�C�X���
    private InputDevice[] joinedDevices = default;
    private PlayerInput[] joinedPlayers = default;
    // ���݂̃v���C���[��
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

        // �ő�Q���\���Ŕz���������
        joinedDevices = new InputDevice[this.maxPlayerCount];
        joinedPlayers = new PlayerInput[this.maxPlayerCount];

        playerJoinInputAction.performed += OnJoin;
        playerLeftInputAction.performed += OnLeft;
    }


    /// <summary>
    /// �f�o�C�X�ɂ����Join�v�������΂����Ƃ��ɌĂ΂�鏈��
    /// </summary>
    private void OnJoin(InputAction.CallbackContext context)
    {
        // �v���C���[�����ő吔�ɒB���Ă�����A�������I��
        if (currentPlayerCount >= maxPlayerCount)
        {
            return;
        }

        // Join�v�����̃f�o�C�X�����ɎQ���ς݂̂Ƃ��A�������I��
        foreach (var device in joinedDevices)
        {
            if (context.control.device == device)
            {
                return;
            }
        }

        // �z��̋󂫂�T���i�ؒf�����ꍇ�A�z��ɋ󂫂��o�邽�߁j
        while (true)
        {
            if (joinedDevices[currentPlayerCount] is null)
            {
                break;
            }

            currentPlayerCount++;
        }

        // PlayerInput�������������z�̃v���C���[���C���X�^���X��
        // ��Join�v�����̃f�o�C�X����R�Â��ăC���X�^���X�𐶐�����
        var player = PlayerInput.Instantiate(
            prefab: virtualPlayer.gameObject,
            playerIndex: currentPlayerCount,
            pairWithDevice: context.control.device
            );

        // �C���X�^���X�������v���C���[�Ƀf�o�C�X���𑗂�
        player.GetComponent<VirtualPlayer>().InitPlayer(player.playerIndex, context.control.device);

        // Join�����f�o�C�X����ۑ�
        joinedDevices[currentPlayerCount] = context.control.device;
        joinedPlayers[currentPlayerCount] = player;

        currentPlayerCount++;
    }

    /// <summary>
    /// �f�o�C�X�ɂ����Left�v�������΂����Ƃ��ɌĂ΂�鏈��
    /// </summary>
    private void OnLeft(InputAction.CallbackContext context)
    {
        // Join�ς݂̃f�o�C�X����T��
        for (int i = 0; i < joinedDevices.Length; i++)
        {
            // Left�v���̑��M���f�o�C�X�ƈ�v�����ꍇ�ALeft������
            if (context.control.device == joinedDevices[i])
            {
                // �L�����N�^�[�m����Left�v����������A�������I��
                // �v���C���[�̑���Ɠ��͂��ꕔ����Ă��邽�߁A���̏ꍇ�̑΍�
                if (joinedPlayers[i].GetComponent<VirtualPlayer>().IsSubmit)
                {
                    return;
                }

                // �z��̏����N���A���A�v���C���[���폜
                joinedDevices[i] = null;
                Destroy(joinedPlayers[i].gameObject);
                joinedPlayers[i] = null;

                // �폜���Index����
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
