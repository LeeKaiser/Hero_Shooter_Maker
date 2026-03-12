using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/*
known context
snapshot of information found in object detection
*/
public struct KnownContext
{
    //variable - public
    //reference to the player that the context belongs to
    public GameObject playerRef;
    //dictionary of known allies
    public Dictionary <PlayableCharCore, PlayerSummary> knownAllyList;
    //dictionary of known enemies
    public Dictionary <PlayableCharCore, PlayerSummary> knownEnemyList;
    //summary of player it belongs to (playerRef)
    public PlayerSummary selfSummary;
    //point of interest that the AI cares about
    public GameObject focusPOI;

    //method
    //initialize all of its information
    public void Init(GameObject pr, Dictionary <PlayableCharCore, PlayerSummary> kal, Dictionary <PlayableCharCore, PlayerSummary> kel, PlayerSummary ss)
    {
        playerRef = pr;
        knownAllyList = kal;
        knownEnemyList = kel;
        selfSummary = ss;
    }

    //set its point of interest
    public void SetPOI(GameObject poi){focusPOI = poi;}

    //return string version
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

        retStr += $"point of interest: {focusPOI}";

        return retStr;
    }
}
