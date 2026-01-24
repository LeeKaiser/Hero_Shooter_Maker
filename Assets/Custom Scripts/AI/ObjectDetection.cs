using UnityEngine;
using System.Collections.Generic;

public class ObjectDetection : MonoBehaviour
{
    public float scanRads = 30;
    public GameObject playerRef;
    public LayerMask groundMask, teamMask, enemyMask;
    [Range(0, 360)] public float sightAngle;

    void Start()
    {
        teamMask = gameObject.layer;
    }

    void Update()
    {
        RadiusScanAll();
    }

    public void RadiusScanAll()
    {
        List<GameObject> alliesList = RadiusScanAllies();
        Debug.Log(alliesList.Count);
        
        
    }

    public List<GameObject> RadiusScanAllies()
    {
        //scan for all objects in scanRads 
        Collider[] rangeCheck = Physics.OverlapSphere(transform.position, scanRads, teamMask);
        //put in list
        List<GameObject> alliesList = new List<GameObject>();
        foreach (var obj in rangeCheck)
        {
            PlayableCharCore player = obj.transform.parent.GetComponent<PlayableCharCore>();
            if (player != null && obj.gameObject != gameObject)
            {
                //add the to list
                alliesList.Add(player.gameObject);
                
            }
        }
        return alliesList;
    }
}
