/// <summary>
/// イベントに登録することができる
/// </summary>
public interface IGameProgressRegistrable
{
    /// <summary>
    /// ゲーム開始時に発行する
    /// </summary>
    event StartHandler OnStart;
    /// <summary>
    /// ゲーム終了時に発行する
    /// </summary>
    event EndHandler OnFinish;
    /// <summary>
    /// ゲームリスタート時に発行する
    /// </summary>
    event StartHandler OnRestart;
    /// <summary>
    /// 一時停止時に発行する
    /// </summary>
    event PauseHandler OnPause;
}
