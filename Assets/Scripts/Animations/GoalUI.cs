using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class GoalUI : MonoBehaviour
{
    private IGameMode gameMode;
    private TeamInfo teamInfo;

    [SerializeField]
    private GameObject goalUIPrefab;
    private GameObject goalUI;

    [Inject]
    public void Inject(IGameMode gameMode,TeamInfo teamInfo)
    {
        this.gameMode = gameMode;
        this.teamInfo = teamInfo;

        Subscription();
    }

    private void Subscription()
    {
        goalUI = Instantiate(goalUIPrefab);
        goalUI.SetActive(false);
        gameMode.OnAysncCompletedRound += UIDisplay;
    }

    private async UniTask UIDisplay(RoundEventArgs roundEventArgs)
    {
        if (teamInfo.TeamType != roundEventArgs.teamInfo.TeamType)
        {
            goalUI.SetActive(true);
            await UniTask.Delay(3000);
            goalUI.SetActive(false);
        }
    }
}

