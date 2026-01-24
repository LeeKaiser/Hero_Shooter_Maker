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
        
    }

    public void RadiusScanAll()
    {
        List<GameObject> alliesList = RadiusScanAllies();

    }

    public List<GameObject> RadiusScanAllies()
    {
        //scan for all objects in scanRads 
        Collider[] rangeCheck = Physics.OverlapSphere(transform.position, scanRads, teamMask);
        //put in list
        List<GameObject> alliesList = new List<GameObject>();
        foreach (var obj in rangeCheck)
        {
            PlayableCharCore player = obj.GetComponent<PlayableCharCore>();
            if (player != null)
            {
                //add to list
                alliesList.Add(player.gameObject);
            }
        }
        return alliesList;
    }
}
