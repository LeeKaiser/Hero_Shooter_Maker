using UnityEngine;
using UnityEngine.AI;
using StarterAssets;

public class AIMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private PlayableCharCore playerRef;


    public Transform lookTarget;
    public Transform moveTarget;

    [SerializeField] private float angleDiff;

    private StarterAssetsInputs movementInput;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerRef = GetComponentInParent<PlayableCharCore>();
        movementInput = GetComponent<StarterAssetsInputs>();
        agent.updateRotation = false;
        agent.speed = 0.01f;
    }

    void Update()
    {
        MoveToLocation(moveTarget.position);
        SetLookDirection(lookTarget.position);
        //CorrectNavMeshData();
        AgentMovement();
    }

    public void AgentMovement()
    {
        Vector3 directionToInput = transform.InverseTransformDirection(agent.velocity.normalized);
        Vector2 agentDirection = new Vector2(directionToInput.normalized.x, directionToInput.normalized.z);
        Debug.Log(agentDirection);
        movementInput.MoveInput(agentDirection);
    }

    

    public void MoveToLocation(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public void SetLookDirection(Vector3 lookPosition)
    {
        if (!agent.updateRotation)
        {
            lookPosition.y = transform.position.y;
            Vector3 lookDirection = lookPosition - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * agent.angularSpeed);
        }
    }
}
