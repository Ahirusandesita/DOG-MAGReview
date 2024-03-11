using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoseOtherUI : TweeningUIBase
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


    public override void OnErase()
    {

    }

    public override void OnStart()
    {
        myTransform.localScale = Vector3.zero;
        image.enabled = true;

        myTransform.DOScale(initScale, 0.5f).SetEase(Ease.Linear);
    }

    public override void PlayLoop()
    {

    }
}
