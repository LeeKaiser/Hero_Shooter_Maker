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
    public UIBar healthBar;
    [SerializeField] CinemachineVirtualCamera cinemachine;
    InputListener listener;
    StarterAssetsInputs starterAsset;

    void Start()
    {
        listener = GetComponent<InputListener>();
        GetComponent<ClientDamageNumber>().CharacterReference = CharacterReference;
        ConnectToPlayer();
    }

    void Update()
    {
        if (healthBar != null && CharacterReference != null)
        {
            healthBar.UpdateSlider(CharacterReference.GetHitPointsCurrent(), CharacterReference.GetHitPointsBase());
        }
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
