using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;

/// <summary>
/// �L�����N�^�[�I���Ȃǂ̓��͂��������z�̃v���C���[�N���X�i�A�E�g�Q�[���j
/// </summary>
public class VirtualPlayer : MonoBehaviour
{
    private PlayerInput playerInput = default;
    private PlayerInfo playerInfo = default;

    private readonly Subject<int> changeSelectedCharacterSubject = new();
    private readonly Subject<PlayerInfo> submitCharacterSubject = new();
    private readonly Subject<Unit> cancelSelectedSubject = new();

    // �L�����N�^�[���m�肵�Ă��邩�ǂ���
    private bool isSubmit = false;

    public bool IsSubmit => isSubmit;


    /// <summary>
    /// ���̓C�x���g�F�L�����N�^�[�ύX
    /// <br>- �����F�����́i-1�j���E���́i1�j</br>
    /// </summary>
    public IObservable<int> ChangeSelectedCharacterSubject => changeSelectedCharacterSubject;

    /// <summary>
    /// ���̓C�x���g�F�L�����N�^�[�m��
    /// </summary>
    public IObservable<PlayerInfo> SubmitCharacterSubject => submitCharacterSubject;

    /// <summary>
    /// ���̓C�x���g�F�I���L�����Z��
    /// </summary>
    public IObservable<Unit> CancelSelectedSubject => cancelSelectedSubject;


    private void Awake()
    {
        playerInput = this.GetComponent<PlayerInput>();

        // �L�����N�^�[�ύX�ɂ�������̓C�x���g�����΂����Ƃ�
        playerInput.actions["LeftSlide"].performed += OnLeftSlide;
        playerInput.actions["RightSlide"].performed += OnRightSlide;

        // �L�����N�^�[�m��ɂ�������̓C�x���g�����΂����Ƃ�
        playerInput.actions["Submit"].performed += OnSubmit;

        // �L�����Z���ɂ�������̓C�x���g�����΂����Ƃ�
        playerInput.actions["Cancel"].performed += OnCancel;
    }

    private void OnDestroy()
    {
        // �L�����N�^�[�ύX�ɂ�������̓C�x���g�����΂����Ƃ�
        playerInput.actions["LeftSlide"].performed -= OnLeftSlide;
        playerInput.actions["RightSlide"].performed -= OnRightSlide;

        // �L�����N�^�[�m��ɂ�������̓C�x���g�����΂����Ƃ�
        playerInput.actions["Submit"].performed -= OnSubmit;

        // �L�����Z���ɂ�������̓C�x���g�����΂����Ƃ�
        playerInput.actions["Cancel"].performed -= OnCancel;

        changeSelectedCharacterSubject.Dispose();
        submitCharacterSubject.Dispose();
        cancelSelectedSubject.Dispose();
    }


    public void InitPlayer(int playerIndex, InputDevice device)
    {
        playerInfo = new(playerIndex, device);
    }

    /// <summary>
    /// �L�����N�^�[�ύX�i���j�����͂��ꂽ�Ƃ��ɌĂ΂�鏈��
    /// </summary>
    private void OnLeftSlide(InputAction.CallbackContext context)
    {
        // �L�����N�^�[���m�肳��Ă���Ƃ��A�������I��
        if (isSubmit)
        {
            return;
        }

        // �C�x���g�𔭉�
        // �z�񑀍�̓s����Aint�^�𑗂�i-1�����A1���E�j
        changeSelectedCharacterSubject.OnNext(-1);
    }

    /// <summary>
    /// �L�����N�^�[�ύX�i�E�j�����͂��ꂽ�Ƃ��ɌĂ΂�鏈��
    /// </summary>
    private void OnRightSlide(InputAction.CallbackContext context)
    {
        // �L�����N�^�[���m�肳��Ă���Ƃ��A�������I��
        if (isSubmit)
        {
            return;
        }

        // �C�x���g�𔭉�
        // �z�񑀍�̓s����Aint�^�𑗂�i-1�����A1���E�j
        changeSelectedCharacterSubject.OnNext(1);
    }

    /// <summary>
    /// �L�����N�^�[�m�肪���͂��ꂽ�Ƃ��ɌĂ΂�鏈��
    /// </summary>
    private void OnSubmit(InputAction.CallbackContext context)
    {
        // �L�����N�^�[���m�肳��Ă���Ƃ��A�������I��
        if (isSubmit)
        {
            return;
        }

        isSubmit = true;

        // �C�x���g�𔭉�
        submitCharacterSubject.OnNext(playerInfo);
    }

    /// <summary>
    /// �L�����Z�������͂��ꂽ�Ƃ��ɌĂ΂�鏈��
    /// </summary>
    private void OnCancel(InputAction.CallbackContext context)
    {
        // �L�����N�^�[���m�肵�Ă��Ȃ��Ƃ��A�������I��
        if (!isSubmit)
        {
            return;
        }

        isSubmit = false;

        // �C�x���g�𔭉�
        cancelSelectedSubject.OnNext(Unit.Default);
    }
}
