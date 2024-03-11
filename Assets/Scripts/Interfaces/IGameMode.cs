using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameMode
{
    /// <summary>
    /// �P���E���h�I�����ɔ��s�����
    /// </summary>
    event RoundHandler OnCompletedRound;
    event AysncWithRoundHandler OnAysncCompletedRound;
    /// <summary>
    /// �P���E���h�I��
    /// </summary>
    /// <param name="teamInfo"></param>
    void CompletedRound(TeamInfo teamInfo);
    /// <summary>
    /// �P���E���h�I��
    /// </summary>
    void CompletedRound(TeamInfo teamInfo, Round round);
}
