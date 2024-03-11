/// <summary>
/// ゲームの進行管理を補助することができる
/// </summary>
public interface IGameProgressManagementAuxiliaryable : IGameProgressRegistrable
{
    /// <summary>
    /// ゲーム開始
    /// </summary>
    void Start();
    /// <summary>
    /// ゲーム終了
    /// </summary>
    void Finish();
    /// <summary>
    /// ゲームリスタート
    /// </summary>
    void Restart();
    /// <summary>
    /// 一時停止
    /// </summary>
    void Pause();
}
