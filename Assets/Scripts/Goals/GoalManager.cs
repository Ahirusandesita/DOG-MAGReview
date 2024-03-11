using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour, ITeamObjectManager
{
    [SerializeField]
    private List<GoalLifetimeScope> goals = new List<GoalLifetimeScope>();

    private ITeamInformation[] teamInformations;
    public ITeamInformation[] TeamInformations
    {
        set
        {
            teamInformations = value;
        }
        get
        {
            return goals.ToArray();
        }
    }
}
