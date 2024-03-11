using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Cysharp.Threading.Tasks;
using System;
[RequireComponent(typeof(AudioSource))]
public class EventUI : MonoBehaviour, IDisposable
{
    private IGameProgressRegistrable gameProgressRegistrable;
    [SerializeField]
    private Image startImage;
    [SerializeField]
    private Image finishImage;

    [SerializeField]
    private AudioClip startSE;
    private AudioSource audioSource;

    [Inject]
    public void Inject(IGameProgressRegistrable gameProgressRegistrable)
    {
        this.gameProgressRegistrable = gameProgressRegistrable;

        Subscription();
    }

    private void Subscription()
    {
        gameProgressRegistrable.OnStart += StartImageDisplay;
        gameProgressRegistrable.OnFinish += FinishImageDisplay;
    }

    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
        startImage.enabled = false;
        finishImage.enabled = false;
    }

    private void StartImageDisplay(StartEventArgs startEventArgs)
    {
        startImage.enabled = true;
        StartAnimation().Forget();
    }

    private void FinishImageDisplay(EndEventArgs endEventArgs)
    {
        finishImage.enabled = true;
        FinishAnimation().Forget();
    }

    private async UniTaskVoid StartAnimation()
    {
        audioSource.PlayOneShot(startSE);
        await UniTask.Delay(2000);
        startImage.enabled = false;
    }

    private async UniTaskVoid FinishAnimation()
    {
        await UniTask.Delay(2000);
        finishImage.enabled = false;
    }

    public void Dispose()
    {
        gameProgressRegistrable.OnStart -= StartImageDisplay;
        gameProgressRegistrable.OnFinish -= FinishImageDisplay;
    }
}
