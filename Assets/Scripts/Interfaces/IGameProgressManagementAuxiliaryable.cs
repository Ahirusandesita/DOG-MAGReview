/// <summary>
/// �Q�[���̐i�s�Ǘ���⏕���邱�Ƃ��ł���
/// </summary>
public interface IGameProgressManagementAuxiliaryable : IGameProgressRegistrable
{
    /// <summary>
    /// �Q�[���J�n
    /// </summary>
    void Start();
    /// <summary>
    /// �Q�[���I��
    /// </summary>
    void Finish();
    /// <summary>
    /// �Q�[�����X�^�[�g
    /// </summary>
    void Restart();
    /// <summary>
    /// �ꎞ��~
    /// </summary>
    void Pause();
}
