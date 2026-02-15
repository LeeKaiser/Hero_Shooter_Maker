using UnityEngine;
using Unity.Behavior;

public abstract class Decision : MonoBehaviour
{
    public BehaviorGraph behaviorGraph;
    public abstract float ScoreDecision(KnownContext currentContext);


}
