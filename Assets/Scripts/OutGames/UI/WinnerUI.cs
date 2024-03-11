using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WinnerUI : TweeningUIBase
{
    private Transform myTransform = default;
    private Image image = default;
    private Vector3 initScale = default;


    private void Awake()
    {
        myTransform = this.transform;
        image = this.GetComponent<Image>();

        initScale = myTransform.localScale;
        image.enabled = false;
    }


    public override void OnStart()
    {
        myTransform.localScale = Vector3.zero;
        image.enabled = true;

        myTransform.DOScale(initScale, 0.8f).SetEase(Ease.OutBounce);
    }

    public override void PlayLoop()
    {

    }
    public override void OnErase()
    {

    }
}
