using UnityEngine;
using StarterAssets;
using UnityEngine.InputSystem;

/*
Camera Controls
controls camera and target point
active only for the player
code based on the Starter Assets package
*/
public class CameraControls : MonoBehaviour
{
    //variable - public
    [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject cinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float topClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float bottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float cameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool lockCameraPosition = false;

        [Tooltip("Control camera sensitivity for x axis")]
        public float cameraSensitivityX = 1.0f;
        [Tooltip("Control camera sensitivity for y axis")]
        public float cameraSensitivityY = 1.0f;
    [Header("TargetPoint")]
        [Tooltip("Transform that represents the place player is looking at")]        
        public Transform targetPoint;
        [Tooltip("Layers that count as ground")]        
        public LayerMask clickLayers;

    //variable - private
    // cinemachine yaw rotation
    private float cinemachineTargetYaw;
    // cinemachine pitch rotation
    private float cinemachineTargetPitch;
    // movement input (for camera's movement)
    private StarterAssetsInputs input;
    //reference to camera's game object
    private GameObject cameraGameObj;
    //reference to camera component
    private Camera mainCamera;
    private const float threshold = 0.01f;
    

    //initialize camera related references
    void Awake()
    {
        if (cameraGameObj == null)
            {
                cameraGameObj = GameObject.FindGameObjectWithTag("MainCamera");
                mainCamera = cameraGameObj.GetComponent<Camera>();
            }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //initialize input and cinemachine target yaw
        input = GetComponent<StarterAssetsInputs>();
        cinemachineTargetYaw = cinemachineCameraTarget.transform.rotation.eulerAngles.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CameraRotation();
        SetTargetPointViaCam();
    }

    
    //sets target point (abilities are aimed at it)
    public void SetTargetPointViaCam()
    {

        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), out hitInfo, 100f, clickLayers);
            if (hit)
            {
                //send to where the camera is pointing to
                targetPoint.position = hitInfo.point;
            }
            else {
                // send to long distance from camera
                targetPoint.position = mainCamera.transform.position + (mainCamera.transform.forward * 100);
            }
    }

    //rotate camera based on input
    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (input.look.sqrMagnitude >= threshold && !lockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = /*IsCurrentDeviceMouse ? 1.0f :*/ Time.deltaTime;
            cinemachineTargetYaw += input.look.x * deltaTimeMultiplier * cameraSensitivityX;//edit this to have sensitivity control
            cinemachineTargetPitch += input.look.y * deltaTimeMultiplier * cameraSensitivityY;
       }

       // clamp our rotations so our values are limited 360 degrees
        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

        // Cinemachine will follow this target
        cinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + cameraAngleOverride,
       cinemachineTargetYaw, 0.0f);
    }
    //method used to clamp angle within 360 degrees
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
