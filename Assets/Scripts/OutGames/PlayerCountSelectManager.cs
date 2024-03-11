using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.InputSystem;

public class PlayerCountSelectManager : OutGameSelectManager<SelectPlayerCountInfo>
{
    [SerializeField] private Image otherImage = default;

    // Debug
    private Image image = default;

    private readonly Subject<int> submitPlayerCountSubject = new Subject<int>();
    public IObservable<int> SubmitPlayerCountSubject => submitPlayerCountSubject;


    private void Awake()
    {
        for (int i = 0; i < SelectableUI.Count; i++)
        {
            SelectableUI[i].UI.enabled = false;
        }
        otherImage.enabled = false;
    }


    public override void OnAwake()
    {
        base.OnAwake();

        for (int i = 0; i < SelectableUI.Count; i++)
        {
            SelectableUI[i].UI.enabled = true;
        }
        otherImage.enabled = true;

        image = SelectableUI[0].UI;
        image.color = Color.red;
    }

    protected override void OnLeftSlide(InputAction.CallbackContext context)
    {
        image.color = Color.white;

        base.OnLeftSlide(context);

        image = SelectableUI[SelectedIndex].UI;
        image.color = Color.red;
    }

    protected override void OnRightSlide(InputAction.CallbackContext context)
    {
        image.color = Color.white;

        base.OnRightSlide(context);

        image = SelectableUI[SelectedIndex].UI;
        image.color = Color.red;
    }

    protected override void OnSubmit(InputAction.CallbackContext context)
    {
        base.OnSubmit(context);

        submitPlayerCountSubject.OnNext(SelectableUI[SelectedIndex].PlayerCount);
        submitPlayerCountSubject.OnCompleted();
    }
}
