using UnityEngine;

public interface IReflectable
{
    /// <summary>
    /// ���������I�u�W�F�N�g(Box)�ɔ��˂���
    /// </summary>
    /// <param name="inDirection">���ˊp</param>
    /// <param name="targetCollider">���������I�u�W�F�N�g��BoxCollider</param>
    /// <returns>���˂�����ɓ��삵�����ǂ���</returns>
    bool Reflect(Vector2 inDirection, BoxCollider2D targetCollider);
}
