public interface ITeamAllocationable
{
    TeamInfo TeamInfo { set; }
    GameModeInfo GameModeInfo { set; }
    TeamAction TeamAction { set; }

    void Build();
}
