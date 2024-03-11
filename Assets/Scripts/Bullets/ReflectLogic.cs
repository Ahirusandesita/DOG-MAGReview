using UnityEngine;
using VContainer;

/// <summary>
/// 反射の挙動を計算するクラス
/// </summary>
public class ReflectLogic : IReflectable
{
    private readonly IPositionAdapter positionAdapter;
    private readonly IRotationAdapter rotationAdapter;

    private readonly (int x, int y) topRight = (1, 1);
    private readonly (int x, int y) topLeft = (-1, 1);
    private readonly (int x, int y) bottomRight = (1, -1);
    private readonly (int x, int y) bottomLeft = (-1, -1);


    [Inject]
    public ReflectLogic(IPositionAdapter positionAdapter, IRotationAdapter rotationAdapter)
    {
        this.positionAdapter = positionAdapter;
        this.rotationAdapter = rotationAdapter;
    }

    public bool Reflect(Vector2 inDirection, BoxCollider2D targetCollider)
    {
        Transform targetTransform = targetCollider.transform;

        // targetのx軸、y軸それぞれの辺の半分のベクトルを算出
        // 回転を考慮するため、法線ベクトルを掛ける
        Vector2 targetHalfVector_x = targetCollider.size.x * targetTransform.localScale.x / 2 * targetTransform.right;
        Vector2 targetHalfVector_y = targetCollider.size.y * targetTransform.localScale.y / 2 * targetTransform.up;

        // targetの右上の座標
        Vector2 targetTopRightPos = (Vector2)targetTransform.position + targetHalfVector_x + targetHalfVector_y;
        // targetの左下の座標
        Vector2 targetBottomLeftPos = (Vector2)targetTransform.position - targetHalfVector_x - targetHalfVector_y;


        // 「targetの右上の座標」から見た、Hitした座標（左および下を-1、右および上を1と表現）
        (int x, int y) signOfHitPosFromTopRight;
        // 「targetの左下の座標」から見た、Hitした座標
        (int x, int y) signOfHitPosFromBottomLeft;

        // Hitした座標を、targetから見たローカル座標に変換
        Vector2 hitPosLocal = targetTransform.InverseTransformPoint(positionAdapter.Position);
        // targetの右上の座標を、targetから見たローカル座標に変換
        Vector2 targetTopRightPosLocal = targetTransform.InverseTransformPoint(targetTopRightPos);
        // targetの左下の座標を、targetから見たローカル座標に変換
        Vector2 targetBottomLeftPosLocal = targetTransform.InverseTransformPoint(targetBottomLeftPos);


        // -1を左方向、1を右方向としてintで表現したローカル座標を算出
        // つまり右上または左下から見て、Hit座標はどの方向か
        signOfHitPosFromTopRight.x = (int)Mathf.Sign(hitPosLocal.x - targetTopRightPosLocal.x);
        signOfHitPosFromTopRight.y = (int)Mathf.Sign(hitPosLocal.y - targetTopRightPosLocal.y);

        signOfHitPosFromBottomLeft.x = (int)Mathf.Sign(hitPosLocal.x - targetBottomLeftPosLocal.x);
        signOfHitPosFromBottomLeft.y = (int)Mathf.Sign(hitPosLocal.y - targetBottomLeftPosLocal.y);


        // 　　　　　　　　　　　┌───────────────●　←targetの右上の座標
        // 　　　　　　　　　　　｜　　　　　　　　　　　　　　　｜
        // 　　　　　　　　　　　｜　　　　　　　　　　　　　　　｜
        // 　　　　　　　　　　　｜　　　　　　　　　　　　　　　｜
        // 　　　　　　　　　　　｜　　　　　　Square　　　　　　｜
        // 　　　　　　　　　　　｜　　　　　　　　　　　　　　　｜
        // 　　　　　　　　　　　｜　　　　　　　　　　　　　　　｜
        // 　　　　　　　　　　　｜　　　　　　　　　　　　　　　｜
        // targetの左下の座標→　●───────────────┘


        // 算出したtargetの法線ベクトル
        Vector2 targetNormal = default;

        // 上面（Hit座標は、targetの右上座標から見て[左上の方向] AND targetの左下座標から見て[右上の方向]）
        if (signOfHitPosFromTopRight == topLeft && signOfHitPosFromBottomLeft == topRight)
        {
            targetNormal = targetTransform.up;
        }
        // 右面
        else if (signOfHitPosFromTopRight == bottomRight && signOfHitPosFromBottomLeft == topRight)
        {
            targetNormal = targetTransform.right;
        }
        // 下面
        else if (signOfHitPosFromTopRight == bottomLeft && signOfHitPosFromBottomLeft == bottomRight)
        {
            targetNormal = -targetTransform.up;
        }
        // 左面
        else if (signOfHitPosFromTopRight == bottomLeft && signOfHitPosFromBottomLeft == topLeft)
        {
            targetNormal = -targetTransform.right;
        }
        // 右上角
        else if (signOfHitPosFromTopRight == topRight && signOfHitPosFromBottomLeft == topRight)
        {
            targetNormal = (targetTransform.up + targetTransform.right).normalized;
        }
        // 右下角
        else if (signOfHitPosFromTopRight == bottomRight && signOfHitPosFromBottomLeft == bottomRight)
        {
            targetNormal = (-targetTransform.up + targetTransform.right).normalized;
        }
        // 左下角
        else if (signOfHitPosFromTopRight == bottomLeft && signOfHitPosFromBottomLeft == bottomLeft)
        {
            targetNormal = (-targetTransform.up + -targetTransform.right).normalized;
        }
        // 左上角
        else if (signOfHitPosFromTopRight == topLeft && signOfHitPosFromBottomLeft == topLeft)
        {
            targetNormal = (targetTransform.up + -targetTransform.right).normalized;
        }


        // 入射角と法線ベクトルの角の大きさを算出
        float inDirAngleSize = Mathf.Abs(Vector2.Angle(inDirection, targetNormal));

        // 2つのベクトルの角度の大きさの絶対値が、0°以上90°以下のとき、重複反射と判定し処理を終了
        if (inDirAngleSize >= 0 && inDirAngleSize <= 90)
        {
            return false;
        }


        // 入射角と法線ベクトルから、反射角を算出
        Vector2 reflectDir = Vector2.Reflect(inDirection, targetNormal);

        // 反射角の方向を向く
        rotationAdapter.RotateInVectorDirection(reflectDir);
        return true;
    }
}

/// <summary>
/// 簡易的な反射計算クラス
/// </summary>
public class SimpleReflectLogic
{
    private readonly (int x, int y) topRight = (1, 1);
    private readonly (int x, int y) topLeft = (-1, 1);
    private readonly (int x, int y) bottomRight = (1, -1);
    private readonly (int x, int y) bottomLeft = (-1, -1);


    /// <summary>
    /// 当たったオブジェクトの法線を取得する（当たったオブジェクトが複数のとき）
    /// </summary>
    public Vector2 GetNormal(Vector2 origin, BoxCollider2D targetCollider)
    {
        Transform targetTransform = targetCollider.transform;

        // targetのx軸、y軸それぞれの辺の半分のベクトルを算出
        // 回転を考慮するため、法線ベクトルを掛ける
        Vector2 targetHalfVector_x = targetCollider.size.x * targetTransform.localScale.x / 2 * targetTransform.right;
        Vector2 targetHalfVector_y = targetCollider.size.y * targetTransform.localScale.y / 2 * targetTransform.up;

        // targetの右上の座標
        Vector2 targetTopRightPos = (Vector2)targetTransform.position + targetHalfVector_x + targetHalfVector_y;
        // targetの左下の座標
        Vector2 targetBottomLeftPos = (Vector2)targetTransform.position - targetHalfVector_x - targetHalfVector_y;


        // 「targetの右上の座標」から見た、Hitした座標（左および下を-1、右および上を1と表現）
        (int x, int y) signOfHitPosFromTopRight;
        // 「targetの左下の座標」から見た、Hitした座標
        (int x, int y) signOfHitPosFromBottomLeft;

        // Hitした座標を、targetから見たローカル座標に変換
        Vector2 hitPosLocal = targetTransform.InverseTransformPoint(origin);
        // targetの右上の座標を、targetから見たローカル座標に変換
        Vector2 targetTopRightPosLocal = targetTransform.InverseTransformPoint(targetTopRightPos);
        // targetの左下の座標を、targetから見たローカル座標に変換
        Vector2 targetBottomLeftPosLocal = targetTransform.InverseTransformPoint(targetBottomLeftPos);


        // -1を左方向、1を右方向としてintで表現したローカル座標を算出
        // つまり右上または左下から見て、Hit座標はどの方向か
        signOfHitPosFromTopRight.x = (int)Mathf.Sign(hitPosLocal.x - targetTopRightPosLocal.x);
        signOfHitPosFromTopRight.y = (int)Mathf.Sign(hitPosLocal.y - targetTopRightPosLocal.y);

        signOfHitPosFromBottomLeft.x = (int)Mathf.Sign(hitPosLocal.x - targetBottomLeftPosLocal.x);
        signOfHitPosFromBottomLeft.y = (int)Mathf.Sign(hitPosLocal.y - targetBottomLeftPosLocal.y);


        // 　　　　　　　　　　　┌───────────────●　←targetの右上の座標
        // 　　　　　　　　　　　｜　　　　　　　　　　　　　　　｜
        // 　　　　　　　　　　　｜　　　　　　　　　　　　　　　｜
        // 　　　　　　　　　　　｜　　　　　　　　　　　　　　　｜
        // 　　　　　　　　　　　｜　　　　　　Square　　　　　　｜
        // 　　　　　　　　　　　｜　　　　　　　　　　　　　　　｜
        // 　　　　　　　　　　　｜　　　　　　　　　　　　　　　｜
        // 　　　　　　　　　　　｜　　　　　　　　　　　　　　　｜
        // targetの左下の座標→　●───────────────┘


        // 算出したtargetの法線ベクトル
        Vector2 targetNormal = default;

        // 上面（Hit座標は、targetの右上座標から見て[左上の方向] AND targetの左下座標から見て[右上の方向]）
        if (signOfHitPosFromTopRight == topLeft && signOfHitPosFromBottomLeft == topRight)
        {
            targetNormal = targetTransform.up;
        }
        // 右面
        else if (signOfHitPosFromTopRight == bottomRight && signOfHitPosFromBottomLeft == topRight)
        {
            targetNormal = targetTransform.right;
        }
        // 下面
        else if (signOfHitPosFromTopRight == bottomLeft && signOfHitPosFromBottomLeft == bottomRight)
        {
            targetNormal = -targetTransform.up;
        }
        // 左面
        else if (signOfHitPosFromTopRight == bottomLeft && signOfHitPosFromBottomLeft == topLeft)
        {
            targetNormal = -targetTransform.right;
        }
        // 右上角
        else if (signOfHitPosFromTopRight == topRight && signOfHitPosFromBottomLeft == topRight)
        {
            targetNormal = (targetTransform.up + targetTransform.right).normalized;
        }
        // 右下角
        else if (signOfHitPosFromTopRight == bottomRight && signOfHitPosFromBottomLeft == bottomRight)
        {
            targetNormal = (-targetTransform.up + targetTransform.right).normalized;
        }
        // 左下角
        else if (signOfHitPosFromTopRight == bottomLeft && signOfHitPosFromBottomLeft == bottomLeft)
        {
            targetNormal = (-targetTransform.up + -targetTransform.right).normalized;
        }
        // 左上角
        else if (signOfHitPosFromTopRight == topLeft && signOfHitPosFromBottomLeft == topLeft)
        {
            targetNormal = (targetTransform.up + -targetTransform.right).normalized;
        }

        return targetNormal;
    }


    /// <summary>
    /// 当たったオブジェクトの法線を取得する（当たったオブジェクトが単体のとき）
    /// </summary>
    public Vector2 GetNormal(Vector2 origin, Vector2 inDirection, BoxCollider2D targetCollider)
    {
        Transform targetTransform = targetCollider.transform;

        // targetのx軸、y軸それぞれの辺の半分のベクトルを算出
        // 回転を考慮するため、法線ベクトルを掛ける
        Vector2 targetHalfVector_x = targetCollider.size.x * targetTransform.localScale.x / 2 * targetTransform.right;
        Vector2 targetHalfVector_y = targetCollider.size.y * targetTransform.localScale.y / 2 * targetTransform.up;

        // targetの右上の座標
        Vector2 targetTopRightPos = (Vector2)targetTransform.position + targetHalfVector_x + targetHalfVector_y;
        // targetの左下の座標
        Vector2 targetBottomLeftPos = (Vector2)targetTransform.position - targetHalfVector_x - targetHalfVector_y;


        // 「targetの右上の座標」から見た、Hitした座標（左および下を-1、右および上を1と表現）
        (int x, int y) signOfHitPosFromTopRight;
        // 「targetの左下の座標」から見た、Hitした座標
        (int x, int y) signOfHitPosFromBottomLeft;

        // Hitした座標を、targetから見たローカル座標に変換
        Vector2 hitPosLocal = targetTransform.InverseTransformPoint(origin);
        // targetの右上の座標を、targetから見たローカル座標に変換
        Vector2 targetTopRightPosLocal = targetTransform.InverseTransformPoint(targetTopRightPos);
        // targetの左下の座標を、targetから見たローカル座標に変換
        Vector2 targetBottomLeftPosLocal = targetTransform.InverseTransformPoint(targetBottomLeftPos);


        // -1を左方向、1を右方向としてintで表現したローカル座標を算出
        // つまり右上または左下から見て、Hit座標はどの方向か
        signOfHitPosFromTopRight.x = (int)Mathf.Sign(hitPosLocal.x - targetTopRightPosLocal.x);
        signOfHitPosFromTopRight.y = (int)Mathf.Sign(hitPosLocal.y - targetTopRightPosLocal.y);

        signOfHitPosFromBottomLeft.x = (int)Mathf.Sign(hitPosLocal.x - targetBottomLeftPosLocal.x);
        signOfHitPosFromBottomLeft.y = (int)Mathf.Sign(hitPosLocal.y - targetBottomLeftPosLocal.y);


        // targetの右上の座標から見たHit座標の距離（角計算で使用）
        Vector2 distancePosFromTopRight;

        distancePosFromTopRight.x = Mathf.Abs(hitPosLocal.x - targetTopRightPosLocal.x);
        distancePosFromTopRight.y = Mathf.Abs(hitPosLocal.y - targetTopRightPosLocal.y);

        // targetの左下の座標から見たHit座標の距離（角計算で使用）
        Vector2 distancePosFromBottomLeft;

        distancePosFromBottomLeft.x = Mathf.Abs(hitPosLocal.x - targetBottomLeftPosLocal.x);
        distancePosFromBottomLeft.y = Mathf.Abs(hitPosLocal.y - targetBottomLeftPosLocal.y);



        // 　　　　　　　　　　　┌───────────────●　←targetの右上の座標
        // 　　　　　　　　　　　｜　　　　　　　　　　　　　　　｜
        // 　　　　　　　　　　　｜　　　　　　　　　　　　　　　｜
        // 　　　　　　　　　　　｜　　　　　　　　　　　　　　　｜
        // 　　　　　　　　　　　｜　　　　　　Square　　　　　　｜
        // 　　　　　　　　　　　｜　　　　　　　　　　　　　　　｜
        // 　　　　　　　　　　　｜　　　　　　　　　　　　　　　｜
        // 　　　　　　　　　　　｜　　　　　　　　　　　　　　　｜
        // targetの左下の座標→　●───────────────┘


        // 算出したtargetの法線ベクトル
        Vector2 targetNormal = default;

        // 上面（Hit座標は、targetの右上座標から見て[左上の方向] AND targetの左下座標から見て[右上の方向]）
        if (signOfHitPosFromTopRight == topLeft && signOfHitPosFromBottomLeft == topRight)
        {
            targetNormal = targetTransform.up;
        }
        // 右面
        else if (signOfHitPosFromTopRight == bottomRight && signOfHitPosFromBottomLeft == topRight)
        {
            targetNormal = targetTransform.right;
        }
        // 下面
        else if (signOfHitPosFromTopRight == bottomLeft && signOfHitPosFromBottomLeft == bottomRight)
        {
            targetNormal = -targetTransform.up;
        }
        // 左面
        else if (signOfHitPosFromTopRight == bottomLeft && signOfHitPosFromBottomLeft == topLeft)
        {
            targetNormal = -targetTransform.right;
        }
        // 右上角
        else if (signOfHitPosFromTopRight == topRight && signOfHitPosFromBottomLeft == topRight)
        {
            // 角に当たっているとき、2面のうち距離が近い方の面の法線を返す
            if (distancePosFromTopRight.x < distancePosFromTopRight.y)
            {
                targetNormal = targetTransform.up;
            }
            else
            {
                targetNormal = targetTransform.right;
            }
        }
        // 右下角
        else if (signOfHitPosFromTopRight == bottomRight && signOfHitPosFromBottomLeft == bottomRight)
        {
            if (distancePosFromTopRight.x < distancePosFromBottomLeft.y)
            {
                targetNormal = -targetTransform.up;
            }
            else
            {
                targetNormal = targetTransform.right;
            }
        }
        // 左下角
        else if (signOfHitPosFromTopRight == bottomLeft && signOfHitPosFromBottomLeft == bottomLeft)
        {
            if (distancePosFromBottomLeft.x < distancePosFromBottomLeft.y)
            {
                targetNormal = -targetTransform.up;
            }
            else
            {
                targetNormal = -targetTransform.right;
            }
        }
        // 左上角
        else if (signOfHitPosFromTopRight == topLeft && signOfHitPosFromBottomLeft == topLeft)
        {
            if (distancePosFromBottomLeft.x < distancePosFromTopRight.y)
            {
                targetNormal = targetTransform.up;
            }
            else
            {
                targetNormal = -targetTransform.right;
            }
        }

        // 入射角と法線ベクトルの角の大きさを算出
        float inDirAngleSize = Mathf.Abs(Vector2.Angle(inDirection, targetNormal));

        // 2つのベクトルの角度の大きさの絶対値が、0°以上90°以下のとき、重複呼び出しと判定し処理を終了
        if (inDirAngleSize >= 0 && inDirAngleSize <= 90)
        {
            // 入射ベクトルをそのまま返す
            return inDirection;
        }

        return targetNormal;
    }
}