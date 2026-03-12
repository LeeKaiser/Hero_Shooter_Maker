using UnityEngine;
using UnityEngine.AI;
using StarterAssets;

/*
AI Movement
continuously moves the character to a destination
*/
public class AIMovement : MonoBehaviour
{
    //Varaibles - Public
    [Tooltip("reference to navmesh agent")]
    [SerializeField] private NavMeshAgent agent;
    [Tooltip("reference to player's information")]
    [SerializeField] private PlayableCharCore playerRef;

    [Tooltip("transform the character looks at")]
    public Transform lookTarget;
    [Tooltip("transform the character moves to")]
    public Transform moveTarget;

    //Variables - private
    //reference to movement input 
    private StarterAssetsInputs movementInput;
    //the direction agent is inputting for movement
    private Vector3 agentDirection;
    //amount of time until navmesh is unpaused. 
    private float navmeshPause = 0;

    //called when character is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerRef = GetComponentInParent<PlayableCharCore>();
        movementInput = GetComponent<StarterAssetsInputs>();
        agent.updateRotation = false;
        //set agent's speed to neglegable amount that is above 0 in order to find the direction that agent should move on its pathfinding
        agent.speed = 0.01f;
    }

    //called every frame
    void Update()
    {
        MoveToLocation(moveTarget.position);
        AgentMovement();
        //lets time pass on navmesh pause
        //TODO: change it to unpause when character lands on the ground
        if (navmeshPause > 0)
        {
            navmeshPause -= Time.deltaTime;
            //unpause navmesh
            if (navmeshPause <= 0)
            {
                //Debug.Log("Unpaused navmesh");
                agent.enabled = true;
                agent.autoTraverseOffMeshLink = true;
            }
        }
    }

    //makes an artificial input to the agent's input file.
    public void AgentMovement()
    {
        //when on navmesh link (on ledge or when it needs to jump), disable agent
        // if destination of navmesh link required jump input, make jump input
        // this is currently determined based on if navmesh link destination is positioned within fall trajectory
        bool inputJump = false;
        if (agent.isOnOffMeshLink)
        {
            
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            Vector3 endPos = data.endPos - transform.position;
            float vertDist = endPos.y;
            endPos.y = 0;
            float horizDist = endPos.magnitude;
            float t = horizDist / playerRef.GetForwardSpeed();
            float y = -0.5f * playerRef.GetGravity() * t * t;
            if (y < vertDist)
            {
                inputJump = true;
            }
            agent.enabled = false;
            agent.autoTraverseOffMeshLink = false;
            navmeshPause = t + 0.1f; //TODO: refactor this in the future to unpause at the exact time it would land on ground

            //temporary fix to ai freezing at ledges, implement an actual fix in the future
            //TODO: make a legitamate fix for this issue (fixing navmesh pause timing is likely enough)
            if (agentDirection == Vector3.zero)
            {
                agentDirection.x = 1;
            }
        }

        //set agent's movement input        
        if (agent.enabled)
        {
            switch (playerRef.movementStyle)
            {
                case MovementStyles.MovementStyle.RotateInsteadOfStrafe:
                    agentDirection = transform.InverseTransformDirection(agent.velocity.normalized);
                    break;
                default:
                    Vector3 agentLookVect = lookTarget.position - transform.position;
                    agentLookVect = agentLookVect.normalized; //direction agent is aiming at
                    Vector3 agentMoveVect = agent.velocity.normalized; //direction agent moves in world space
                    agentDirection = Quaternion.Inverse(Quaternion.LookRotation(agentLookVect)) * agentMoveVect;
                    break;
            }
        }
        
        //make input for agent
        Vector2 agentInput = new Vector2(agentDirection.normalized.x, agentDirection.normalized.z);
        movementInput.JumpInput(inputJump);
        movementInput.MoveInput(agentInput);
    }

    
    //Move To Location
    //sets navmesh agent's destination to the location in parameter. generally called with position of moveTarget.
    public void MoveToLocation(Vector3 destination)
    {
        if (agent.enabled)
        {
            agent.SetDestination(destination);
        }
        
    }

    
}
