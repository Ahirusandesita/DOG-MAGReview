using UnityEngine;
using VContainer;
using UniRx;
using System;
using Cysharp.Threading.Tasks;

public class Status : MonoBehaviour, IDamagable, IDisposable
{
    private TeamInfo teamInfo;
    private CharacterParamater characterParamater;
    private IGameProgressRegistrable gameProgressRegistrable;
    private ReactiveProperty<int> hpProperty = new ReactiveProperty<int>();
    public IReadOnlyReactiveProperty<int> HpProperty => hpProperty;



    public TeamInfo TeamInfo
    {
        get
        {
            return teamInfo;
        }
    }

    public bool IsInvincible { get; private set; } = false;

    [Inject]
    public void Inject(TeamInfo teamInfo, CharacterParamater characterParamater,IGameProgressRegistrable gameProgressRegistrable)
    {
        this.teamInfo = teamInfo;
        this.characterParamater = characterParamater;
        this.gameProgressRegistrable = gameProgressRegistrable;

        gameProgressRegistrable.OnRestart += Restart;

        hpProperty.Value = characterParamater.Life;
    }
    public void TakeDamage(TeamInfo teamInfo, int damage = 1, Vector2 impactDir = default)
    {
        if(teamInfo.TeamType == this.teamInfo.TeamType)
        {
            return;
        }

        if (hpProperty.Value > 0 && !IsInvincible)
        {
            hpProperty.Value -= damage;
        }

        if(hpProperty.Value <= 0)
        {
            IsInvincible = true;
            DeathAysnc().Forget();
        }
    }

    private void Resuscitation()
    {
        hpProperty.Value = characterParamater.Life;
        InvincibleTime().Forget();
    }

    private void Restart(StartEventArgs startEventArgs)
    {
        hpProperty.Value = characterParamater.Life;
    }

    private void FinishInvincible()
    {
        IsInvincible = false;
    }

    public void Dispose()
    {
        hpProperty.Dispose();
    }
    
    private async UniTaskVoid DeathAysnc()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(characterParamater.DeathPenalty));
        Resuscitation();
    }

    private async UniTaskVoid InvincibleTime()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1.5f));
        FinishInvincible();
    }


}
