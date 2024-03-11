using OutGameEnum;

public struct Round
{
    public int score;
    public TeamType teamType;

    public Round(int score, TeamType teamType)
    {
        this.score = score;
        this.teamType = teamType;
    }
    public Round(TeamInfo teamInfo)
    {
        this.score = teamInfo.TeamParamater.ScoreAddedAmount;
        this.teamType = teamInfo.TeamType;
    }
    public static Round operator +(Round left, Round right)
    {
        Round score = default;
        score.score = left.score + right.score;
        return score;
    }
}
