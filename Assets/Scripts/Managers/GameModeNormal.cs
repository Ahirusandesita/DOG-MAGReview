using VContainer;
using System;
using System.Threading.Tasks;
using System.Linq;
using Cysharp.Threading.Tasks;
/// <summary>
/// ÉQÅ[ÉÄÉÇÅ[Éhnormal
/// </summary>

public delegate UniTask AysncWithRoundHandler(RoundEventArgs roundEventArgs);

public class GameModeNormal : IGameMode, IDisposable
{
    private IGameProgressManagementAuxiliaryable gameProgressManagement;

    public event RoundHandler OnCompletedRound;
    public event AysncWithRoundHandler OnAysncCompletedRound;


    [Inject]
    public GameModeNormal(IGameProgressManagementAuxiliaryable gameProgressManagement)
    {
        this.gameProgressManagement = gameProgressManagement;
        Subscription();
    }

    private void Subscription()
    {
        gameProgressManagement.OnStart += StartHandler;
    }

    private void StartHandler(StartEventArgs startEventArgs)
    {
    }

    public void CompletedRound(TeamInfo teamInfo)
    {
        RoundEventArgs roundEventArgs = new RoundEventArgs();
        roundEventArgs.round = new Round(teamInfo);
        roundEventArgs.teamInfo = teamInfo;
        OnCompletedRound?.Invoke(roundEventArgs);
        OnAsyncEvent(teamInfo);
    }
    public void CompletedRound(TeamInfo teamInfo, Round round)
    {
        RoundEventArgs roundEventArgs = new RoundEventArgs();
        roundEventArgs.round = new Round(teamInfo);
        roundEventArgs.round += round;
        roundEventArgs.teamInfo = teamInfo;

        OnCompletedRound?.Invoke(roundEventArgs);
        OnAsyncEvent(teamInfo);
    }

    private async void OnAsyncEvent(TeamInfo teamInfo)
    {
        RoundEventArgs roundEventArgs = new RoundEventArgs();
        roundEventArgs.teamInfo = teamInfo;

        await UniTask.WhenAll(
             OnAysncCompletedRound?.GetInvocationList()
                .OfType<AysncWithRoundHandler>()
                .Select(async (OnAysncEvent) => await OnAysncEvent.Invoke(roundEventArgs)));

        StartNextRound();
    }

    private void StartNextRound()
    {
        gameProgressManagement.Restart();
    }

    public void Dispose()
    {
        gameProgressManagement = null;
        OnCompletedRound = null;
    }

}
