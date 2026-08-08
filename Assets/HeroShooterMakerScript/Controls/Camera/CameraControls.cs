using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
 
/*
CameraControls
Drives the Cinemachine follow target's orientation from look input, and
projects a world-space aim point from wherever the camera is pointed.
*/
public class CameraControls : MonoBehaviour
{
    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;
 
    [Tooltip("How far in degrees can you move the camera up")]
    [FormerlySerializedAs("TopClamp")]
    public float pitchUpperLimit = 70.0f;
 
    [Tooltip("How far in degrees can you move the camera down")]
    [FormerlySerializedAs("BottomClamp")]
    public float pitchLowerLimit = -30.0f;
 
    [Tooltip("Additional degrees to override the camera. Useful for fine tuning camera position when locked")]
    [FormerlySerializedAs("CameraAngleOverride")]
    public float pitchOverride = 0.0f;
 
    [Tooltip("For locking the camera position on all axis")]
    [FormerlySerializedAs("LockCameraPosition")]
    public bool cameraLocked = false;
 
    [Tooltip("Control camera sensitivity for x axis")]
    public float CameraSensitivityX = 1.0f;
    [Tooltip("Control camera sensitivity for y axis")]
    public float CameraSensitivityY = 1.0f;
 
    [Header("AimTarget")]
    [Tooltip("Transform that represents the place player is looking at")]
    public Transform AimTarget;
    [Tooltip("Layers that count as ground")]
    public LayerMask ClickLayers;
 
    [Tooltip("Distance to project the aim target when nothing is hit")]
    public float FallbackAimDistance = 100f;
 
    private Vector2 _lookAngles; // x = yaw, y = pitch
    private InputConverter _input;
    private Camera _mainCamera;
    private const float LookDeadzone = 0.01f;
 
    private void Awake()
    {
        if (_mainCamera == null)
        {
            GameObject camObj = GameObject.FindGameObjectWithTag("MainCamera");
            _mainCamera = camObj.GetComponent<Camera>();
        }
        Cursor.lockState = CursorLockMode.Locked;
    }
 
    private void Start()
    {
        _input = GetComponent<InputConverter>();
        _lookAngles.x = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
    }
 
    private void LateUpdate()
    {
        ApplyLookRotation();
        ProjectAimTarget();
    }
 
    public void SetAimTargetViaCam()
    {
        ProjectAimTarget();
    }
 
    private void ProjectAimTarget()
    {
        Ray cursorRay = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
 
        if (Physics.Raycast(cursorRay, out RaycastHit hitInfo, FallbackAimDistance, ClickLayers))
        {
            AimTarget.position = hitInfo.point;
        }
        else
        {
            AimTarget.position = _mainCamera.transform.position + _mainCamera.transform.forward * FallbackAimDistance;
        }
    }
 
    private void ApplyLookRotation()
    {
        if (!cameraLocked && _input.look.sqrMagnitude >= LookDeadzone)
        {
            float dt = Time.deltaTime;
            _lookAngles.x += _input.look.x * dt * CameraSensitivityX;
            _lookAngles.y += _input.look.y * dt * CameraSensitivityY;
        }
 
        _lookAngles.x = WrapDegrees(_lookAngles.x);
        _lookAngles.y = Mathf.Clamp(WrapDegrees(_lookAngles.y), pitchLowerLimit, pitchUpperLimit);
 
        CinemachineCameraTarget.transform.rotation =
            Quaternion.Euler(_lookAngles.y + pitchOverride, _lookAngles.x, 0.0f);
    }
 
    private static float WrapDegrees(float degrees)
    {
        if (degrees > 360f) return degrees - 360f;
        if (degrees < -360f) return degrees + 360f;
        return degrees;
    }
}
