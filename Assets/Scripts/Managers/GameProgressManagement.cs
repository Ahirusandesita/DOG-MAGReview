using VContainer.Unity;
using System;
/// <summary>
/// �Q�[���̐i�s�Ǘ��N���X
/// </summary>
public class GameProgressManagement : IGameProgressRegistrable, IGameProgressManagementAuxiliaryable, IStartable, IDisposable
{
    /// <summary>
    /// �Q�[���J�n���ɔ��s����
    /// </summary>
    public event StartHandler OnStart;
    /// <summary>
    /// �Q�[���I�����ɔ��s����
    /// </summary>
    public event EndHandler OnFinish;
    /// <summary>
    /// �Q�[�����X�^�[�g���ɔ��s����
    /// </summary>
    public event StartHandler OnRestart;

    private bool pauseState = false;
    /// <summary>
    /// �ꎞ��~���ɔ��s����
    /// </summary>
    public event PauseHandler OnPause;

    /// <summary>
    /// �Q�[���J�n
    /// </summary>
    public void Start()
    {
        OnStart?.Invoke(new StartEventArgs());
    }

    void IStartable.Start() { }
    /// <summary>
    /// �Q�[���I��
    /// </summary>
    public void Finish()
    {
        OnFinish?.Invoke(new EndEventArgs());
    }
    /// <summary>
    /// �Q�[�����X�^�[�g
    /// </summary>
    public void Restart()
    {
        OnRestart?.Invoke(new StartEventArgs());
    }
    /// <summary>
    /// �ꎞ��~
    /// </summary>
    public void Pause()
    {
        PauseEventArgs pauseEventArgs = new PauseEventArgs();
        pauseEventArgs.isPause = !pauseState;
        pauseState = !pauseState;
        OnPause?.Invoke(pauseEventArgs);
    }

    public void Dispose()
    {

    }
}
