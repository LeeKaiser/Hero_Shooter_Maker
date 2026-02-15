using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public struct KnownContext
{
    public GameObject playerRef;
    public Dictionary <PlayableCharCore, PlayerSummary> knownAllyList;
    public Dictionary <PlayableCharCore, PlayerSummary> knownEnemyList;
    public PlayerSummary selfSummary;

    public void Init(GameObject pr, Dictionary <PlayableCharCore, PlayerSummary> kal, Dictionary <PlayableCharCore, PlayerSummary> kel, PlayerSummary ss)
    {
        playerRef = pr;
        knownAllyList = kal;
        knownEnemyList = kel;
        selfSummary = ss;
    }

    public string toString()
    {
        string retStr = $"self: \n";
        retStr += selfSummary.toString();

        retStr += $"\n known allies: \n";

        foreach (KeyValuePair<PlayableCharCore, PlayerSummary> player in knownAllyList)
        {
            retStr += player.Value.toString();
        }

        retStr += $"\n known enemies: \n";
        foreach (KeyValuePair<PlayableCharCore, PlayerSummary> player in knownEnemyList)
        {
            retStr += player.Value.toString();
        }

        return retStr;
    }
}
