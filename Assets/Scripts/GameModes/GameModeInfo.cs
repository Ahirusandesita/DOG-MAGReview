
public class GameModeInfo
{
    /// <summary>
    /// 1スタート　2チームなら２
    /// </summary>
    private int numberOfTeam;
    public int NumberOfTeam
    {
        get
        {
            return numberOfTeam;
        }
    }
    public GameModeInfo(int numberOfTeam)
    {
        this.numberOfTeam = numberOfTeam;
    }
}
