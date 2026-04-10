using UnityEngine;
using Cinemachine;
using StarterAssets;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PlayerClient : MonoBehaviour
{
    public CharCore CharacterReference;
    public ClientUI PlayerCanvas;
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
        Transform cameraRoot = CharacterReference.PlayerArmature.transform.Find("PlayerCameraRoot");
        cinemachine.LookAt = cameraRoot;
        cinemachine.Follow = cameraRoot;
        listener.SetInputAction( CharacterReference.PlayerArmature.GetComponent<InputEventCaller>());
        starterAsset = CharacterReference.PlayerArmature.GetComponent<StarterAssetsInputs>();
        PlayerCanvas.characterReference = CharacterReference;
        PlayerCanvas.SetUpNewUI();
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
