using UnityEngine;
using UnityEngine.InputSystem;

public class TargetPointScript : MonoBehaviour
{
    public Camera playerCam;
    public LayerMask clickLayers;

   

    public void SetTargetPointViaCam()
    {
        transform.position = playerCam.transform.position + (playerCam.transform.forward * 100);

        // RaycastHit hitInfo = new RaycastHit();
        // bool hit = Physics.Raycast(playerCam.ScreenPointToRay(Mouse.current.position.ReadValue()), out hitInfo, 100f, clickLayers);
        //     if (hit)
        //     {
        //         //send to where the camera is pointing to
        //         transform.position = hitInfo.point;
        //     }
        //     else {
        //         // send to long distance from camera
        //         transform.position = playerCam.transform.position + (playerCam.transform.forward * 100);
        //     }
    }
}
