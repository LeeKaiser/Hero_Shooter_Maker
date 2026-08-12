using UnityEngine;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Collections;
using HeroShooterMaker.Character;

//PatrolLandmark
//indicates location that agent wants to patrol or thinks is important
namespace HeroShooterMaker.AI
{
    public class PatrolLandmark : MonoBehaviour
    {
        [Tooltip("the priority of the location for the agent. the index that is checked is based on the agent's patrol check index")]
        public List<int> PatrolPriority = new List<int>();

        //reveal to detection

        [Tooltip("radius/distance at which AI can detect object")]
        public float ScanRads = 30;
        [Tooltip("maximum amount of object that can be detected by AI")]
        [Range(100, 9999)] public int MaxObjectDetected = 100;
        [Tooltip("Reveals itself to AI that is not scanning for itself. Turn this off if you want to have many amount of this agent.")]
        public bool EnableSelfReveal = true;

        private JobHandle overlapHandle;
        private NativeArray<OverlapSphereCommand> commands;
        private NativeArray<ColliderHit> rangeCheck;
        private bool scanInProgress = false;

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
                    //add self to ally's object detection 
                    if (!(detection == null) && !detection.EnableIndependentScan && distance <= detection.ScanRads)
                    {
                        detection.AddPOI(obj.collider.gameObject.GetComponent<PatrolLandmark>());
                        detection.SetContext();
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