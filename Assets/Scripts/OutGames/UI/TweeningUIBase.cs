using UnityEngine;

public abstract class TweeningUIBase : MonoBehaviour
{
    /// <summary>
    /// �o�����ɍĐ�����A�j���[�V����
    /// </summary>
    public abstract void OnStart();

    /// <summary>
    /// ���[�v�Đ�����A�j���[�V����
    /// </summary>
    public abstract void PlayLoop();

    /// <summary>
    /// �������ɍĐ�����A�j���[�V����
    /// </summary>
    public abstract void OnErase();
}
