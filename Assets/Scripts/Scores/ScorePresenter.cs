using UnityEngine;
using UniRx;
using VContainer;

public class ScorePresenter : MonoBehaviour
{
    private IScoreCountable scoreCounter;
    private ScoreView scoreView;

    [Inject]
    public void Inject(IScoreCountable scoreCounter,ScoreView scoreView)
    {
        this.scoreCounter = scoreCounter;
        this.scoreView = scoreView;

        Subscription();
    }

    private void Subscription()
    {
        scoreCounter.ScoreProperty.Subscribe(score => scoreView.Display(score));
    }
}
