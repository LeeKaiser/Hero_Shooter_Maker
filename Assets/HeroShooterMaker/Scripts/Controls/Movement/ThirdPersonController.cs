using UnityEngine;
using UnityEngine.Serialization;
using HeroShooterMaker.CharacterEvents;
using UnityEngine.AI;
using HeroShooterMaker.EventBus;
using HeroShooterMaker.Character;

/*
ThirdPersonController
Drives locomotion for a character: directional movement, turning to face the
current target point, jumping, gravity, and a lightweight NavMeshAgent handoff
so AI and player characters can share the same controller.
*/

namespace HeroShooterMaker.Controls
{
    [RequireComponent(typeof(CharacterController))]
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]

        [Tooltip("controller active")]
        public bool IsActive = true;

        [Tooltip("Move speed of the character in m/s")]
        public float ForwardMoveSpeed = 2.0f;
        public float StrafeMoveSpeed = 2.0f;
        public float BackwardMoveSpeed = 2.0f;

        [Tooltip("if the character will turn to face movement direction or not")]
        public MovementStyles.MovementStyle movementStyle;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        [FormerlySerializedAs("RotationSmoothTime")]
        public float turnSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        [FormerlySerializedAs("SpeedChangeRate")]
        public float accelerationSharpness = 10.0f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        [FormerlySerializedAs("JumpTimeout")]
        public float jumpRearmDelay = 0.50f;

        [Tooltip("Time required for the grounded status to change")]
        public float JumpForgivenessTime = 0.3f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;
        public bool CanJump = true;
        public bool RecentlyJumped = false;

        [Tooltip("Useful for rough ground")]
        [FormerlySerializedAs("GroundedOffset")]
        public float groundProbeVerticalOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        [FormerlySerializedAs("GroundedRadius")]
        public float groundProbeRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        [FormerlySerializedAs("GroundLayers")]
        public LayerMask groundContactMask;

        [Tooltip("Transform that represents the place player is looking at")]
        public Transform targetPoint;

        // player
        private float _speed;
        private float _facingYaw;
        private float _turnRateRef;
        private float _verticalVelocity;
        private readonly float _terminalFallSpeed = 53.0f;
        private Vector3 _externalForceDirection;
        private float _externalForceVelocity;

        private Vector3 _currentDirection;

        private bool _horizontalMovementPause;
        private bool _verticalMovementPause;

        // absolute-time markers used instead of countdown counters
        private float _nextJumpAllowedTime;
        private float _lastConfirmedGroundedTime;

        // Prevents a jump from re-triggering mid-arc (short rearm delay) or from a single-frame
        // ground graze (e.g. clipping a ledge corner) - only cleared once truly landed.
        private bool _isAirborneFromJump;
        private float _groundedStreak;
        private const float RequiredGroundedStreakToClearJumpLock = 0.05f;

        private CharacterController _controller;
        private InputConverter _input;
        private NavMeshAgent _agent;

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<InputConverter>();

            _nextJumpAllowedTime = Time.time;
            _lastConfirmedGroundedTime = Time.time;

            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;
            _agent.updatePosition = false;
        }

        private void Update()
        {
            if (IsActive)
            {
                HandleLedgeDropOff();
                SampleGroundState();
                UpdateVerticalMotion();
                Move();
            }

            SyncAgentToTransform();
        }

        // teleports player to a position
        public void Translate(Vector3 targetPosition)
        {
            _controller.enabled = false;
            transform.position = targetPosition;
            _controller.enabled = true;
        }

        public void ApplyExternalForce(Vector3 direction, float velocity)
        {
            _externalForceDirection = direction;
            _externalForceVelocity = velocity;
        }

        private void HandleLedgeDropOff()
        {
            if (!_agent.enabled) return;

            float heightAboveAgent = transform.position.y - _agent.nextPosition.y;

            // character has stepped past where the agent's projected surface is - treat as a ledge
            if (heightAboveAgent > 0.2f || _agent.isOnOffMeshLink)
            {
                DisableAgent();
            }
        }

        private void SyncAgentToTransform()
        {
            if (!_agent.enabled) return;

            Vector3 planarDelta = _agent.nextPosition - transform.position;
            planarDelta.y = 0;

            if (planarDelta.sqrMagnitude > 0.1f * 0.1f)
            {
                _agent.Warp(transform.position);
            }
            else
            {
                _agent.nextPosition = transform.position;
            }
        }

        private void SampleGroundState()
        {
            bool wasGrounded = Grounded;

            // Start comfortably above the character, not just above the check point - SphereCast
            // will not register a hit against a collider it's already overlapping at the start
            // position, so the origin needs real clearance from the ground, not a thin buffer.
            const float startHeightAboveController = 0.5f;
            Vector3 probeOrigin = transform.position + Vector3.up * startHeightAboveController;

            // Sweep down far enough to reach groundProbeVerticalOffset below the character's feet.
            float probeDistance = Mathf.Max(0.01f, startHeightAboveController - groundProbeVerticalOffset);

            bool hitGround = Physics.SphereCast(probeOrigin, groundProbeRadius, Vector3.down,
                out _, probeDistance, groundContactMask, QueryTriggerInteraction.Ignore);

            Grounded = hitGround;

            if (hitGround)
            {
                _lastConfirmedGroundedTime = Time.time;
                _groundedStreak += Time.deltaTime;
                EnableAgent();

                // only clear the jump lock once we've been continuously grounded for a beat -
                // a single-frame graze (e.g. a ledge corner) won't count as a real landing
                if (_isAirborneFromJump && _groundedStreak >= RequiredGroundedStreakToClearJumpLock)
                {
                    _isAirborneFromJump = false;
                }
            }
            else
            {
                _groundedStreak = 0f;
            }

            bool withinCoyoteWindow = !hitGround && (Time.time - _lastConfirmedGroundedTime <= JumpForgivenessTime);
            CanJump = !_isAirborneFromJump && (hitGround || withinCoyoteWindow);

            PlayerGrounded groundedEvent = new PlayerGrounded();
            groundedEvent.playerIdentity = GetComponentInParent<CharCore>();
            groundedEvent.grounded = Grounded;
            EventBus<PlayerGrounded>.Invoke(groundedEvent);

            if (!wasGrounded && Grounded)
            {
                OnLanded();
            }
        }

        private void Move()
        {
            Vector2 moveInput = _input.move.normalized;
            Vector3 inputDirection = new Vector3(_input.move.x, 0f, _input.move.y).normalized;

            float baseSpeed = DetermineSpeed(moveInput);
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            float targetSpeed = baseSpeed * inputMagnitude;
            if (_input.move == Vector2.zero) targetSpeed = 0f;

            Vector3 externalPlanar = new Vector3(_externalForceDirection.x, 0f, _externalForceDirection.z) * _externalForceVelocity;
            float currentPlanarSpeed = new Vector3(_controller.velocity.x - externalPlanar.x, 0f,
                _controller.velocity.z - externalPlanar.z).magnitude;

            float speedTolerance = 0.1f;

            if (Mathf.Abs(currentPlanarSpeed - targetSpeed) > speedTolerance)
            {
                _speed = Mathf.Lerp(currentPlanarSpeed, targetSpeed, Time.deltaTime * accelerationSharpness);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            FaceTargetPoint(inputDirection);
            _currentDirection = ResolveMoveDirection();

            Vector3 movement = Vector3.zero;
            if (!_horizontalMovementPause) movement += _currentDirection.normalized * _speed;
            if (!_verticalMovementPause) movement += Vector3.up * _verticalVelocity;
            movement += _externalForceDirection.normalized * _externalForceVelocity;

            _controller.Move(movement * Time.deltaTime);
        }

        private float DetermineSpeed(Vector2 moveInput)
        {
            switch (movementStyle)
            {
                case MovementStyles.MovementStyle.AlwaysFaceForward:
                    if (Mathf.Abs(moveInput.y) >= Mathf.Abs(moveInput.x))
                    {
                        return moveInput.y >= 0 ? ForwardMoveSpeed : BackwardMoveSpeed;
                    }
                    return StrafeMoveSpeed;
                case MovementStyles.MovementStyle.RotateInsteadOfStrafe:
                    return moveInput.y >= 0 ? ForwardMoveSpeed : BackwardMoveSpeed;
                default:
                    return ForwardMoveSpeed;
            }
        }

        private void FaceTargetPoint(Vector3 inputDirection)
        {
            Vector3 toTarget = targetPoint.position - transform.position;
            _facingYaw = Mathf.Atan2(toTarget.x, toTarget.z) * Mathf.Rad2Deg;

            switch (movementStyle)
            {
                case MovementStyles.MovementStyle.FaceMovement:
                    _facingYaw += Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
                    break;
                case MovementStyles.MovementStyle.RotateInsteadOfStrafe:
                    _facingYaw = StrafeMoveSpeed * turnSmoothTime * Mathf.Atan2(inputDirection.x, 0) * Mathf.Rad2Deg + transform.eulerAngles.y;
                    break;
            }

            float smoothedYaw = Mathf.SmoothDampAngle(transform.eulerAngles.y, _facingYaw, ref _turnRateRef, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, smoothedYaw, 0.0f);
        }

        private Vector3 ResolveMoveDirection()
        {
            switch (movementStyle)
            {
                case MovementStyles.MovementStyle.AlwaysFaceForward:
                    return transform.forward * _input.move.y + transform.right * _input.move.x;
                case MovementStyles.MovementStyle.RotateInsteadOfStrafe:
                    return _input.move.y >= 0 ? transform.forward : -transform.forward;
                default:
                    return transform.forward;
            }
        }

        private void UpdateVerticalMotion()
        {
            if (CanJump)
            {
                if (_input.jump && Time.time >= _nextJumpAllowedTime)
                {
                    _verticalVelocity = ComputeJumpLaunchSpeed();
                    _nextJumpAllowedTime = Time.time + jumpRearmDelay;
                    _isAirborneFromJump = true;
                    _groundedStreak = 0f;

                    PlayerJump jumpEvent = new PlayerJump();
                    jumpEvent.playerIdentity = GetComponentInParent<CharCore>();
                    EventBus<PlayerJump>.Invoke(jumpEvent);
                }
            }
            else
            {
                _input.jump = false;
            }

            if (Grounded && _verticalVelocity < 0.0f)
            {
                _verticalVelocity = -3f;
            }

            if (_verticalVelocity < _terminalFallSpeed)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private float ComputeJumpLaunchSpeed()
        {
            // vertical speed needed for the character to reach JumpHeight under Gravity
            return Mathf.Sqrt(2f * Mathf.Abs(Gravity) * JumpHeight);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Grounded
                ? new Color(0.0f, 1.0f, 0.0f, 0.35f)
                : new Color(1.0f, 0.0f, 0.0f, 0.35f);

            Gizmos.DrawSphere(
                transform.position + Vector3.up * groundProbeVerticalOffset,
                groundProbeRadius);
        }

        // Called directly from SampleGroundState() on the airborne -> grounded transition,
        // now that landing is no longer driven by an Animator clip event.
        private void OnLanded()
        {

            PlayerLandOnGround landEvent = new PlayerLandOnGround();
            landEvent.playerIdentity = GetComponentInParent<CharCore>();
            EventBus<PlayerLandOnGround>.Invoke(landEvent);
        }

        public void SetForwardMovementSpeed(float speed) { ForwardMoveSpeed = speed; }
        public void SetStrafeMovementSpeed(float speed) { StrafeMoveSpeed = speed; }
        public void SetBackwardMovementSpeed(float speed) { BackwardMoveSpeed = speed; }
        public void SetGravity(float grav) { Gravity = grav; }
        public void SetJumpHeight(float jump) { JumpHeight = jump; }
        public void SetCanJump(bool canJump) { CanJump = canJump; }
        public void setPlayerMovementStyle(MovementStyles.MovementStyle style) { movementStyle = style; }
        public void SetHorizontalMovementPause(bool pause) { _horizontalMovementPause = pause; }
        public void SetVerticalMovementPause(bool pause) { _verticalMovementPause = pause; }

        public void ResetCharacterVelocity()
        {
            if (gameObject.activeSelf)
            {
                _controller.Move(Vector3.zero);
            }
            
        }

        public void DisableAgent()
        {
            _agent.autoTraverseOffMeshLink = false;
            _agent.enabled = false;
        }

        public void EnableAgent()
        {
            _agent.autoTraverseOffMeshLink = true;
            _agent.enabled = true;
        }

        public Vector3 GetCurrentDirection() { return _currentDirection; }
    }
}