using UnityEngine;
using Unity.Cinemachine;
using HeroShooterMaker.AI;
using HeroShooterMaker.Controls;
using HeroShooterMaker.Character;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using HeroShooterMakerDemo; //supports UI systems specific to the demo

namespace HeroShooterMaker.Client
{
    public class PlayerClient : MonoBehaviour
    {
        public CharCore CharacterReference;
        public ClientUI PlayerCanvas;
        public UIBar healthBar;
        [SerializeField] CinemachineCamera cinemachine;
        InputListener listener;

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

        void GeneratePlayer()
        {

        }

        public void ConnectToPlayer()
        {
            Transform cameraRoot = CharacterReference.PlayerArmature.transform.Find("PlayerCameraRoot");
            cinemachine.LookAt = cameraRoot;
            cinemachine.Follow = cameraRoot;
            listener.SetInputConverter(CharacterReference.PlayerArmature.GetComponent<InputConverter>());
            PlayerCanvas.characterReference = CharacterReference;
            PlayerCanvas.SetUpNewUI();
            CharacterReference.PlayerArmature.GetComponent<AIProcess>().enabled = false;
            CharacterReference.PlayerArmature.GetComponent<ObjectDetection>().enabled = false;
            CharacterReference.PlayerArmature.GetComponent<AIMovement>().enabled = false;
        }


    }
}