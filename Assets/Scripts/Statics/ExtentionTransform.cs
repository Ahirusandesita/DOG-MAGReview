using UnityEngine;

public static class ExtentionTransform
{
    /// <summary>
    /// 指定したベクトルの方向を向く
    /// </summary>
    public static void RotateInVectorDirection(this IRotationAdapter rotationAdapter, Vector2 targetDir)
    {
        if (targetDir == Vector2.zero)
        {
            return;
        }

        // 弧度法の基準角をy軸にするための補正値
        const float CORRECTION_TO_FRONT = 90f;

        // 指定した方向を向く
        Vector3 euler = new Vector3(0f, 0f, Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - CORRECTION_TO_FRONT);
        rotationAdapter.Rotation = Quaternion.Euler(euler);
    }

    /// <summary>
    /// 指定したベクトルの方向を向く
    /// </summary>
    public static void RotateInVectorDirection(this Transform transform, Vector2 targetDir)
    {
        if (targetDir == Vector2.zero)
        {
            return;
        }

        // 弧度法の基準角をy軸にするための補正値
        const float CORRECTION_TO_FRONT = 90f;

        // 指定した方向を向く
        Vector3 euler = new Vector3(0f, 0f, Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - CORRECTION_TO_FRONT);
        transform.rotation = Quaternion.Euler(euler);
    }
}
