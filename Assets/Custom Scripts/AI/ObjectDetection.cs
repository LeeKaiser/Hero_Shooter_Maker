using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ObjectDetection : MonoBehaviour
{
    [Tooltip("time between each scan")]
    public float scanTimeInterval = 0.5f;
    [Tooltip("radius/distance at which AI can detect object")]
    public float scanRads = 30;
    [Tooltip("reference to self")]
    public GameObject playerRef;
    [Tooltip("mask for detecting objects")]
    public LayerMask groundMask, teamMask, enemyMask;
    [Tooltip("angle at which they detect certain objects")]
    [Range(0, 360)] public float sightAngle;

    [Tooltip("amount of time it takes for the AI to forget the ally after not being detected")]
    public float allyMemoryExpirationTime = 10f;
    Dictionary <PlayableCharCore, PlayerSummary> knownAllyList = new Dictionary<PlayableCharCore, PlayerSummary>();

    PlayerSummary selfSummary = new PlayerSummary();

    void Start()
    {
        teamMask = gameObject.layer;
        StartCoroutine(WaitThenScan());
    }

    void Update()
    {
        //RadiusScanAll();
    }

    IEnumerator WaitThenScan()
    {
        yield return new WaitForSeconds(scanTimeInterval);
        RadiusScanAll();
        ElapseExpirationTime(scanTimeInterval);
        StartCoroutine(WaitThenScan());
    }

    public void RadiusScanAll()
    {
        //scan for all objects in scanRads 
        Collider[] rangeCheck = Physics.OverlapSphere(transform.position, scanRads);
        //put in list
        List<PlayerSummary> alliesList = new List<PlayerSummary>();
        List<GameObject> enemiesList = new List<GameObject>();
        foreach (var obj in rangeCheck)
        {
            //check if its player
            PlayableCharCore player = obj.transform.GetComponentInParent<PlayableCharCore>();
            if (player != null)
            {
                //check if its self
                if (obj.gameObject == gameObject)
                {
                    selfSummary.SetValues(player, obj.transform.GetComponentInParent<AbilityManager>(), 
                        obj.transform, transform, 999f);
                }
                //check if its teammate
                else if (player.gameObject.layer == teamMask)
                {
                    if (!knownAllyList.ContainsKey(player))
                    {
                        //if not already in ally list
                        knownAllyList.Add(player, new PlayerSummary());
                        //Debug.Log($"added new player to memory {knownAllyList[player]}");
                    }
                    knownAllyList[player].SetValues(player, obj.transform.GetComponentInParent<AbilityManager>(), 
                        obj.transform, transform, allyMemoryExpirationTime);
                    
                        
                    //Debug.Log($"Added player summary: {knownAllyList[player].toString()}");
                    
                }
                //add as enemy otherwise
                else
                {
                    enemiesList.Add(player.gameObject);
                }
                
            }
            //add other object types
        
        }
        
    }

    //
    void ElapseExpirationTime(float timeElapsed)
    {
        List<PlayableCharCore> toRemove = new();
        foreach (KeyValuePair<PlayableCharCore, PlayerSummary> player in knownAllyList)
        {
            
            if (player.Value.timeUntilExpire <= 0)
            {
                toRemove.Add(player.Key);
                
            }
            else
            {
                player.Value.SubtractTimeRemaining(timeElapsed);
            }
            
        }

        foreach (var key in toRemove)
        {
            knownAllyList.Remove(key);
            
        }
    }

    public string toString()
    {
        string retStr = "";
        retStr += selfSummary.toString();

        foreach (KeyValuePair<PlayableCharCore, PlayerSummary> player in knownAllyList)
        {
            retStr += player.Value.toString();
        }

        return retStr;
    }
}
