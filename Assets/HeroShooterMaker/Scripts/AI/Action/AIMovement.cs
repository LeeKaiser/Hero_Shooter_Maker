using UnityEngine;
using UnityEngine.AI;
using HeroShooterMaker.CharacterEvents;
using HeroShooterMaker.Controls;
using HeroShooterMaker.Character;

/*
AI Movement
Automatically causes AI to look at the LookTarget and move to MoveTarget. Requires Navmesh Agent. 

AI has modified behavior for when it encounters off mesh link. 
It moves forward and drops if the off mesh link end is close enough to be reached without jumping, 
and jumps if the distance from self to off mesh link end is too far to be made by dropping. 
*/
namespace HeroShooterMaker.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIMovement : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private CharCore playerReference;


        public Transform AimTarget;
        public Transform MoveTarget;

        private InputConverter inputConvert;

        private Vector3 agentDirection;
        private Vector3 previousMovement;
        private Vector3 landingSpot;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            playerReference = GetComponentInParent<CharCore>();
            inputConvert = GetComponent<InputConverter>();

            //set agent's speed to neglegable amount that is above 0 in order to find the direction that agent should move on its pathfinding
            agent.speed = 0.01f;

            AimTarget = playerReference.transform.Find("AimTarget").transform;
            MoveTarget = playerReference.transform.Find("MoveTarget").transform;
        }



        void Update()
        {
            AgentMovement();
        }



        //makes an artificial input to the agent's input file.
        public void AgentMovement()
        {
            Vector3 pathfindingDirection = agent.velocity.normalized;

            //when on navmesh link (on ledge or when it needs to jump), disable agent
            // if destination of navmesh link required jump input, make jump input
            // this is currently determined based on if navmesh link destination is positioned within jump trajectory
            bool inputJump = false;
            if (agent.isOnOffMeshLink)
            {
                OffMeshLinkData data = agent.currentOffMeshLinkData;
                Vector3 endPos = data.endPos - transform.position;
                float vertDist = endPos.y;
                endPos.y = 0;
                float horizDist = endPos.magnitude;
                float t = horizDist / playerReference.GetForwardSpeed();
                float y = 0.5f * playerReference.GetGravity() * t * t;
                if (y < vertDist)
                {
                    inputJump = true;
                }

                //set pathfinding direction towards direction of end point
                pathfindingDirection = endPos.normalized;
                landingSpot = data.endPos;
            }
            Vector3 airStrafe = landingSpot - transform.position;
            airStrafe.y = 0;
            Vector3 agentLookVect = AimTarget.position - transform.position;
            agentLookVect = agentLookVect.normalized; //direction agent is aiming at
            airStrafe = Quaternion.Inverse(Quaternion.LookRotation(agentLookVect)) * airStrafe;
            if (agent.enabled)
            {
                //set agent's movement input        
                switch (playerReference.MovementStyle)
                {
                    case MovementStyles.MovementStyle.RotateInsteadOfStrafe:
                        agentDirection = transform.InverseTransformDirection(pathfindingDirection);
                        break;
                    default:
                        agentDirection = Quaternion.Inverse(Quaternion.LookRotation(agentLookVect)) * pathfindingDirection;

                        break;
                }

            }
            else
            {
                //while jumping, adjust direction to landing spot if far from it
                previousMovement = Vector3.Lerp(previousMovement.normalized, airStrafe.normalized, 5f * Time.deltaTime);

                agentDirection = previousMovement;

            }
            //ensures the jump is fully completed
            if (agentDirection == Vector3.zero)
            {
                agentDirection = previousMovement;
            }
            else
            {
                previousMovement = agentDirection;
            }

            //when too close to destination, don't move
            if ((transform.position - MoveTarget.position).magnitude <= 0.5f)
            {
                agentDirection = Vector3.zero;
                inputJump = false;
            }

            //make input for agent
            Vector2 agentInput = new Vector2(agentDirection.normalized.x, agentDirection.normalized.z);
            inputConvert.JumpInput(inputJump);
            inputConvert.MoveInput(agentInput);
        }


        //Move To Location
        //sets navmesh agent's destination to the location in parameter. generally called with position of MoveTarget.
        public void MoveToLocation()
        {
            if (agent.enabled && agent.isOnNavMesh)
            {
                agent.SetDestination(MoveTarget.position);
            }

        }


    }
}