 using UnityEngine;
#if ENABLEINPUTSYSTEM 
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

/*
Third person controller
controls movement
code based on the Starter Assets package
*/
namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLEINPUTSYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]

        [Tooltip("controller active")]
        public bool isActive = true;

        [Tooltip("Move speed of the character in unit/s")]
        public float forwardMoveSpeed = 2.0f;
        public float strafeMoveSpeed = 2.0f;
        public float backwardMoveSpeed = 2.0f;

        [Tooltip("if the character will turn to face movement direction or not")]
        public MovementStyles.MovementStyle movementStyle;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float rotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float speedChangeRate = 10.0f;

        public AudioClip landingAudioClip;
        public AudioClip[] footstepAudioClips;
        [Range(0, 1)] public float footstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float jumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float jumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float fallTimeout = 0.15f;

        [Tooltip("Time required for the grounded status to change")]
        public float jumpForgivenessTime = 0.3f;

        [Header("Player grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool grounded = true;
        public bool canJump = true;
        public bool recentlyJumped = false;

        [Tooltip("amount of times the character can jump in the air")]
        public int airJumps = 0;

        [Tooltip("Useful for rough ground")]
        public float groundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float groundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask groundLayers;

        [Tooltip("Transform that represents the place player is looking at")]        
        public Transform targetPoint;

        

        // player
        private float speed;
        private float animationBlend;
        private float targetRotation = 0.0f;
        private float rotationVelocity;
        private float verticalVelocity;
        private float terminalVelocity = 53.0f;

        // timeout deltatime
        private float jumpTimeoutDelta;
        private float fallTimeoutDelta;

        // animation IDs
        private int animIDSpeed;
        private int animIDgrounded;
        private int animIDJump;
        private int animIDFreeFall;
        private int animIDMotionSpeed;

#if ENABLEINPUTSYSTEM 
        private PlayerInput playerInput;
#endif
        private Animator animator;
        private CharacterController controller;
        private StarterAssetsInputs input;
        

        

        private bool hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLEINPUTSYSTEM
                return playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }


        private void Awake()
        {
            // get a reference to our main camera
            
        }

        private void Start()
        {   
            hasAnimator = TryGetComponent(out animator);
            controller = GetComponent<CharacterController>();
            input = GetComponent<StarterAssetsInputs>();
#if ENABLEINPUTSYSTEM 
            playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            jumpTimeoutDelta = jumpTimeout;
            fallTimeoutDelta = fallTimeout;
        }

        private void Update()
        {
            if (isActive){
                hasAnimator = TryGetComponent(out animator);

                JumpAndGravity();
                groundedCheck();
                Move();
            }
            
        }

        private void AssignAnimationIDs()
        {
            animIDSpeed = Animator.StringToHash("Speed");
            animIDgrounded = Animator.StringToHash("grounded");
            animIDJump = Animator.StringToHash("Jump");
            animIDFreeFall = Animator.StringToHash("FreeFall");
            animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void groundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset,
                transform.position.z);
            grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers,
                QueryTriggerInteraction.Ignore);
            if (!grounded)
                {
                    //do this again cayote time later
                    Invoke("SecondGroundCheck", jumpForgivenessTime);
                }
            else{
                canJump = true;
            }

            // update animator if using character
            if (hasAnimator)
            {
                animator.SetBool(animIDgrounded, grounded);
            }
        }

        private void SecondGroundCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset,
                transform.position.z);
            canJump = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers,
                QueryTriggerInteraction.Ignore);
        }

        

        private void Move()
        {
            Vector2 moveInput = input.move.normalized;
            Vector3 inputDirection = new Vector3(input.move.x, 0f, input.move.y).normalized;

            float baseSpeed = DetermineSpeed(moveInput);

            float inputMagnitude = input.analogMovement ? input.move.magnitude : 1f;

            // Scale final speed using normalized direction vector
            float targetSpeed = baseSpeed * inputMagnitude;
            if (input.move == Vector2.zero) targetSpeed = 0f;


            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

            float speedOffset = 0.1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * speedChangeRate);

                // round speed to 3 decimal places
                speed = Mathf.Round(speed * 1000f) / 1000f;
            }
            else
            {
                speed = targetSpeed;
            }

            animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
            if (animationBlend < 0.01f) animationBlend = 0f;

            PlayerFaceTargetPoint(inputDirection);

            Vector3 targetDirection = GetTargetDirection();

            // move the player
                controller.Move(targetDirection.normalized * speed * Time.deltaTime +
                    new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
            

            // update animator if using character
            if (hasAnimator)
            {
                animator.SetFloat(animIDSpeed, animationBlend);
                animator.SetFloat(animIDMotionSpeed, inputMagnitude);
            }
        }

        private float DetermineSpeed(Vector2 moveInput)
        {
            switch(movementStyle)
            {
                case MovementStyles.MovementStyle.AlwaysFaceForward:
                    // Determine which axis dominates — this is raw input based, so consistent
                    if (Mathf.Abs(moveInput.y) >= Mathf.Abs(moveInput.x))
                    {
                        return moveInput.y >= 0 ? forwardMoveSpeed : backwardMoveSpeed;
                        
                    }
                    else
                    {
                        return strafeMoveSpeed;
                    }
                case MovementStyles.MovementStyle.RotateInsteadOfStrafe:
                    return moveInput.y >= 0 ? forwardMoveSpeed : backwardMoveSpeed;
                default:
                    return forwardMoveSpeed;
            }
        }

        private void PlayerFaceTargetPoint(Vector3 inputDirection)
        {
            //vector of player facing the targetpoint
            Vector3 playerFaceTarget = targetPoint.position - transform.position;
            //playerFaceTarget.y = 0;
            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            targetRotation = Mathf.Atan2(playerFaceTarget.x, playerFaceTarget.z) * Mathf.Rad2Deg;

            switch(movementStyle)
            {
                case MovementStyles.MovementStyle.FaceMovement:
                    targetRotation += Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
                    break;
                case MovementStyles.MovementStyle.RotateInsteadOfStrafe:
                    targetRotation = strafeMoveSpeed * rotationSmoothTime * Mathf.Atan2(inputDirection.x, 0) * Mathf.Rad2Deg + transform.eulerAngles.y;
                    break;
                default:
                    break;
            }

            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                    rotationSmoothTime);

            // rotate
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

         private Vector3 GetTargetDirection()
        {
            switch(movementStyle)
            {
                case MovementStyles.MovementStyle.AlwaysFaceForward:
                    return transform.forward * input.move.y + transform.right * input.move.x;
                case MovementStyles.MovementStyle.RotateInsteadOfStrafe:
                    return input.move.y >= 0 ? transform.forward : -transform.forward;
                default:
                    return transform.forward;
            }
        } 

        private void JumpAndGravity()
        {
            if (canJump)
            {
                // update animator if using character
                if (hasAnimator)
                {
                    animator.SetBool(animIDJump, false);
                    animator.SetBool(animIDFreeFall, false);
                }

                // Jump
                if (input.jump && jumpTimeoutDelta <= 0.0f )
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

                    // update animator if using character
                    if (hasAnimator)
                    {
                        animator.SetBool(animIDJump, true);
                    }

                    canJump = false;
                    
                }
                //air jump
                //else if (input.jump &&) 

                // jump timeout
                if (jumpTimeoutDelta >= 0.0f)
                {
                    jumpTimeoutDelta -= Time.deltaTime;
                    if (jumpTimeoutDelta <= 0.0f)
                    {
                        recentlyJumped = false;
                    }
                }
            }
            else{
                // reset the jump timeout timer
                jumpTimeoutDelta = jumpTimeout;

                // fall timeout
                if (fallTimeoutDelta >= 0.0f)
                {
                    fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (hasAnimator)
                    {
                        animator.SetBool(animIDFreeFall, true);
                    }
                }
                // if we are not grounded, do not jump
                input.jump = false;
            }
            if (grounded)
            {
                // stop our velocity dropping infinitely when grounded
                if (verticalVelocity < 0.0f)
                {
                    verticalVelocity = -3f;
                }

                // reset the fall timeout timer
                fallTimeoutDelta = fallTimeout;

                
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (verticalVelocity < terminalVelocity)
            {
                verticalVelocity += gravity * Time.deltaTime;
            }
        }
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z),
                groundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (footstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, footstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(footstepAudioClips[index], transform.TransformPoint(controller.center), footstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(landingAudioClip, transform.TransformPoint(controller.center), footstepAudioVolume);
            }
        }

        public void SetForwardMovementSpeed(float speed){ forwardMoveSpeed = speed;}

        public void SetStrafeMovementSpeed(float speed){strafeMoveSpeed = speed;}

        public void SetBackwardMovementSpeed(float speed){backwardMoveSpeed = speed;}

        public void SetGravity(float grav){gravity = grav;}

        public void SetJumpHeight(float jump){jumpHeight = jump;}

        //intended to be used for air jump mechanics
        public void SetcanJump(bool canJump){canJump = canJump;}

        public void setPlayerMovementStyle(MovementStyles.MovementStyle FaceMove){movementStyle = FaceMove;}
    }
}