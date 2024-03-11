using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using System;

[RequireComponent(typeof(AudioSource))]
public class BGMManager : MonoBehaviour,IDisposable
{
    [SerializeField]
    private AudioClip BGM;
    private AudioSource audioSource;
    private IGameProgressRegistrable gameProgressRegistrable;

    [Inject]
    public void Inject(IGameProgressRegistrable gameProgressRegistrable)
    {
        this.gameProgressRegistrable = gameProgressRegistrable;
        gameProgressRegistrable.OnStart += BGMStart;
    }

    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    private void BGMStart(StartEventArgs startEventArgs)
    {
        audioSource.PlayOneShot(BGM);
    }
    public void Dispose()
    {
        gameProgressRegistrable.OnStart -= BGMStart;
    }
}
