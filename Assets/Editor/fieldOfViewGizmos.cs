using UnityEditor;
using UnityEngine;

namespace StarterAssets
{
[CustomEditor(typeof(ObjectDetection)), CanEditMultipleObjects]
public class fieldOfVIewGizmos : Editor
{
    private void OnSceneGUI()
    {
        ObjectDetection fOV = (ObjectDetection)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fOV.transform.position, Vector3.up, Vector3.forward, 360, fOV.scanRads);

        Vector3 viewAngle1 = DistanceFromAngle(fOV.transform.eulerAngles.y, -fOV.sightAngle / 2);
        Vector3 viewAngle2 = DistanceFromAngle(fOV.transform.eulerAngles.y, fOV.sightAngle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fOV.transform.position, fOV.transform.position + viewAngle1 * fOV.scanRads);
        Handles.DrawLine(fOV.transform.position, fOV.transform.position + viewAngle2 * fOV.scanRads);

        
    }

    private Vector3 DistanceFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
}