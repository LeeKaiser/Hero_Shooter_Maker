using UnityEngine;
using UnityEngine.AI;
using StarterAssets;
using MovementStyles;

public class AIMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private PlayableCharCore playerRef;


    public Transform lookTarget;
    public Transform moveTarget;

    [SerializeField] private float angleDiff;

    private StarterAssetsInputs movementInput;

    private Vector3 agentDirection;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerRef = GetComponentInParent<PlayableCharCore>();
        movementInput = GetComponent<StarterAssetsInputs>();
        agent.updateRotation = false;
        //set agent's speed to neglegable amount that is above 0 in order to find the direction that agent should move on its pathfinding
        agent.speed = 0.01f;
    }

    void Update()
    {
        MoveToLocation(moveTarget.position);
        AgentMovement();
    }

    //makes an artificial input to the agent's input file.
    public void AgentMovement()
    {
        
        switch (playerRef.movementStyle)
        {
            case MovementStyle.RotateInsteadOfStrafe:
                agentDirection = transform.InverseTransformDirection(agent.velocity.normalized);
                break;
            default:
                Vector3 agentLookVect = lookTarget.position - transform.position;
                agentLookVect = agentLookVect.normalized; //direction agent is aiming at
                Vector3 agentMoveVect = agent.velocity.normalized; //direction agent moves in world space
                agentDirection = Quaternion.Inverse(Quaternion.LookRotation(agentLookVect)) * agentMoveVect;
                break;
        }
        
        Vector2 agentInput = new Vector2(agentDirection.normalized.x, agentDirection.normalized.z);
        movementInput.MoveInput(agentInput);
    }

    
    //Move To Location
    //sets navmesh agent's destination to the location in parameter. generally called with position of moveTarget.
    public void MoveToLocation(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    
}
