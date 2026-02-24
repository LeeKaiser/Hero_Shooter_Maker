using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private PlayableCharCore playerRef;

    public Transform lookTarget;
    public Transform moveTarget;

    [SerializeField] private float angleDiff;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerRef = GetComponentInParent<PlayableCharCore>();

    }

    void Update()
    {
        MoveToLocation(moveTarget.position);
        SetLookDirection(lookTarget.position);
        CorrectNavMeshData();
    }

    public void CorrectNavMeshData()
    {
        agent.updateRotation = playerRef.PlayerFaceMovement;
        
        if (!agent.updateRotation)
        {
            //determine what the speed would be based on angle between player rotation and player movement vector
            Vector3 lookVect = transform.forward.normalized;
            Vector3 moveVect = agent.velocity.normalized;

            angleDiff = Mathf.Acos(Vector3.Dot(lookVect, moveVect)) * Mathf.Rad2Deg;

            if (angleDiff <= 45)
            {
                agent.speed = playerRef.GetForwardSpeed();
            }
            else if (angleDiff >= 135)
            {
                agent.speed = playerRef.GetBackwardSpeed();
            }
            else
            {
                agent.speed = playerRef.GetStrafeSpeed();
            }
        }
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
