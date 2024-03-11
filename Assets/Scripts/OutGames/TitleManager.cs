using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem;

public class TitleManager : OutGameSelectManager<TitleInfo>
{
    [SerializeField] private Image flashableUI = default;

    private CancellationToken token = default;
    private ITween tween = default;
    private readonly Subject<Unit> submitTitleSubject = new();


    public IObservable<Unit> SubmitTitleSubject => submitTitleSubject;


    private void Awake()
    {
        OnAwake();

        token = this.GetCancellationTokenOnDestroy();
    }


    public override void OnAwake()
    {
        base.OnAwake();

        SelectableUI[0].TitleUI.enabled = true;
        SelectableUI[1].TitleUI.enabled = false;
        flashableUI.enabled = true;

        // UIの点滅処理
        tween = new Tween().Flash(flashableUI, 0.75f, 0.1f, 1f);
    }


    protected override async void OnSubmit(InputAction.CallbackContext context)
    {
        base.OnSubmit(context);

        // 点滅UIを非表示
        tween.Stop();
        tween.Dispose();
        flashableUI.enabled = false;

        await SubmitTitleAsync();

        submitTitleSubject.OnNext(Unit.Default);
        submitTitleSubject.OnCompleted();
    }


    /// <summary>
    /// タイトルのアニメーション
    /// </summary>
    private async UniTask SubmitTitleAsync()
    {
        const int LOOP_TIMES = 200;

        SelectableUI[0].TitleUI.enabled = false;
        SelectableUI[1].TitleUI.enabled = true;

        await UniTask.Delay(TimeSpan.FromSeconds(1.5), cancellationToken: token);

        for (int i = 0; i < LOOP_TIMES; i++)
        {
            float alfa = SelectableUI[1].TitleUI.color.a;
            alfa -= 1f / LOOP_TIMES;
            SelectableUI[1].TitleUI.color = new Color(1f, 1f, 1f, alfa);

            await UniTask.Yield(token);
        }

        await UniTask.Delay(TimeSpan.FromSeconds(0.5), cancellationToken: token);
    }
}
