using System.Collections.Generic;
using UnityEngine;
using OutGameEnum;
public interface ITeamInformation
{
    TeamInfo TeamInfo { get; set; }
    GameModeInfo GameModeInfo { get; set; }
    TeamAction TeamAction { get; set; }
    void Build();
}

public interface ITeamObjectManager
{
    ITeamInformation[] TeamInformations { get; set; }
}



public interface IPositionOnStage
{
    public IReadOnlyPositionAdapter this[TeamType teamType] { get; }
}
public class StageManager : MonoBehaviour, IPositionOnStage
{
    [SerializeField]
    private List<TeamObject> teamObjects = new List<TeamObject>();
    public ITeamObjectManager[] TeamObjectManagers
    {
        get
        {
            return this.GetComponents<ITeamObjectManager>();
        }
    }

    public IReadOnlyPositionAdapter this[TeamType teamType]
    {
        get
        {
            foreach (TeamObject teamObject in teamObjects)
            {
                if (teamObject.teamType == teamType)
                {
                    return teamObject.transformAdapter;
                }
            }
            //NullObject
            return new NullPositionAdapter();
        }
    }
}

[System.Serializable]
public class TeamObject
{
    public TeamType teamType;
    public TransformAdapter transformAdapter;
}

public class NullPositionAdapter : IReadOnlyPositionAdapter, IPositionAdapter
{
    public Vector3 Position
    {
        get
        {
            return Vector3.zero;
        }
        set
        {

        }
    }
}
