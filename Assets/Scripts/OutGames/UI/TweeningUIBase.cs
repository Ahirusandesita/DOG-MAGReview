using UnityEngine;

public abstract class TweeningUIBase : MonoBehaviour
{
    /// <summary>
    /// 出現時に再生するアニメーション
    /// </summary>
    public abstract void OnStart();

    /// <summary>
    /// ループ再生するアニメーション
    /// </summary>
    public abstract void PlayLoop();

    /// <summary>
    /// 消去時に再生するアニメーション
    /// </summary>
    public abstract void OnErase();
}
