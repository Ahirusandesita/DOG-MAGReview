using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using VContainer;
using OutGameEnum;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private Canvas inGameCanvas = default;
    [SerializeField] private Canvas resultCanvas = default;
    [SerializeField] private GameObject postProcesser = default;
    [SerializeField] private TweeningUIBase[] animatableUI = default;
    [SerializeField] private ResultPoint[] resultPoints = new ResultPoint[2];

    private Transform canvasTransform = default;
    private Camera mainCamera = default;
    private CancellationToken token = default;
    private Dictionary<TeamType, OutGameData[]> playCharacterData = default;

    public ResultData ResultData { get; set; }

    // Debug
    public CharacterAsset redCharacter1;
    public CharacterAsset redCharacter2;


    private void Awake()
    {
        token = this.GetCancellationTokenOnDestroy();
        mainCamera = Camera.main;
        canvasTransform = resultCanvas.transform;

        resultCanvas.gameObject.SetActive(false);

        // Debug
        playCharacterData = new Dictionary<TeamType, OutGameData[]>();
        playCharacterData.Add(TeamType.Red, new OutGameData[] { redCharacter1.CharacterUI, redCharacter2.CharacterUI });
    }


    [Inject]
    public void Inject(Dictionary<TeamType, OutGameData[]> playCharacterData)
    {
        this.playCharacterData = playCharacterData;
    }

    public void DisplayResult()
    {
        try
        {
            ResultAsync().Forget();
        }
        catch (OperationCanceledException) { }
    }

    private async UniTaskVoid ResultAsync()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(2.5), cancellationToken: token);

        SpriteRenderer[] spriteRenderers = FindObjectsOfType<SpriteRenderer>();
        foreach (var sr in spriteRenderers)
        {
            float alfa = 0.1f;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alfa);
        }

        inGameCanvas.gameObject.SetActive(false);
        mainCamera.orthographicSize *= 1.5f;
        Instantiate(postProcesser);

        await UniTask.Delay(TimeSpan.FromSeconds(1.5), cancellationToken: token);

        resultCanvas.gameObject.SetActive(true);

        foreach (var item in resultPoints)
        {
            item.SetNumber(10);
        }

        foreach (var item in animatableUI)
        {
            item.OnStart();
        }

        // Debug
        //Instantiate(playCharacterData[TeamType.Red][0].CharacterUI, canvasTransform);
    }
}

public struct ResultData
{
    public TeamType WinningTeam { get; private set; }
    public int WinningTeamPoint { get; private set; }
    public int LoseTeamPoint { get; private set; }


    public ResultData(TeamType winningTeam, int winningTeamPoint, int loseTeamPoint)
    {
        WinningTeam = winningTeam;
        WinningTeamPoint = winningTeamPoint;
        LoseTeamPoint = loseTeamPoint;
    }
}
