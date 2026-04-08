using UnityEngine;
using Cinemachine;
using StarterAssets;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PlayerClient : MonoBehaviour
{
    public CharCore PlayerReference;
    public Transform PlayerCanvas;
    [SerializeField] CinemachineVirtualCamera cinemachine;
    InputListener listener;
    StarterAssetsInputs starterAsset;

    //set cinemachine follow target to player's cinemachine root
    //set event caller in input listener to player's event caller
    //set camera controls starter asset input
    //set movement controls starter asset input
    void Start()
    {
        listener = GetComponent<InputListener>();
        ConnectToPlayer();
    }

    public void ConnectToPlayer()
    {
        cinemachine.LookAt = PlayerReference.PlayerArmature.transform.Find("PlayerCameraRoot");
        listener.SetInputAction( PlayerReference.PlayerArmature.GetComponent<InputEventCaller>());
        starterAsset = PlayerReference.PlayerArmature.GetComponent<StarterAssetsInputs>();
        PlayerReference.GetComponent<AbilityManager>().PlayerCanvas = PlayerCanvas;
    }

    #if ENABLE_INPUT_SYSTEM
    public void OnMove(InputValue value)
    {
        starterAsset.MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        if(starterAsset.cursorInputForLook)
        {
            starterAsset.LookInput(value.Get<Vector2>());
        }
    }

    public void OnJump(InputValue value)
    {
        starterAsset.JumpInput(value.isPressed);
    }
    #endif
}
