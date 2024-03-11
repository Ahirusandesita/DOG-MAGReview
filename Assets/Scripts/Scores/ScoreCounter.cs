using UniRx;
using VContainer;
using System;
using UnityEngine;
/// <summary>
/// チーム毎のスコアカウンター
/// </summary>
public class ScoreCounter : IScoreCountable, IDisposable
{
    private ReactiveProperty<int> scoreProperty = new ReactiveProperty<int>();
    public IReadOnlyReactiveProperty<int> ScoreProperty => scoreProperty;

    private IGameMode gameMode;
    private TeamInfo teamInfo;

    [Inject]
    public ScoreCounter(IGameMode gameMode, TeamInfo teamInfo)
    {
        this.gameMode = gameMode;
        this.teamInfo = teamInfo;

        Subscription();
    }

    private void Subscription()
    {
        gameMode.OnCompletedRound += CompletedRoundHandler;
    }

    private void CompletedRoundHandler(RoundEventArgs roundEventArgs)
    {
        if (roundEventArgs.teamInfo.TeamType != this.teamInfo.TeamType)
        {
            AddScore(roundEventArgs.round.score);
        }
    }

    private void AddScore(int score)
    {
        scoreProperty.Value += score;
    }

    public void Dispose()
    {
        scoreProperty.Dispose();
        gameMode.OnCompletedRound -= CompletedRoundHandler;
        gameMode = null;
        teamInfo = null;
    }
}
