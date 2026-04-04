using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.Collections;
using Unity.Jobs;

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

    [Tooltip("maximum amount of object that can be detected by AI")]
    [Range(100, 999)]public int MaxObjectDetected = 100;

    [Tooltip("amount of time it takes for the AI to forget the ally after not being detected")]
    public float AllyMemoryExpirationTime = 10f;
    Dictionary <CharCore, PlayerSummary> knownAllyList = new Dictionary<CharCore, PlayerSummary>();

    [Tooltip("amount of time it takes for the AI to forget the enemy after not being detected")]
    public float EnemyMemoryExpirationTime = 3f;
    Dictionary <CharCore, PlayerSummary> knownEnemyList = new Dictionary<CharCore, PlayerSummary>();

    PlayerSummary selfSummary = new PlayerSummary();

    GameObject focusPOI;

    KnownContext currentContext = new KnownContext();
    private JobHandle overlapHandle;
    private NativeArray<OverlapSphereCommand> commands;
    private NativeArray<ColliderHit> rangeCheck;
    private bool scanInProgress = false;

    void Start()
    {
        TeamMask = gameObject.layer;
        currentContext.Init(PlayerReference, knownAllyList, knownEnemyList, selfSummary);
    }

    void Update()
    {
        if (scanInProgress)
        {
            FinishScan();
        }
    }
    public KnownContext GetCurrentContext(){return currentContext;}

    public void RadiusScanAll()
    {
        //scan for all objects in ScanRads 
        commands = new NativeArray<OverlapSphereCommand>(1, Allocator.TempJob);
        rangeCheck = new NativeArray<ColliderHit>(MaxObjectDetected, Allocator.TempJob); //TODO: set size of array to amount of players present in scene

        commands[0] = new OverlapSphereCommand(transform.position, ScanRads, QueryParameters.Default);

        overlapHandle = OverlapSphereCommand.ScheduleBatch(commands, rangeCheck, 1, MaxObjectDetected);

        scanInProgress = true;

        
    }

    private void FinishScan()
    {
        if (!overlapHandle.IsCompleted)
        {
            //Debug.Log("Job not done yet — was actually async!");
            return;
        }
        overlapHandle.Complete();

        //put in list
        foreach (var obj in rangeCheck)
        {
            if (obj.collider == null)
            {
                continue;
            }
            //check if its player
            CharCore player = obj.collider.transform.GetComponentInParent<CharCore>();
            if (player != null)
            {
                //check if its self
                if (obj.collider.gameObject == gameObject)
                {
                    selfSummary.SetValues(player, player.AbilityManage, 
                        obj.collider.transform, transform, 999f);
                }
                //check if its teammate
                else if (player.gameObject.layer == TeamMask)
                {
                    if (!knownAllyList.TryGetValue(player, out PlayerSummary summary))
                    {
                        // Key didn't exist — create and add it
                        summary = new PlayerSummary();
                        knownAllyList.Add(player, summary);
                    }
                    summary.SetValues(player, player.AbilityManage, 
                        obj.collider.transform, transform, AllyMemoryExpirationTime);
                    
                        
                    //Debug.Log($"Added player summary: {knownAllyList[player].toString()}");
                    
                }
                //add as enemy otherwise
                else
                {
                    Vector3 vectorToTarget = obj.collider.transform.position - transform.position;
                    //TODO: change forward to vector from self to direction the AI is supposed to be aiming at in the future
                    //if within  view, then add to memory
                    if (Vector3.Angle(transform.forward, vectorToTarget.normalized) < SightAngle / 2)
                    {

                        if (!Physics.Raycast(transform.position + new Vector3(0, 1.6f, 0), vectorToTarget.normalized, vectorToTarget.magnitude, GroundMask))
                        {
                            if (!knownEnemyList.TryGetValue(player, out PlayerSummary summary))
                            {
                                // Key didn't exist — create and add it
                                summary = new PlayerSummary();
                                knownEnemyList.Add(player, summary);
                            }
                            summary.SetValues(player, player.AbilityManage, 
                                obj.collider.transform, transform, EnemyMemoryExpirationTime);
                        }
                    }

                }
                
            }
            //add other object types
            if (obj.collider.CompareTag("Point Of Interest"))
            {
                focusPOI = obj.collider.gameObject;
                //Debug.Log($"point of interest: {focusPOI}");
            }
        }
        currentContext.Init(PlayerReference, knownAllyList, knownEnemyList, selfSummary);
        currentContext.SetPOI(focusPOI);
        
        commands.Dispose();
        rangeCheck.Dispose();
        scanInProgress = false;
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

    void OnDisable()
    {
        if (scanInProgress)
        {
            overlapHandle.Complete();
            commands.Dispose();
            rangeCheck.Dispose();
        }
    }
}

    