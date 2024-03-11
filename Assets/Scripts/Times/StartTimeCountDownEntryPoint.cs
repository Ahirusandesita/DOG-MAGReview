using UnityEngine;
using VContainer;
using VContainer.Unity;

public class StartTimeCountDownEntryPoint : ITickable, IStartable
{
    private ICountDownable timeLogic;
    [Inject]
    public StartTimeCountDownEntryPoint(ICountDownable timeLogic)
    {
        this.timeLogic = timeLogic;
    }
    public void Start()
    {
        timeLogic.Start();
    }
    public void Tick()
    {
        timeLogic.CountDownInvoke(Time.deltaTime);
    }
}
public class TimeCountDownEntryPoint : ITickable
{
    private ICountDownable timeLogic;
    [Inject]
    public TimeCountDownEntryPoint(IGameProgressRegistrable gameProgressRegistrable, ICountDownable timeLogic)
    {
        this.timeLogic = timeLogic;
        gameProgressRegistrable.OnStart += Start;
    }
    private void Start(StartEventArgs startEventArgs)
    {
        timeLogic.Start();
    }
    public void Tick()
    {
        timeLogic.CountDownInvoke(Time.deltaTime);
    }
}