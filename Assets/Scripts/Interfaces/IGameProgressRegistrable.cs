/// <summary>
/// �C�x���g�ɓo�^���邱�Ƃ��ł���
/// </summary>
public interface IGameProgressRegistrable
{
    /// <summary>
    /// �Q�[���J�n���ɔ��s����
    /// </summary>
    event StartHandler OnStart;
    /// <summary>
    /// �Q�[���I�����ɔ��s����
    /// </summary>
    event EndHandler OnFinish;
    /// <summary>
    /// �Q�[�����X�^�[�g���ɔ��s����
    /// </summary>
    event StartHandler OnRestart;
    /// <summary>
    /// �ꎞ��~���ɔ��s����
    /// </summary>
    event PauseHandler OnPause;
}
