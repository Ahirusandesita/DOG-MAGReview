using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoseUI : TweeningUIBase
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

        myTransform.DOScale(initScale, 0.5f).SetEase(Ease.Linear).OnComplete(Fall);

        void Fall()
        {
            myTransform.DOLocalRotate(new Vector3(0f, 0f, -15f), 1.2f).SetEase(Ease.OutElastic);
            myTransform.DOMoveY(myTransform.position.y - 30f, 1.2f).SetEase(Ease.OutQuart);
        }
    }

    public override void PlayLoop()
    {
        
    }
}
