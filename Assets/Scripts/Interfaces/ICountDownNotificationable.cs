using UniRx;


public interface ICountDownNotificationable
{
    IReadOnlyReactiveProperty<int> TimeIntProperty { get; }
    IReadOnlyReactiveProperty<float> TimeFloatProperty { get; }
}
public interface ICountDownable
{
    public void CountDownInvoke(float deltaTime);
    void Start();
}