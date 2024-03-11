using UniRx;
using System;
using VContainer;

public class StartCountDownLogic : ICountDownNotificationable, ICountDownable, IDisposable
{
    private ReactiveProperty<float> timeFloatProperty = new ReactiveProperty<float>();
    private ReactiveProperty<int> timeIntProperty = new ReactiveProperty<int>();
    public IReadOnlyReactiveProperty<float> TimeFloatProperty => timeFloatProperty;
    public IReadOnlyReactiveProperty<int> TimeIntProperty => timeIntProperty;

    public Action<float> TimeAction;
    private IGameProgressManagementAuxiliaryable gameProgressAuziliaryable;

    [Inject]
    public StartCountDownLogic(IGameProgressManagementAuxiliaryable gameProgressAuziliaryable)
    {
        this.gameProgressAuziliaryable = gameProgressAuziliaryable;

    }
    public void CountDownInvoke(float deltaTile)
    {
        TimeAction?.Invoke(deltaTile);
    }

    public void Start()
    {
        timeFloatProperty.Value = 3.99f;
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
        timeFloatProperty.Value -= countDownTime;
        if ((int)timeFloatProperty.Value < timeIntProperty.Value)
        {
            timeIntProperty.Value--;

            //ƒQ[ƒ€ŠJŽn
            if (timeIntProperty.Value <= 0)
            {
                gameProgressAuziliaryable.Start();
                TimeAction -= CountDown;
            }
        }
    }
}
