using VContainer.Unity;
using System;
/// <summary>
/// ゲームの進行管理クラス
/// </summary>
public class GameProgressManagement : IGameProgressRegistrable, IGameProgressManagementAuxiliaryable, IStartable, IDisposable
{
    /// <summary>
    /// ゲーム開始時に発行する
    /// </summary>
    public event StartHandler OnStart;
    /// <summary>
    /// ゲーム終了時に発行する
    /// </summary>
    public event EndHandler OnFinish;
    /// <summary>
    /// ゲームリスタート時に発行する
    /// </summary>
    public event StartHandler OnRestart;

    private bool pauseState = false;
    /// <summary>
    /// 一時停止時に発行する
    /// </summary>
    public event PauseHandler OnPause;

    /// <summary>
    /// ゲーム開始
    /// </summary>
    public void Start()
    {
        OnStart?.Invoke(new StartEventArgs());
    }

    void IStartable.Start() { }
    /// <summary>
    /// ゲーム終了
    /// </summary>
    public void Finish()
    {
        OnFinish?.Invoke(new EndEventArgs());
    }
    /// <summary>
    /// ゲームリスタート
    /// </summary>
    public void Restart()
    {
        OnRestart?.Invoke(new StartEventArgs());
    }
    /// <summary>
    /// 一時停止
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
