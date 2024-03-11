using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using System;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerMove : MonoBehaviour, IDisposable
{
    private InputSystemUseObject inputSystemUseObject;
    private IMovable movable;
    private Vector2 moveDir = default;
    private Vector2 rotateDir = default;
    private CharacterParamater paramater;
    private IGameProgressRegistrable gameProgressRegistrable;
    private IReadOnlyPositionAdapter readOnlyPositionAdapter;

    private Action PauseAction;
    private Action MoveAction;

    [SerializeField] private ContactFilter2D contactFilter2D = default;
    private CircleCollider2D myCollider = default;
    private Collider2D[] hitColliders = new Collider2D[2];
    private SimpleReflectLogic simpleReflect = new SimpleReflectLogic();
    private Transform myTransform = default;

    private bool canMove = true;
    private bool onLock = false;

    [Inject]
    public void Inject(IMovable movable, InputSystemUseObject inputSystemUseObject, CharacterParamater paramater, IGameProgressRegistrable gameProgressRegistrable, IReadOnlyPositionAdapter readOnlyPositionAdapter)
    {
        this.movable = movable;
        this.inputSystemUseObject = inputSystemUseObject;
        this.paramater = paramater;
        this.gameProgressRegistrable = gameProgressRegistrable;
        this.readOnlyPositionAdapter = readOnlyPositionAdapter;

        Subscription();
    }

    private void Awake()
    {
        myTransform = this.transform;
        myCollider = this.GetComponent<CircleCollider2D>();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (onLock)
        {
            moveDir = Vector2.zero;
        }
        else
        {
            moveDir = context.ReadValue<Vector2>();
        }

        rotateDir = context.ReadValue<Vector2>();
    }

    private void OnMoveStarted(InputAction.CallbackContext context)
    {
        MoveAction += () =>
        {
            CorrectTheVectorIfCollision();
            myTransform.RotateInVectorDirection(rotateDir);
            movable.Move(moveDir, paramater.MoveSpeed);
        };
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        MoveAction = null;
        moveDir = Vector2.zero;
    }

    private void OnLock(InputAction.CallbackContext context)
    {
        onLock = true;
    }

    private void OnLockCanceled(InputAction.CallbackContext context)
    {
        onLock = false;
    }

    /// <summary>
    /// イベント購読
    /// </summary>
    private void Subscription()
    {
        inputSystemUseObject.Performed("Move", OnMove);
        inputSystemUseObject.Started("Move", OnMoveStarted);
        inputSystemUseObject.Canceled("Move", OnMoveCanceled);

        inputSystemUseObject.Performed("Lock", OnLock);
        inputSystemUseObject.Canceled("Lock", OnLockCanceled);

        gameProgressRegistrable.OnPause += PauseHandler;
        gameProgressRegistrable.OnRestart += RestartHandler;
        gameProgressRegistrable.OnFinish += FinishHandler;

        PauseAction += Move;

        myTransform.position = readOnlyPositionAdapter.Position;
    }

    private void PauseHandler(PauseEventArgs pauseEventArgs)
    {
        if (pauseEventArgs.isPause)
        {
            PauseAction = null;
        }
        else
        {
            PauseAction += Move;
        }
    }

    private void FinishHandler(EndEventArgs endEventArgs)
    {
        PauseAction = null;
    }

    private void Move()
    {
        MoveAction?.Invoke();
    }

    public void Life(int hp)
    {
        if (hp <= 0)
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
    }

    private void Update()
    {
        if (canMove)
        {
            PauseAction?.Invoke();
        }
    }
    public void Dispose()
    {
        MoveAction = null;
        gameProgressRegistrable.OnPause -= PauseHandler;
    }

    private void RestartHandler(StartEventArgs startEventArgs)
    {
        myTransform.position = readOnlyPositionAdapter.Position;
    }

    /// <summary>
    /// 壁の判定を取り、進行方向を補正する
    /// </summary>
    private void CorrectTheVectorIfCollision()
    {
        // 当たり判定を取得
        int hitCount = myCollider.OverlapCollider(contactFilter2D, hitColliders);

        switch (hitCount)
        {
            case 0:
                return;

            case 1:
                // 当たったコライダーをダウンキャスト
                if (hitColliders[0] is BoxCollider2D boxCollider_a)
                {
                    // 当たったコライダーの法線ベクトルを算出（重複呼び出しの場合、入射ベクトルがそのまま返される）
                    Vector2 hitNormal = simpleReflect.GetNormal(myTransform.position, moveDir, boxCollider_a);

                    // 重複呼び出しだったら、処理を終了
                    if (hitNormal == moveDir)
                    {
                        return;
                    }

                    // 壁のベクトルを算出（法線に対し垂直なベクトル）
                    Vector2 wallDir = Vector3.Cross(hitNormal, Vector3.forward).normalized;

                    // 入射ベクトルと壁のベクトルの角度の大きさを算出
                    float inDirAngleSize_a = Mathf.Abs(Vector2.Angle(moveDir, wallDir));

                    // 角度の大きさが90°より大きければ、反転
                    if (inDirAngleSize_a > 90f)
                    {
                        wallDir *= -1f;
                    }

                    // 改めて、壁と進行方向のベクトルの角度の大きさを取得
                    float inDirAngleSize_A = Mathf.Abs(Vector2.Angle(moveDir, wallDir));

                    // 角度が0°に近ければ近いほど速く移動する
                    float magnitude = 1 - (inDirAngleSize_A / 90f);
                    moveDir = wallDir * (moveDir.magnitude * magnitude);
                }
                else
                {
                    Debug.LogError("プレイヤーがBoxCollider2D以外を持ったCollisionレイヤーオブジェクトに当たっています。");
                }

                break;

            case 2:
                Vector2[] hitNormals = new Vector2[2];

                for (int i = 0; i < 2; i++)
                {
                    // 当たったコライダーをダウンキャスト
                    if (hitColliders[i] is BoxCollider2D boxCollider_b)
                    {
                        // 当たったコライダーの法線ベクトルを算出（重複呼び出しの場合、入射ベクトルがそのまま返される）
                        hitNormals[i] = simpleReflect.GetNormal(myTransform.position, boxCollider_b);
                    }
                    else
                    {
                        Debug.LogError("プレイヤーがBoxCollider2D以外を持ったCollisionレイヤーオブジェクトに当たっています。");
                    }
                }

                // 壁のベクトルを算出（法線に対し垂直なベクトル）
                // ベクトルの向きを外側または内側で統一したいためｄ、片方の軸を反転して算出
                Vector2 wallDir1 = Vector3.Cross(hitNormals[0], Vector3.forward).normalized;
                Vector2 wallDir2 = Vector3.Cross(hitNormals[1], Vector3.back).normalized;

                // 壁のベクトルを外側に強制する
                // ある一方の壁のベクトルと、もう一方の法線ベクトルの角度の大きさが90°以上だったら、
                // 壁のベクトルは内側を向いていると判断
                if (Mathf.Abs(Vector2.Angle(wallDir1, hitNormals[1])) > 90f)
                {
                    wallDir1 *= -1;
                    wallDir2 *= -1;
                }

                // 壁のベクトルの角度の半分の大きさを算出
                float wallHalfAngleSize = Mathf.Abs(Vector2.Angle(wallDir1, wallDir2) / 2);

                // 2つの法線ベクトルを合成したベクトルを算出
                Vector2 halfNormal = (hitNormals[0] + hitNormals[1]).normalized;

                // 進行方向と合成した法線ベクトルの角度の大きさを算出
                float inDirAngleSize_b = Vector2.Angle(moveDir, halfNormal);

                // 角ではない方向に進もうとしているとき
                if (inDirAngleSize_b < wallHalfAngleSize + 90f && inDirAngleSize_b > wallHalfAngleSize)
                {
                    // 壁と進行方向のベクトルの角度の大きさを取得
                    float inDirAngleSize_wall1 = Mathf.Abs(Vector2.Angle(moveDir, wallDir1));
                    float inDirAngleSize_wall2 = Mathf.Abs(Vector2.Angle(moveDir, wallDir2));
                    float magnitude;

                    // 壁に沿って進む
                    if (inDirAngleSize_wall1 < 90f)
                    {
                        // 角度が0°に近ければ近いほど速く移動する
                        magnitude = 1 - (inDirAngleSize_wall1 / 90f);
                        moveDir = wallDir1 * (moveDir.magnitude * magnitude);
                    }
                    // 壁に沿って進む
                    else if (inDirAngleSize_wall2 < 90f)
                    {
                        magnitude = 1 - (inDirAngleSize_wall2 / 90f);
                        moveDir = wallDir2 * (moveDir.magnitude * magnitude);
                    }
                    else
                    {
                        moveDir = Vector2.zero;
                    }
                }
                // 壁に向かって進行しようとしていたら、動けなくする
                else if (Mathf.Abs(inDirAngleSize_b) > wallHalfAngleSize)
                {
                    moveDir = Vector2.zero;
                }

                break;
        }
    }
}
