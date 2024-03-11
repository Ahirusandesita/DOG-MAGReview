using System;
using UnityEngine;
using VContainer;
using OutGameEnum;

[RequireComponent(typeof(CircleCollider2D))]
public class Bullet : PoolObject
{
    [SerializeField] ContactFilter2D contactFilter2D = default;

    private IGameProgressRegistrable gameProgressRegistrable = default;
    private IGameMode gameMode = default;
    private IMovable movable = default;
    private IReflectable reflectable = default;
    private BulletParamater bulletParamater = default;
    private TeamInfo teamInfo = default;

    private CircleCollider2D myCollider = default;
    private Vector3 initialScale = default;
    private float lifeTime = default;
    private int life = default;

    private Action UpdateAction = default;
    private Action PauseAction = default;

    private readonly Collider2D[] hitColliders = new Collider2D[4];

    private int attackPower;
    public int AttackPower
    {
        get => attackPower;
        set
        {
            attackPower = value;
            life = attackPower;
        }
    }

    public TeamInfo TeamInfo => teamInfo;

    //Debug
    private SpriteRenderer spriteRenderer;
    [Inject]
    public void Inject(IGameProgressRegistrable gameProgressRegistrable, IGameMode gameMode, IMovable movable, IReflectable reflectable, BulletParamater bulletParamater, TeamInfo teamInfo)
    {
        this.gameProgressRegistrable = gameProgressRegistrable;
        this.gameMode = gameMode;
        this.movable = movable;
        this.reflectable = reflectable;
        this.bulletParamater = bulletParamater;
        this.teamInfo = teamInfo;
        Subscription();

        //Debug
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        switch (teamInfo.TeamType)
        {
            case TeamType.Red:
                spriteRenderer.color = Color.red;
                break;
            case TeamType.Blue:
                spriteRenderer.color = Color.blue;
                break;
        }
    }

    private void Awake()
    {
        myTransform = this.transform;
        myCollider = this.GetComponent<CircleCollider2D>();
        initialScale = myTransform.localScale;
    }

    private void Update()
    {
        PauseAction?.Invoke();
    }


    private void Subscription()
    {
        gameProgressRegistrable.OnPause += PauseHandler;

        UpdateAction += () =>
        {
            CheckCollision();
            movable.Move(myTransform.up, bulletParamater.BulletSpeed);
        };

        PauseAction += () => UpdateAction?.Invoke();
    }

    public override void Enable(Vector2 initialPos, Quaternion initialDir)
    {
        base.Enable(initialPos, initialDir);

        myTransform.localScale = initialScale;
        lifeTime = bulletParamater.BulletLifeTime;
        UpdateAction += LifeTimeCount;
    }

    public override void Disable()
    {
        UpdateAction -= LifeTimeCount;
        life = bulletParamater.AttackPower;
        base.Disable();
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

    /// <summary>
    /// 当たり判定を取得する
    /// </summary>
    private void CheckCollision()
    {
        // 当たり判定を取得
        int hitCount = myCollider.OverlapCollider(contactFilter2D, hitColliders);

        for (int i = 0; i < hitCount; i++)
        {
            // 当たったオブジェクトがIDamagableを実装していれば、被弾させ消滅する
            if (hitColliders[i].TryGetComponent(out IDamagable damagable))
            {
                // 同チームだったら処理を終了
                if (damagable.TeamInfo.TeamType == teamInfo.TeamType)
                {
                    return;
                }

                // 無敵中だったら処理を終了
                if (damagable.IsInvincible)
                {
                    return;
                }

                damagable.TakeDamage(teamInfo, attackPower, myTransform.up);

                Pool.Return(this);
                return;
            }

            // 当たったオブジェクトがBulletなら、ともに消滅させる
            if (hitColliders[i].TryGetComponent(out Bullet bullet))
            {
                if (bullet.TeamInfo.TeamType != teamInfo.TeamType)
                {
                    // 相手の方が強い弾だったら、処理を相手に任せる
                    if (bullet.AttackPower > attackPower)
                    {
                        return;
                    }

                    // 自分の弾の体力を減らし、相手の弾を消滅させる
                    TakeDamage(bullet.AttackPower);
                    bullet.Pool.Return(bullet);
                    return;
                }
            }

            // 当たったコライダーをダウンキャストし、そのコライダーに反射する
            // （Boxにのみ反射できるアルゴリズムのため）
            if (hitColliders[i] is BoxCollider2D boxCollider)
            {
                reflectable.Reflect(myTransform.up, boxCollider);
            }
        }
    }

    /// <summary>
    /// 弾の生存時間のカウントダウン
    /// </summary>
    private void LifeTimeCount()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0f)
        {
            Pool.Return(this);
        }
    }

    private void TakeDamage(int damage)
    {
        life -= damage;
        if (life <= 0)
        {
            Pool.Return(this);
        }
    }
}
