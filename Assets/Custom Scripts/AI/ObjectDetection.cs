using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ObjectDetection : MonoBehaviour
{
    public float scanTimeInterval = 0.5f;
    public float scanRads = 30;
    public GameObject playerRef;
    public LayerMask groundMask, teamMask, enemyMask;
    [Range(0, 360)] public float sightAngle;

    public float allyMemoryTime = 10f;
    Dictionary <PlayableCharCore, PlayerSummary> knownAllyList;

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
            if (player != null && obj.gameObject != gameObject)
            {
                //check if its teammate
                if (player.gameObject.layer == teamMask)
                {
                    PlayerSummary summary = new PlayerSummary();
                    summary.SetValues(player, obj.transform.GetComponentInParent<AbilityManager>(), obj.transform, transform);
                    alliesList.Add(summary);
                    Debug.Log($"Added player summary: {summary.toString()}");
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
}
