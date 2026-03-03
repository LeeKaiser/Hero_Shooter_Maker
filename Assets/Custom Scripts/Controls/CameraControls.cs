using UnityEngine;
using StarterAssets;
using UnityEngine.InputSystem;

public class CameraControls : MonoBehaviour
{
    [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        [Tooltip("Control camera sensitivity for x axis")]
        public float CameraSensitivityX = 1.0f;
        [Tooltip("Control camera sensitivity for y axis")]
        public float CameraSensitivityY = 1.0f;
    [Header("TargetPoint")]
        [Tooltip("Transform that represents the place player is looking at")]        
        public Transform targetPoint;
        [Tooltip("Layers that count as ground")]        
        public LayerMask clickLayers;


    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    private StarterAssetsInputs _input;
    private GameObject _cameraGameObj;
    private Camera _mainCamera;
    private const float _threshold = 0.01f;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (_cameraGameObj == null)
            {
                _cameraGameObj = GameObject.FindGameObjectWithTag("MainCamera");
                _mainCamera = _cameraGameObj.GetComponent<Camera>();
            }
    }
    
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CameraRotation();
        SetTargetPointViaCam();
    }

    

    public void SetTargetPointViaCam()
    {
        //transform.position = playerCam.transform.position + (playerCam.transform.forward * 100);

        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), out hitInfo, 100f, clickLayers);
            if (hit)
            {
                //send to where the camera is pointing to
                targetPoint.position = hitInfo.point;
            }
            else {
                // send to long distance from camera
                targetPoint.position = _mainCamera.transform.position + (_mainCamera.transform.forward * 100);
            }
    }

    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = /*IsCurrentDeviceMouse ? 1.0f :*/ Time.deltaTime;
            _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier * CameraSensitivityX;//edit this to have sensitivity control
            _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier * CameraSensitivityY;
       }

       // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
       _cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
