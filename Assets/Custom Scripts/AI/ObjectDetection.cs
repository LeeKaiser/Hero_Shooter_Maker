using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ObjectDetection : MonoBehaviour
{
    public float ScanTimeInterval = 0.5f;
    public float scanRads = 30;
    public GameObject playerRef;
    public LayerMask groundMask, teamMask, enemyMask;
    [Range(0, 360)] public float sightAngle;

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
        yield return new WaitForSeconds(ScanTimeInterval);
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
            PlayableCharCore player = obj.transform.parent.GetComponent<PlayableCharCore>();
            if (player != null && obj.gameObject != gameObject)
            {
                //check if its teammate
                if (player.gameObject.layer == teamMask)
                {
                    PlayerSummary summary = new PlayerSummary();
                    summary.SetValues(player, obj.GetComponent<AbilityManager>(), obj.transform, transform);
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
