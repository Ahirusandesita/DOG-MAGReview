using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using OutGameEnum;

/// <summary>
/// �S�v���C���[���ʂ̑I�����ڂ̊��N���X
/// </summary>
public abstract class OutGameSelectManager<T> : MonoBehaviour where T : IOutGameInfo
{
    [SerializeField] private List<T> selectableUI = default;

    private OutGameInputActions outGameInputActions = default;
    private int selectableMaxIndex = default;


    protected IReadOnlyList<T> SelectableUI => selectableUI;

    protected int SelectedIndex { get; private set; } = 0;


    protected virtual void OnEnable()
    {
        outGameInputActions?.Enable();
    }

    protected virtual void Start()
    {
        selectableMaxIndex = SelectableUI.Count - 1;
    }

    protected virtual void OnDisable()
    {
        outGameInputActions?.Disable();
    }

    protected virtual void OnDestroy()
    {
        outGameInputActions?.Dispose();
    }


    /// <summary>
    /// �O��OutGame���ڂ��I�������Ƃ��ɌĂ΂�鏉��������
    /// <br>- base: Input�C�x���g�̍w��</br>
    /// </summary>
    public virtual void OnAwake()
    {
        outGameInputActions = new OutGameInputActions();
        outGameInputActions.Enable();

        outGameInputActions.OutGame.LeftSlide.performed += OnLeftSlide;
        outGameInputActions.OutGame.RightSlide.performed += OnRightSlide;
        outGameInputActions.OutGame.Submit.performed += OnSubmit;
    }

    /// <summary>
    /// ���������͂����s���ꂽ�Ƃ��ɌĂ΂�鏈��
    /// <br>- base: Index����</br>
    /// </summary>
    /// <param name="context"></param>
    protected virtual void OnLeftSlide(InputAction.CallbackContext context)
    {
        SelectedIndex--;

        if (SelectedIndex < 0)
        {
            SelectedIndex = selectableMaxIndex;
        }
    }

    /// <summary>
    /// �E�������͂����s���ꂽ�Ƃ��ɌĂ΂�鏈��
    /// <br>- base: Index����</br>
    /// </summary>
    /// <param name="context"></param>
    protected virtual void OnRightSlide(InputAction.CallbackContext context)
    {
        SelectedIndex++;

        if (SelectedIndex > selectableMaxIndex)
        {
            SelectedIndex = 0;
        }
    }

    /// <summary>
    /// �m��ɂ�������͂����s���ꂽ�Ƃ��ɌĂ΂�鏈��
    /// <br>- base: Input�C�x���g�̍w�ǂ�j��</br>
    /// </summary>
    /// <param name="context"></param>
    protected virtual void OnSubmit(InputAction.CallbackContext context)
    {
        outGameInputActions.OutGame.Submit.performed -= OnSubmit;
    }
}

[Serializable]
public class TitleInfo : IOutGameInfo
{
    [SerializeField] private Image titleUI = default;

    public Image TitleUI => titleUI;
}

[Serializable]
public class SelectGameRuleInfo : IOutGameInfo
{
    [SerializeField] private Image uI = default;
    [SerializeField] private GameModeType gameModeType = default;

    public Image UI => uI;
    public GameModeType GameModeType => gameModeType;
}

[Serializable]
public class SelectPlayerCountInfo : IOutGameInfo
{
    [SerializeField] private Image uI = default;
    [SerializeField, Min(1)] private int playerCount = default;

    public Image UI => uI;
    public int PlayerCount => playerCount;
}
