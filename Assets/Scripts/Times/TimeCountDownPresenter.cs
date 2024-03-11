using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

public class TimeCountDownPresenter : MonoBehaviour
{
    private ICountDownNotificationable timeLogic;
    private NumberDigitManager numberDigitManager;
    [Inject]
    public void Inject(ICountDownNotificationable timeLogic, NumberDigitManager numberDigitManager)
    {
        this.timeLogic = timeLogic;
        this.numberDigitManager = numberDigitManager;

        Subscription();
    }
    private void Subscription()
    {
        timeLogic.TimeIntProperty.Skip(1).Subscribe(time => numberDigitManager.Display(time));
        timeLogic.TimeIntProperty.Skip(1).Where(time => time == 0).Subscribe(time =>
        {
            Destroy(this.gameObject);
        });
    }
}