using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;

/// <summary>
/// キャラクター選択などの入力を扱う仮想のプレイヤークラス（アウトゲーム）
/// </summary>
public class VirtualPlayer : MonoBehaviour
{
    private PlayerInput playerInput = default;
    private PlayerInfo playerInfo = default;

    private readonly Subject<int> changeSelectedCharacterSubject = new();
    private readonly Subject<PlayerInfo> submitCharacterSubject = new();
    private readonly Subject<Unit> cancelSelectedSubject = new();

    // キャラクターを確定しているかどうか
    private bool isSubmit = false;

    public bool IsSubmit => isSubmit;


    /// <summary>
    /// 入力イベント：キャラクター変更
    /// <br>- 引数：左入力（-1）か右入力（1）</br>
    /// </summary>
    public IObservable<int> ChangeSelectedCharacterSubject => changeSelectedCharacterSubject;

    /// <summary>
    /// 入力イベント：キャラクター確定
    /// </summary>
    public IObservable<PlayerInfo> SubmitCharacterSubject => submitCharacterSubject;

    /// <summary>
    /// 入力イベント：選択キャンセル
    /// </summary>
    public IObservable<Unit> CancelSelectedSubject => cancelSelectedSubject;


    private void Awake()
    {
        playerInput = this.GetComponent<PlayerInput>();

        // キャラクター変更にあたる入力イベントが発火したとき
        playerInput.actions["LeftSlide"].performed += OnLeftSlide;
        playerInput.actions["RightSlide"].performed += OnRightSlide;

        // キャラクター確定にあたる入力イベントが発火したとき
        playerInput.actions["Submit"].performed += OnSubmit;

        // キャンセルにあたる入力イベントが発火したとき
        playerInput.actions["Cancel"].performed += OnCancel;
    }

    private void OnDestroy()
    {
        // キャラクター変更にあたる入力イベントが発火したとき
        playerInput.actions["LeftSlide"].performed -= OnLeftSlide;
        playerInput.actions["RightSlide"].performed -= OnRightSlide;

        // キャラクター確定にあたる入力イベントが発火したとき
        playerInput.actions["Submit"].performed -= OnSubmit;

        // キャンセルにあたる入力イベントが発火したとき
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
    /// キャラクター変更（左）が入力されたときに呼ばれる処理
    /// </summary>
    private void OnLeftSlide(InputAction.CallbackContext context)
    {
        // キャラクターが確定されているとき、処理を終了
        if (isSubmit)
        {
            return;
        }

        // イベントを発火
        // 配列操作の都合上、int型を送る（-1が左、1が右）
        changeSelectedCharacterSubject.OnNext(-1);
    }

    /// <summary>
    /// キャラクター変更（右）が入力されたときに呼ばれる処理
    /// </summary>
    private void OnRightSlide(InputAction.CallbackContext context)
    {
        // キャラクターが確定されているとき、処理を終了
        if (isSubmit)
        {
            return;
        }

        // イベントを発火
        // 配列操作の都合上、int型を送る（-1が左、1が右）
        changeSelectedCharacterSubject.OnNext(1);
    }

    /// <summary>
    /// キャラクター確定が入力されたときに呼ばれる処理
    /// </summary>
    private void OnSubmit(InputAction.CallbackContext context)
    {
        // キャラクターが確定されているとき、処理を終了
        if (isSubmit)
        {
            return;
        }

        isSubmit = true;

        // イベントを発火
        submitCharacterSubject.OnNext(playerInfo);
    }

    /// <summary>
    /// キャンセルが入力されたときに呼ばれる処理
    /// </summary>
    private void OnCancel(InputAction.CallbackContext context)
    {
        // キャラクターを確定していないとき、処理を終了
        if (!isSubmit)
        {
            return;
        }

        isSubmit = false;

        // イベントを発火
        cancelSelectedSubject.OnNext(Unit.Default);
    }
}
