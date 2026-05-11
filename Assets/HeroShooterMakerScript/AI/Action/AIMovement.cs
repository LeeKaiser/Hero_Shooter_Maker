using UnityEngine;
using UnityEngine.AI;
using MovementInputEvents;

/*
AI Movement
Automatically causes AI to look at the LookTarget and move to MoveTarget. Requires Navmesh Agent. 

AI has modified behavior for when it encounters off mesh link. 
It moves forward and drops if the off mesh link end is close enough to be reached without jumping, 
and jumps if the distance from self to off mesh link end is too far to be made by dropping. 
*/
public class AIMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private CharCore playerReference;


    public Transform AimTarget;
    public Transform MoveTarget;

    private InputConverter inputConvert;

    private Vector3 agentDirection;
    private float navmeshPause = 0;

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

    void OnEnable()
    {
    }

    void OnDisable()
    {
    }

    void Update()
    {
        AgentMovement();
    }

    

    //makes an artificial input to the agent's input file.
    public void AgentMovement()
    {
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
            float y = -0.5f * playerReference.GetGravity() * t * t;
            if (y < vertDist)
            {
                inputJump = true;
            }
            navmeshPause = t + 0.1f;

            //temporary fix to ai freezing at ledges, implement an actual fix in the future
            if (agentDirection == Vector3.zero)
            {
                Debug.Log("fixed input to 1z");
                agentDirection.z = 1;
            }
        }

        //set agent's movement input        
        if (agent.enabled)
        {
            switch (playerReference.MovementStyle)
            {
                case MovementStyles.MovementStyle.RotateInsteadOfStrafe:
                    agentDirection = transform.InverseTransformDirection(agent.velocity.normalized);
                    break;
                default:
                    Vector3 agentLookVect = AimTarget.position - transform.position;
                    agentLookVect = agentLookVect.normalized; //direction agent is aiming at
                    Vector3 agentMoveVect = agent.velocity.normalized; //direction agent moves in world space
                    agentDirection = Quaternion.Inverse(Quaternion.LookRotation(agentLookVect)) * agentMoveVect;
                    break;
            }
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
        if (agent.enabled)
        {
            agent.SetDestination(MoveTarget.position);
        }
        else
        {
            agent.enabled = true;
            agent.SetDestination(MoveTarget.position);
            agent.enabled = false;
        }
        
    }

    
}
