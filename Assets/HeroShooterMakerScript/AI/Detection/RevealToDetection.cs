using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using HeroShooterMaker.Character;

/*
Reveal To Detection
Reveals the player to other characters
use for characters that appear very frequently, such as swarm of small enemies. 
*/
namespace HeroShooterMaker.AI
{
    public class RevealToDetection : MonoBehaviour
    {
        [Tooltip("radius/distance at which AI can detect object")]
        public float ScanRads = 30;
        [Tooltip("mask for detecting objects")]
        public LayerMask GroundMask, TeamMask, EnemyMask;
        [Tooltip("maximum amount of object that can be detected by AI")]
        [Range(100, 9999)] public int MaxObjectDetected = 100;
        [Tooltip("Reveals itself to AI that is not scanning for itself. Turn this off if you want to have many amount of this agent.")]
        public bool EnableSelfReveal = true;

        private CharCore playerReference;
        private JobHandle overlapHandle;
        private NativeArray<OverlapSphereCommand> commands;
        private NativeArray<ColliderHit> rangeCheck;
        private bool scanInProgress = false;

        void Start()
        {
            playerReference = transform.GetComponentInParent<CharCore>();
            TeamMask = playerReference.PlayerAllegience.TeamLayer;
            EnemyMask = playerReference.PlayerAllegience.EnemyLayer;

        }

        void Update()
        {
            if (scanInProgress)
            {
                FinishScan();
            }
            else
            {
                RadiusScanAll();
            }
        }

        public void RadiusScanAll()
        {
            //if the character does not want to reveal itself
            if (!EnableSelfReveal)
            {
                return;
            }
            //scan for all objects in ScanRads 
            commands = new NativeArray<OverlapSphereCommand>(1, Allocator.TempJob);
            rangeCheck = new NativeArray<ColliderHit>(MaxObjectDetected, Allocator.TempJob);

            commands[0] = new OverlapSphereCommand(transform.position, ScanRads, QueryParameters.Default);

            overlapHandle = OverlapSphereCommand.ScheduleBatch(commands, rangeCheck, 1, MaxObjectDetected);

            scanInProgress = true;


        }

        private void FinishScan()
        {
            if (!overlapHandle.IsCompleted)
            {
                return;
            }
            overlapHandle.Complete();

            //put in list
            foreach (var obj in rangeCheck)
            {
                if (obj.collider == null)
                {
                    continue;
                }
                //check if its player
                CharCore player = obj.collider.transform.GetComponentInParent<CharCore>();
                ObjectDetection detection = obj.collider.transform.GetComponent<ObjectDetection>();
                float distance = (obj.collider.transform.position - transform.position).magnitude;
                if (player != null)
                {
                    //check if its self
                    if (obj.collider.gameObject == gameObject)
                    {
                        continue;
                    }
                    //check if its teammate
                    else if ((1 << player.gameObject.layer) == TeamMask)
                    {
                        //add self to ally's object detection 
                        if (!(detection == null) && !detection.EnableIndependentScan && distance <= detection.ScanRads)
                        {
                            detection.AddInAlly(playerReference, transform);
                            detection.SetContext();
                        }
                    }
                    //add as enemy otherwise
                    else if (((1 << player.gameObject.layer) & EnemyMask) > 0)
                    {
                        if (!(detection == null) && !detection.EnableIndependentScan && distance <= detection.ScanRads)
                        {
                            detection.AddInEnemy(playerReference, transform);
                            detection.SetContext();
                        }

                    }

                }
            }


            commands.Dispose();
            rangeCheck.Dispose();
            scanInProgress = false;
        }

        void OnDisable()
        {
            if (!overlapHandle.IsCompleted) { overlapHandle.Complete(); }
            if (commands.IsCreated) { commands.Dispose(); }
            if (rangeCheck.IsCreated) { rangeCheck.Dispose(); }
            scanInProgress = false;
        }
    }
}