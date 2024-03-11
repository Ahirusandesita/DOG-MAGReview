using UnityEngine;
using VContainer;
using UniRx;
public class TimePresenter : MonoBehaviour
{
    private ICountDownNotificationable timeLogic;
    private TimeView timeView;
    [Inject]
    public void Inject(ICountDownNotificationable timeLogic, TimeView timeView)
    {
        this.timeLogic = timeLogic;
        this.timeView = timeView;

        Subscription();
    }
    private void Subscription()
    {
        timeLogic.TimeIntProperty.Skip(1).Where(time => time != 0).Subscribe(time => timeView.Display(time.ToString()));
        timeLogic.TimeIntProperty.Skip(1).Where(time => time == 0).Subscribe(time =>
        {
            timeView.Close();
            Destroy(this.gameObject);
            });
    }
}
