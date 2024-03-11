using UnityEngine;
using OutGameEnum;

public class TeamInfo
{
    public TeamType TeamType { get; private set; }
    public TeamColorAsset TeamColor { get; private set; }
    public GameParamater TeamParamater { get; private set; }


    public TeamInfo(TeamType teamType, TeamColorAsset teamColor)
    {
        TeamType = teamType;
        TeamColor = teamColor;
        TeamParamater = new GameParamater();
    }

    public TeamInfo(TeamType teamType,TeamColorAsset teamColor, GameParamater teamParamater)
    {
        TeamType = teamType;
        TeamParamater = teamParamater;
        TeamColor = teamColor;
    }
}
public class NullTeamInfo : TeamInfo
{
    public NullTeamInfo() : base(TeamType.Null, ScriptableObject.CreateInstance<TeamColorAsset>(), new GameParamater()) { }
}

public class GameParamater
{
    public int InitialScore { get; private set; }
    public int ScoreAddedAmount { get; private set; }


    public GameParamater()
    {
        InitialScore = 0;
        ScoreAddedAmount = 1;
    }

    public GameParamater(int initialScore, int scoreAddedAmount)
    {
        InitialScore = initialScore;
        ScoreAddedAmount = scoreAddedAmount;
    }
}

public class NullGameParamater : GameParamater
{
    public NullGameParamater() : base(0, 0) { }
}
