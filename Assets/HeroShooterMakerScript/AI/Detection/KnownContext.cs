using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/*
Known Context
struct which contains context at a certain time
*/
public struct KnownContext
{
    public GameObject PlayerReference;
    public Dictionary <CharCore, PlayerSummary> KnownAllyList;
    public Dictionary <CharCore, PlayerSummary> KnownEnemyList;
    public PlayerSummary SelfSummary;
    public List<PatrolLandmark> focusPOIList;

    public void Init(GameObject pr, Dictionary <CharCore, PlayerSummary> kal, Dictionary <CharCore, PlayerSummary> kel, PlayerSummary ss)
    {
        PlayerReference = pr;
        KnownAllyList = kal;
        KnownEnemyList = kel;
        SelfSummary = ss;
    }

    public void SetPOI(List<PatrolLandmark> poi){focusPOIList = poi;}

    public string toString()
    {
        string retStr = $"self: \n";
        retStr += SelfSummary.toString();

        retStr += $"\n known allies: \n";

        foreach (KeyValuePair<CharCore, PlayerSummary> player in KnownAllyList)
        {
            retStr += player.Value.toString();
        }

        retStr += $"\n known enemies: \n";
        foreach (KeyValuePair<CharCore, PlayerSummary> player in KnownEnemyList)
        {
            retStr += player.Value.toString();
        }

        retStr += $"point of interest: {focusPOIList}";

        return retStr;
    }
}
