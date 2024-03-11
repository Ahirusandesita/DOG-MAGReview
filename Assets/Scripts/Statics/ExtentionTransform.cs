using UnityEngine;

public static class ExtentionTransform
{
    /// <summary>
    /// �w�肵���x�N�g���̕���������
    /// </summary>
    public static void RotateInVectorDirection(this IRotationAdapter rotationAdapter, Vector2 targetDir)
    {
        if (targetDir == Vector2.zero)
        {
            return;
        }

        // �ʓx�@�̊�p��y���ɂ��邽�߂̕␳�l
        const float CORRECTION_TO_FRONT = 90f;

        // �w�肵������������
        Vector3 euler = new Vector3(0f, 0f, Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - CORRECTION_TO_FRONT);
        rotationAdapter.Rotation = Quaternion.Euler(euler);
    }

    /// <summary>
    /// �w�肵���x�N�g���̕���������
    /// </summary>
    public static void RotateInVectorDirection(this Transform transform, Vector2 targetDir)
    {
        if (targetDir == Vector2.zero)
        {
            return;
        }

        // �ʓx�@�̊�p��y���ɂ��邽�߂̕␳�l
        const float CORRECTION_TO_FRONT = 90f;

        // �w�肵������������
        Vector3 euler = new Vector3(0f, 0f, Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - CORRECTION_TO_FRONT);
        transform.rotation = Quaternion.Euler(euler);
    }
}
