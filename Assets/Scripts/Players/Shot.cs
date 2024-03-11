using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using System;
using UniRx;
[RequireComponent(typeof(AudioSource))]
public class Shot : MonoBehaviour, IDisposable, IShot
{
    [SerializeField] private PoolObjectAsset poolObject = default;
    [SerializeField]
    protected AudioClip shotSE;
    [SerializeField]
    protected AudioClip chargeShotSE;
    private AudioSource audioSource;


    private InputSystemUseObject inputSystemUseObject;
    protected BulletParamater bulletParamater;
    private TeamInfo teamInfo;
    private IGameProgressRegistrable gameProgressRegistrable;
    private IGameMode gameMode;

    protected CharacterParamater characterParamater;
    protected ReactiveProperty<int> bulletCountProperty = new ReactiveProperty<int>();
    public IReadOnlyReactiveProperty<int> BulletCountProperty => bulletCountProperty;
    private float lastTime = default;
    protected IGettablePool pool;


    private Charge charge = new Charge(0f, false);
    private int chargePower = 0;
    [SerializeField]
    private int startChargePower;

    private bool canShot = true;

    [Inject]
    public void Inject(InputSystemUseObject inputSystemUseObject, BulletParamater bulletParamater, TeamInfo teamInfo, CharacterParamater characterParamater, IGameProgressRegistrable gameProgressRegistrable, IGameMode gameMode)
    {
        this.inputSystemUseObject = inputSystemUseObject;
        this.bulletParamater = bulletParamater;
        this.teamInfo = teamInfo;
        this.characterParamater = characterParamater;
        this.gameProgressRegistrable = gameProgressRegistrable;
        this.gameMode = gameMode;

        pool = new ObjectPooler(poolObject, parentName: "Bullets");
        foreach (PoolObject poolObject in pool.ObjectPool)
        {
            poolObject.Initialize(BulletSettings, poolObject);
        }
        Subscription();

        lastTime = Time.time;
    }

    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    private void BulletSettings(PoolObject poolObject)
    {
        BulletLifetimeScope bulletLifetimeScope = poolObject.GetComponent<BulletLifetimeScope>();

        bulletLifetimeScope.Paramater = bulletParamater;
        bulletLifetimeScope.TeamInfo = teamInfo;
        bulletLifetimeScope.Build();
    }

    private void Teset() { }

    private void Subscription()
    {
        inputSystemUseObject.Performed("Fire", OnFireCharge);
        inputSystemUseObject.Canceled("Fire", OnFire);

        gameProgressRegistrable.OnRestart += RestartHandler;
        gameProgressRegistrable.OnFinish += FinishHandler;
        gameMode.OnCompletedRound += RoundCompletedHandler;
        bulletCountProperty.Value = characterParamater.BulletMaxCount;

        startChargePower = chargePower + characterParamater.BulletParamater.AttackPower;
        chargePower = startChargePower;
    }

    public void Life(int hp)
    {
        if (hp > 0)
        {
            canShot = true;
        }
        else
        {
            canShot = false;
            chargePower = startChargePower;
        }
    }

    private void OnFireCharge(InputAction.CallbackContext context)
    {
        if (!canShot)
        {
            return;
        }
        charge = new Charge(Time.time, true);
    }
    private void OnFire(InputAction.CallbackContext context)
    {
        if (!canShot)
        {
            charge = new Charge(0f, false);
            return;
        }
        OnShot();
        charge = new Charge(0f, false);
    }

    private void OnShot()
    {
        if (!charge.isCharge)
        {
            NormalShot(chargePower);
            
        }
        else if (charge.isCharge)
        {
            ChargeShot(chargePower);
            chargePower = startChargePower;
        }
    }
    protected virtual void NormalShot(int chargePower)
    {
        if (bulletCountProperty.Value <= 0)
        {
            return;
        }
        bulletCountProperty.Value--;

        // Bulletをプールから取得
        PoolObject obj = pool.Get();
        audioSource.PlayOneShot(shotSE);

        // Poolの例外オブジェクト（容量が足りなく途中でInstantiateしたオブジェクト）の場合、手動で初期化処理を実行
        if (!obj.IsInitialCreate)
        {
            obj.Initialize(BulletSettings, obj);
        }
        obj.Enable(transform.position, transform.rotation);

        obj.GetComponent<Bullet>().AttackPower = chargePower;
    }

    protected virtual void ChargeShot(int chargePower)
    {
        // Bulletをプールから取得
        PoolObject obj = pool.Get();

        // Poolの例外オブジェクト（容量が足りなく途中でInstantiateしたオブジェクト）の場合、手動で初期化処理を実行
        if (!obj.IsInitialCreate)
        {
            obj.Initialize(BulletSettings, obj);
        }
        obj.Enable(transform.position, transform.rotation);

        //弾をcharge分大きくする
        obj.transform.localScale *= ((float)chargePower / 1.5f);
        //弾にパワー代入
        obj.GetComponent<Bullet>().AttackPower = chargePower;
    }

    private void Update()
    {
        if (Time.time - lastTime > characterParamater.BulletCooltime && !charge.canCharge)
        {
            if (bulletCountProperty.Value < characterParamater.BulletMaxCount)
            {
                BulletCharge();
            }
            lastTime = Time.time;
        }
        else if (charge.canCharge)
        {
            if (charge.isCharge)
            {
                lastTime = Time.time;
            }

            if (charge.CheckCompletionCharge(characterParamater.ChargeTime))
            {
                if (bulletCountProperty.Value > 0)
                {
                    bulletCountProperty.Value--;
                    chargePower++;
                }
            }
        }
    }

    protected virtual void BulletCharge()
    {
        bulletCountProperty.Value++;
    }

    private void RestartHandler(StartEventArgs startEventArgs)
    {
        bulletCountProperty.Value = characterParamater.BulletMaxCount;
        inputSystemUseObject.Performed("Fire", OnFireCharge);
        inputSystemUseObject.Canceled("Fire", OnFire);
    }

    private void FinishHandler(EndEventArgs endEventArgs)
    {
        inputSystemUseObject.PerformedCancellation("Fire", OnFireCharge);
        inputSystemUseObject.CanceledCancellation("Fire", OnFire);
    }

    private void RoundCompletedHandler(RoundEventArgs roundEventArgs)
    {
        charge = new Charge(0f, false);
        inputSystemUseObject.PerformedCancellation("Fire", OnFireCharge);
        inputSystemUseObject.CanceledCancellation("Fire", OnFire);
    }


    public void Dispose()
    {
        gameProgressRegistrable.OnRestart -= RestartHandler;
        gameProgressRegistrable.OnStart -= RestartHandler;
        gameMode.OnCompletedRound -= RoundCompletedHandler;
    }
}

public class Charge
{
    public float startChargeTime;
    public bool canCharge;

    public bool isCharge;
    public Charge(float startChargeTime, bool canCharge)
    {
        this.startChargeTime = startChargeTime;
        this.canCharge = canCharge;
        isCharge = false;
    }

    public bool CheckCompletionCharge(float chargeTime)
    {
        if (Time.time - startChargeTime > chargeTime)
        {
            startChargeTime = Time.time;
            isCharge = true;
            return true;
        }
        return false;
    }
}