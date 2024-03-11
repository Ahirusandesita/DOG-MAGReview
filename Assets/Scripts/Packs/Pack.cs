using System;
using System.Threading;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using VContainer;
using Random = UnityEngine.Random;
using OutGameEnum;

[RequireComponent(typeof(CircleCollider2D))]
public class Pack : MonoBehaviour, IDamagable
{
    [SerializeField] private PackAsset packAsset = default;
    [SerializeField] private ContactFilter2D contactFilter2D = default;

    private IGameProgressRegistrable gameProgressRegistrable = default;
    private IGameMode gameMode = default;
    private IMovable movable = default;
    private IReflectable reflectable = default;

    private Transform myTransform = default;
    private CircleCollider2D myCollider = default;
    private Vector3 initialScale = default;
    private CancellationToken token = default;

    private float baseMoveSpeed = default;
    private float currentMoveSpeed = default;
    private float speedUpTime = default;
    private bool isSpeedUp = false;
    private TeamInfo hitTeamInfo = default;
    private bool isAnimation = false;

    private readonly ReactiveProperty<int> reflectCountProperty = new ReactiveProperty<int>();

    private Action UpdateAction = default;
    private Action PauseAction = default;

    private ParticleSystem[] onGoalParticles = default;
    private readonly Collider2D[] hitColliders = new Collider2D[4];

    //Debug(for Color)
    private SpriteRenderer spriteRenderer = default;

    // (for enabled)
    private SpriteRenderer[] spriteRenderers = default;

    public TeamInfo TeamInfo
    {
        get
        {
            return new NullTeamInfo();
        }
    }

    public bool IsInvincible { get => false; }

    [Inject]
    public void Inject(IGameProgressRegistrable gameProgressRegistrable, IGameMode gameMode, IMovable movable, IReflectable reflectable)
    {
        this.gameProgressRegistrable = gameProgressRegistrable;
        this.gameMode = gameMode;
        this.movable = movable;
        this.reflectable = reflectable;

        Subscription();
    }


    private void Awake()
    {
        myTransform = this.transform;
        myCollider = this.GetComponent<CircleCollider2D>();
        initialScale = myTransform.localScale;
        token = this.GetCancellationTokenOnDestroy();

        baseMoveSpeed = packAsset.BaseMoveSpeed;
        currentMoveSpeed = baseMoveSpeed;

        // Particleの初期生成
        onGoalParticles = new ParticleSystem[packAsset.OnGoalParticles.Count];
        for (int i = 0; i < onGoalParticles.Length; i++)
        {
            onGoalParticles[i] = Instantiate(packAsset.OnGoalParticles[i], myTransform);
            onGoalParticles[i].gameObject.SetActive(false);
        }

        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderers = this.GetComponentsInChildren<SpriteRenderer>();
        // 見た目を非表示
        foreach (var sr in spriteRenderers)
        {
            sr.enabled = false;
        }
    }

    private void Update()
    {
        PauseAction?.Invoke();
    }


    private void Subscription()
    {
        gameProgressRegistrable.OnStart += StartHandler;
        gameProgressRegistrable.OnRestart += RestartHandler;
        gameProgressRegistrable.OnPause += PauseHandler;
        gameProgressRegistrable.OnFinish += FinishHandler;
        gameMode.OnCompletedRound += CompletedRoundHandler;

        UpdateAction += () =>
        {
            CheckCollision();
            movable.Move(myTransform.up, currentMoveSpeed);
        };

        // 規定の反射回数に到達したとき、メッセージを発行
        reflectCountProperty
            .Where(value => value == packAsset.ReflectionRequiredTimesForSpeedup)
            .Subscribe(value => SpeedupOnReflection())
            .AddTo(this);
    }

    private async void StartHandler(StartEventArgs startEventArgs)
    {
        // 見た目を表示
        foreach (var sr in spriteRenderers)
        {
            sr.enabled = true;
        }

        try
        {
            // 出現
            await PopupAsync();
        }
        catch (OperationCanceledException){ }

        myTransform.RotateInVectorDirection(RandomDirection());
        PauseAction += () => UpdateAction?.Invoke();
        gameProgressRegistrable.OnStart -= StartHandler;
    }

    private async void RestartHandler(StartEventArgs startEventArgs)
    {
        // リセット
        myTransform.position = Vector3.zero;
        myTransform.rotation = Quaternion.Euler(Vector3.up);
        spriteRenderer.color = Color.white;

        hitTeamInfo = null;

        try
        {
            // 出現
            await PopupAsync();
        }
        catch (OperationCanceledException) { }

        myTransform.RotateInVectorDirection(RandomDirection());
        PauseAction += () => UpdateAction?.Invoke();
    }

    private void CompletedRoundHandler(RoundEventArgs roundEventArgs)
    {
        foreach (var particle in onGoalParticles)
        {
            particle.gameObject.SetActive(true);
            particle.Play();
        }

        PauseAction = null;
        reflectCountProperty.Value = 0;
        baseMoveSpeed = packAsset.BaseMoveSpeed;
        currentMoveSpeed = baseMoveSpeed;
    }

    private void PauseHandler(PauseEventArgs pauseEventArgs)
    {
        if (pauseEventArgs.isPause)
        {
            PauseAction = null;
        }
        else
        {
            PauseAction += () => UpdateAction?.Invoke();
        }
    }

    private void FinishHandler(EndEventArgs endEventArgs)
    {
        PauseAction = null;
    }

    public void TakeDamage(TeamInfo teamInfo, int damage = 1, Vector2 impactDir = default)
    {
        if (isAnimation)
        {
            return;
        }

        hitTeamInfo = teamInfo;

        //Debug
        switch (teamInfo.TeamType)
        {
            case TeamType.Red:
                spriteRenderer.color = Color.red;
                break;
            case TeamType.Blue:
                spriteRenderer.color = Color.blue;
                break;
        }

        KnockBack(impactDir, damage);
    }


    /// <summary>
    /// 当たり判定を取得する
    /// </summary>
    private void CheckCollision()
    {
        // 当たり判定を取得
        int hitCount = myCollider.OverlapCollider(contactFilter2D, hitColliders);
        for (int i = 0; i < hitCount; i++)
        {
            // 当たったオブジェクトがIDamagableを実装していれば、被弾させる
            if (hitColliders[i].TryGetComponent(out IDamagable damagable))
            {
                TeamInfo teamInfo;

                if (hitTeamInfo is null)
                {
                    teamInfo = new NullTeamInfo();
                }
                else
                {
                    teamInfo = hitTeamInfo;
                }

                damagable.TakeDamage(teamInfo, 999, myTransform.up);
                return;
            }

            // 当たったオブジェクトがゴールなら、ゴール判定を取る
            if (hitColliders[i].TryGetComponent(out Goal goal))
            {
                goal.GoalIn(hitTeamInfo);
                return;
            }

            // 当たったコライダーをダウンキャストし、そのコライダーに反射する
            // （Boxにのみ反射できるアルゴリズムのため）
            if (hitColliders[i] is BoxCollider2D boxCollider)
            {
                bool result = reflectable.Reflect(myTransform.up, boxCollider);

                if (result)
                {
                    reflectCountProperty.Value++;
                }
            }
        }
    }

    /// <summary>
    /// ノックバックする
    /// </summary>
    /// <param name="knockBackDir">ノックバックする方向</param>
    private void KnockBack(Vector2 knockBackDir, float power)
    {
        // ノックバックする方向を向く
        myTransform.RotateInVectorDirection(knockBackDir);

        // ノックバック方向に一時的に加速する------------------------------

        // スピードアップ時間を更新
        speedUpTime = packAsset.TimeToSpeedupOnHit;

        // 重複して速度の乗算はしない
        if (!isSpeedUp)
        {
            isSpeedUp = true;
            currentMoveSpeed *= packAsset.SpeedupScaleOnHit + (packAsset.SpeedupScaleOnBulletPower - 1) * power;

            // カウントダウン開始
            UpdateAction += SpeedupTimeCount;
        }
        // ----------------------------------------------------------------
    }

    /// <summary>
    ///  スピードアップ時間のカウントダウン
    /// </summary>
    private void SpeedupTimeCount()
    {
        speedUpTime -= Time.deltaTime;

        // スピードアップ時間が終了したら、元に戻し、この関数をPlayerLoopから除外する
        if (speedUpTime <= 0f)
        {
            currentMoveSpeed = baseMoveSpeed;
            isSpeedUp = false;
            UpdateAction -= SpeedupTimeCount;
        }
    }

    /// <summary>
    /// Packが出現するときのアニメーション
    /// </summary>
    /// <returns></returns>
    private async UniTask PopupAsync()
    {
        const int loopTimes = 150;

        isAnimation = true;
        myTransform.localScale = Vector3.zero;

        await UniTask.Delay(TimeSpan.FromSeconds(0.8), cancellationToken: token);

        for (int i = 0; i < loopTimes; i++)
        {
            myTransform.localScale += initialScale / (loopTimes * 2);
            await UniTask.Yield(token);
        }

        for (int i = 0; i < loopTimes / 2; i++)
        {
            myTransform.localScale += initialScale / loopTimes;
            await UniTask.Yield(token);
        }

        myTransform.localScale = initialScale;

        await UniTask.Delay(TimeSpan.FromSeconds(0.8), cancellationToken: token);
        isAnimation = false;
    }

    /// <summary>
    /// 上方向か下方向かランダムで決定する
    /// </summary>
    /// <returns></returns>
    private Vector2 RandomDirection()
    {
        if (Random.Range(0, 2) == 0)
        {
            return Vector2.up;
        }
        else
        {
            return Vector2.down;
        }
    }

    /// <summary>
    /// 反射回数に応じてスピードアップする
    /// </summary>
    private void SpeedupOnReflection()
    {
        // 反射回数をリセット
        reflectCountProperty.Value = 0;

        // スピードを底上げする（ゴールが入るまでこのままのスピード）
        baseMoveSpeed *= packAsset.SpeedupScaleOnReflection;

        // スピードの上限は2倍まで
        if (baseMoveSpeed > packAsset.BaseMoveSpeed * 2)
        {
            baseMoveSpeed = packAsset.BaseMoveSpeed * 2;
        }

        currentMoveSpeed = baseMoveSpeed;
    }
}
