using System.Collections.Generic;
using UnityEngine;
using VContainer;
using Cysharp.Threading.Tasks;
using System.Threading;
using OutGameEnum;

public class PlayerView : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private List<GameObject> sprites = new List<GameObject>();
    [SerializeField]
    private List<GameObject> characterViews = new List<GameObject>();

    private TeamInfo teamInfo;
    private CancellationTokenSource cancellationTokenSource;

    [Inject]
    public void Inject(TeamInfo teamInfo)
    {
        this.teamInfo = teamInfo;
        cancellationTokenSource = new CancellationTokenSource();

        ColorChange();
    }

    private void ColorChange()
    {
        switch (teamInfo.TeamType)
        {
            case TeamType.Red:
                spriteRenderer.color = Color.red;
                break;
            case TeamType.Blue:
                spriteRenderer.color = Color.blue;
                break;
            case TeamType.Yellow:
                spriteRenderer.color = Color.yellow;
                break;
            case TeamType.Green:
                spriteRenderer.color = Color.green;
                break;
        }


    }

    public void Use(int bulletIndex)
    {
        for (int i = 0; i < bulletIndex; i++)
        {
            if (i >= sprites.Count)
                return;

            sprites[i].SetActive(true);
        }
        for (int i = bulletIndex; i < sprites.Count; i++)
        {
            sprites[i].SetActive(false);
        }
    }

    public void NoDraw()
    {
        for (int i = 0; i < sprites.Count; i++)
        {
            sprites[i].SetActive(false);
        }
    }

    public void Life(int hp)
    {
        if (hp <= 0)
        {
            cancellationTokenSource = new CancellationTokenSource();
            DeathAnimationAsync().Forget();
        }
        else
        {
            cancellationTokenSource.Cancel();
        }
    }
    private async UniTaskVoid DeathAnimationAsync()
    {
        while (true)
        {
            if (cancellationTokenSource.IsCancellationRequested)
            {
                break;
            }
            await UniTask.Delay(100);
            foreach(GameObject obj in characterViews)
            {
                obj.SetActive(false);
            }
            if (cancellationTokenSource.IsCancellationRequested)
            {
                break;
            }
            await UniTask.Delay(100);
            foreach (GameObject obj in characterViews)
            {
                obj.SetActive(true);
            }
        }
        foreach (GameObject obj in characterViews)
        {
            obj.SetActive(true);
        }


    }
}


