using UnityEngine;

public interface IReflectable
{
    /// <summary>
    /// 当たったオブジェクト(Box)に反射する
    /// </summary>
    /// <param name="inDirection">入射角</param>
    /// <param name="targetCollider">当たったオブジェクトのBoxCollider</param>
    /// <returns>反射が正常に動作したかどうか</returns>
    bool Reflect(Vector2 inDirection, BoxCollider2D targetCollider);
}
