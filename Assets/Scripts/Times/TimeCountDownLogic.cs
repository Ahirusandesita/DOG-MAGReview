using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

public class TimeCountDownLogic : ICountDownNotificationable, ICountDownable, IDisposable
{
    private ReactiveProperty<float> timeFloatProperty = new ReactiveProperty<float>();
    private ReactiveProperty<int> timeIntProperty = new ReactiveProperty<int>();
    public IReadOnlyReactiveProperty<float> TimeFloatProperty => timeFloatProperty;
    public IReadOnlyReactiveProperty<int> TimeIntProperty => timeIntProperty;

    public Action<float> TimeAction;
    private IGameProgressManagementAuxiliaryable gameProgressAuziliaryable;

    private bool isPause = false;

    [Inject]
    public TimeCountDownLogic(IGameProgressManagementAuxiliaryable gameProgressAuziliaryable,IGameMode gameMode)
    {
        this.gameProgressAuziliaryable = gameProgressAuziliaryable;

        gameProgressAuziliaryable.OnRestart += (a) => isPause = false;
        gameMode.OnCompletedRound += (a) => isPause = true;
    }
    public void CountDownInvoke(float deltaTile)
    {
        TimeAction?.Invoke(deltaTile);
    }

    public void Start()
    {
        timeFloatProperty.Value = 180.99f;
        timeIntProperty.Value = (int)timeFloatProperty.Value;
        TimeAction += CountDown;
    }
    public void Dispose()
    {
        TimeAction = null;
        timeFloatProperty.Dispose();
    }

    private void CountDown(float countDownTime)
    {
        if (isPause)
        {
            return;
        }

        timeFloatProperty.Value -= countDownTime;
        if ((int)timeFloatProperty.Value < timeIntProperty.Value)
        {
            timeIntProperty.Value--;

            //ƒQ[ƒ€ŠJŽn
            if (timeIntProperty.Value <= 0)
            {
                gameProgressAuziliaryable.Finish();
                TimeAction -= CountDown;
            }
        }
    }
}