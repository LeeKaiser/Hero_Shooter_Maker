using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TakeCover", menuName = "AIAction/Movement/MoveTakeCover")]
public class DetermineMovementTakeCover : DetermineMovement
{
    [Header("Cover Search Settings")]
    public float searchRadius = 15f;
    public float behindEdgeOffset = 1.0f;  // How far behind the edge to sample
    public int edgeSamplesPerSearch = 20;   // How many edge points to try
    public float randomAngleTweak = 10f;
    public float randomDistanceTweak = 2f;
    public override void ExecuteDetermineMovement(AIAction action)
    {
        if (!(action.Detection.GetCurrentContext().KnownEnemyList == null))
        {
        //go away from enemy for cases where there is no cover
            Vector3 nextDestination = action.playerArmature.transform.position;

            Vector3 enemyToSelf =  action.playerArmature.transform.position - action.targetPlayer.transform.position;
            Quaternion randomRot = Quaternion.AngleAxis(Random.Range(-randomAngleTweak,randomAngleTweak),Vector3.up);
            nextDestination = nextDestination + (randomRot * enemyToSelf.normalized * 2);

            //go behind nearby cover
            // Sample points around the agent and find NavMesh edges near each
            Vector3 agentPos = action.playerArmature.transform.position;
            Vector3 enemyPos = action.targetPlayer.transform.position;
            List<Vector3> candidates = new List<Vector3>();

            for (int i = 0; i < edgeSamplesPerSearch; i++)
            {
                // Spread sample origins evenly in a circle around the agent
                float angle = (360f / edgeSamplesPerSearch) * i;
                Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
                Vector3 sampleOrigin = agentPos + dir * (searchRadius * 0.5f);

                // Snap sample origin to NavMesh
                UnityEngine.AI.NavMeshHit navHit;
                if (!UnityEngine.AI.NavMesh.SamplePosition(sampleOrigin, out navHit, searchRadius, UnityEngine.AI.NavMesh.AllAreas))
                    continue;

                // Find the nearest NavMesh boundary edge from this point
                UnityEngine.AI.NavMeshHit edgeHit;
                if (!UnityEngine.AI.NavMesh.FindClosestEdge(navHit.position, out edgeHit, UnityEngine.AI.NavMesh.AllAreas))
                    continue;

                Vector3 edgePoint = edgeHit.position;
                Vector3 edgeNormal = edgeHit.normal;

                // Collect candidates along this edge
                candidates.AddRange(SampleAlongEdge(edgePoint, edgeNormal, enemyPos));
            }

            float bestDist = Mathf.Infinity;

            foreach (Vector3 candidate in candidates)
            {
                if (!IsInCover(candidate, enemyPos, action.obstacleMask))
                    continue;

                float dist = Vector3.Distance(agentPos, candidate);
                if (dist < bestDist)
                {
                    bestDist = dist;
                    nextDestination = candidate;
                }
            }

            Debug.Log(nextDestination);
            action.MoveTarget.position = nextDestination;
            action.Movement.MoveToLocation();
        }
    }

    //cover search created with help of Claude AI
    // Samples several points along the edge, offset behind it away from the enemy
    private List<Vector3> SampleAlongEdge(Vector3 edgePoint, Vector3 edgeNormal, Vector3 enemyPos)
    {
        List<Vector3> points = new List<Vector3>();

        // Make sure normal points AWAY from the enemy
        Vector3 toEdge = (edgePoint - enemyPos).normalized;
        if (Vector3.Dot(edgeNormal, toEdge) < 0)
            edgeNormal = -edgeNormal;

        // The edge runs perpendicular to the normal (in the XZ plane)
        Vector3 edgeTangent = Vector3.Cross(edgeNormal, Vector3.up).normalized;

        // Sample left, center, and right along the edge
        float[] tangentOffsets = { -1f, -0.5f, 0f, 0.5f, 1f };
        foreach (float t in tangentOffsets)
        {
            Vector3 alongEdge = edgePoint + edgeTangent * t;
            Vector3 behindEdge = alongEdge + edgeNormal * behindEdgeOffset;

            // Snap the candidate to the NavMesh
            UnityEngine.AI.NavMeshHit snapHit;
            if (UnityEngine.AI.NavMesh.SamplePosition(behindEdge, out snapHit, 1.5f, UnityEngine.AI.NavMesh.AllAreas))
                points.Add(snapHit.position);
        }

        return points;
    }

    // Raycast from enemy to point — if something blocks it, the point is in cover
    private bool IsInCover(Vector3 point, Vector3 enemyPos, LayerMask obstacleMask)
    {
        Vector3 direction = point - enemyPos;

        return Physics.Raycast(enemyPos, direction.normalized, direction.magnitude, obstacleMask);
    }
}
