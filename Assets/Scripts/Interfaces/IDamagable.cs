using UnityEngine;

/// <summary>
/// 被弾可能なインターフェース
/// </summary>
public interface IDamagable
{
    /// <summary>
    /// ダメージを受ける処理
    /// </summary>
    /// <param name="teamInfo">呼び出し元のチーム情報</param>
    /// <param name="damage">受けるダメージ量</param>
    /// <param name="impactDir">接触したオブジェクトの進行ベクトル
    /// <br>- Normalizeすることが望ましい</br></param>
    void TakeDamage(TeamInfo teamInfo, int damage = 1, Vector2 impactDir = default);

    TeamInfo TeamInfo { get; }

    bool IsInvincible { get; }
}
