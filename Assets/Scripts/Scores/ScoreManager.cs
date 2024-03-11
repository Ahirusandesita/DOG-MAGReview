using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour, ITeamObjectManager
{
    [SerializeField]
    private List<ScoreLifetimeScope> scoreLifetimeScope = new List<ScoreLifetimeScope>();
    public ITeamInformation[] TeamInformations
    {
        get
        {
            return scoreLifetimeScope.ToArray();
        }
        set
        {

        }
    }
}
