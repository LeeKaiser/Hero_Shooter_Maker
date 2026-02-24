using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ObjectDetection : MonoBehaviour
{
    
    
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

    [Tooltip("amount of time it takes for the AI to forget the enemy after not being detected")]
    public float enemyMemoryExpirationTime = 3f;
    Dictionary <PlayableCharCore, PlayerSummary> knownEnemyList = new Dictionary<PlayableCharCore, PlayerSummary>();

    PlayerSummary selfSummary = new PlayerSummary();

    GameObject focusPOI;

    KnownContext currentContext = new KnownContext();

    void Start()
    {
        teamMask = gameObject.layer;
        
    }

    void Update()
    {
        //RadiusScanAll();
    }

    

    public KnownContext GetCurrentContext(){return currentContext;}

    public void RadiusScanAll()
    {
        //scan for all objects in scanRads 
        Collider[] rangeCheck = Physics.OverlapSphere(transform.position, scanRads);
        //put in list
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
                    Vector3 vectorToTarget = obj.transform.position - transform.position;
                    //TODO: change forward to vector from self to direction the AI is looking at in the future
                    //if within  view, then add to memory
                    if (Vector3.Angle(transform.forward, vectorToTarget.normalized) < sightAngle / 2)
                    {
                        if (!Physics.Raycast(transform.position + new Vector3(0, 1.6f, 0), vectorToTarget.normalized, vectorToTarget.magnitude, groundMask))
                        {
                            if (!knownEnemyList.ContainsKey(player))
                            {
                                //if not already in enemy list
                                knownEnemyList.Add(player, new PlayerSummary());
                                //Debug.Log($"added new player to memory {knownEnemyList[player]}");
                            }
                            knownEnemyList[player].SetValues(player, obj.transform.GetComponentInParent<AbilityManager>(), 
                                obj.transform, transform, enemyMemoryExpirationTime);
                        }
                        
                    }

                }
                
            }
            //add other object types
            if (obj.CompareTag("Point Of Interest"))
            {
                focusPOI = obj.gameObject;
                //Debug.Log($"point of interest: {focusPOI}");
            }
        }
        
        currentContext.Init(playerRef, knownAllyList, knownEnemyList, selfSummary);
        currentContext.SetPOI(focusPOI);
        
    }

    //
    public void ElapseExpirationTime(float timeElapsed)
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

        toRemove = new();

        foreach (KeyValuePair<PlayableCharCore, PlayerSummary> player in knownEnemyList)
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
            knownEnemyList.Remove(key);
            
        }
    }

    public string toString()
    {
        return currentContext.toString();
    }
}
