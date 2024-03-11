using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameMode
{
    /// <summary>
    /// １ラウンド終了時に発行される
    /// </summary>
    event RoundHandler OnCompletedRound;
    event AysncWithRoundHandler OnAysncCompletedRound;
    /// <summary>
    /// １ラウンド終了
    /// </summary>
    /// <param name="teamInfo"></param>
    void CompletedRound(TeamInfo teamInfo);
    /// <summary>
    /// １ラウンド終了
    /// </summary>
    void CompletedRound(TeamInfo teamInfo, Round round);
}
