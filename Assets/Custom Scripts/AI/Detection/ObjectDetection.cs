using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/*
ObjectDetection
Produces a list of information based on what is nearby the character. 
*/
public class ObjectDetection : MonoBehaviour
{
    [Tooltip("radius/distance at which AI can detect object")]
    public float ScanRads = 30;
    [Tooltip("reference to self")]
    public GameObject PlayerReference;
    [Tooltip("mask for detecting objects")]
    public LayerMask GroundMask, TeamMask;
    [Tooltip("angle at which they detect certain objects")]
    [Range(0, 360)] public float SightAngle;

    [Tooltip("amount of time it takes for the AI to forget the ally after not being detected")]
    public float AllyMemoryExpirationTime = 10f;
    Dictionary <CharCore, PlayerSummary> knownAllyList = new Dictionary<CharCore, PlayerSummary>();

    [Tooltip("amount of time it takes for the AI to forget the enemy after not being detected")]
    public float EnemyMemoryExpirationTime = 3f;
    Dictionary <CharCore, PlayerSummary> knownEnemyList = new Dictionary<CharCore, PlayerSummary>();

    PlayerSummary selfSummary = new PlayerSummary();

    GameObject focusPOI;

    KnownContext currentContext = new KnownContext();

    void Start()
    {
        TeamMask = gameObject.layer;
        currentContext.Init(PlayerReference, knownAllyList, knownEnemyList, selfSummary);
    }

    public KnownContext GetCurrentContext(){return currentContext;}

    public void RadiusScanAll()
    {
        //scan for all objects in ScanRads 
        Collider[] rangeCheck = Physics.OverlapSphere(transform.position, ScanRads);
        //put in list
        foreach (var obj in rangeCheck)
        {
            //check if its player
            CharCore player = obj.transform.GetComponentInParent<CharCore>();
            if (player != null)
            {
                //check if its self
                if (obj.gameObject == gameObject)
                {
                    selfSummary.SetValues(player, obj.transform.GetComponentInParent<AbilityManager>(), 
                        obj.transform, transform, 999f);
                }
                //check if its teammate
                else if (player.gameObject.layer == TeamMask)
                {
                    if (!knownAllyList.ContainsKey(player))
                    {
                        //if not already in ally list
                        knownAllyList.Add(player, new PlayerSummary());
                        //Debug.Log($"added new player to memory {knownAllyList[player]}");
                    }
                    knownAllyList[player].SetValues(player, obj.transform.GetComponentInParent<AbilityManager>(), 
                        obj.transform, transform, AllyMemoryExpirationTime);
                    
                        
                    //Debug.Log($"Added player summary: {knownAllyList[player].toString()}");
                    
                }
                //add as enemy otherwise
                else
                {
                    Vector3 vectorToTarget = obj.transform.position - transform.position;
                    //TODO: change forward to vector from self to direction the AI is looking at in the future
                    //if within  view, then add to memory
                    if (Vector3.Angle(transform.forward, vectorToTarget.normalized) < SightAngle / 2)
                    {
                        if (!Physics.Raycast(transform.position + new Vector3(0, 1.6f, 0), vectorToTarget.normalized, vectorToTarget.magnitude, GroundMask))
                        {
                            if (!knownEnemyList.ContainsKey(player))
                            {
                                //if not already in enemy list
                                knownEnemyList.Add(player, new PlayerSummary());
                                //Debug.Log($"added new player to memory {knownEnemyList[player]}");
                            }
                            knownEnemyList[player].SetValues(player, obj.transform.GetComponentInParent<AbilityManager>(), 
                                obj.transform, transform, EnemyMemoryExpirationTime);
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
        currentContext.Init(PlayerReference, knownAllyList, knownEnemyList, selfSummary);
        currentContext.SetPOI(focusPOI);
    }

    //
    public void ElapseExpirationTime(float timeElapsed)
    {
        List<CharCore> toRemove = new();
        foreach (KeyValuePair<CharCore, PlayerSummary> player in knownAllyList)
        {
            if (player.Value.TimeUntilExpire <= 0)
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
        foreach (KeyValuePair<CharCore, PlayerSummary> player in knownEnemyList)
        {
            if (player.Value.TimeUntilExpire <= 0)
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
