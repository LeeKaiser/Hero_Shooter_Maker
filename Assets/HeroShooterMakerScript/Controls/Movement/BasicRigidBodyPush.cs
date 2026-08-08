using UnityEngine;
using UnityEngine.Serialization;
 
/*
BasicRigidBodyPush
Lets a CharacterController shove dynamic rigidbodies it walks into, so props
don't feel like immovable walls.
*/
public class BasicRigidBodyPush : MonoBehaviour
{
    [FormerlySerializedAs("pushLayers")]
    public LayerMask pushableLayers;
 
    [FormerlySerializedAs("canPush")]
    public bool pushEnabled;
 
    [FormerlySerializedAs("strength")]
    [Range(0.5f, 5f)]
    public float pushForce = 1.1f;
 
    [Tooltip("Ignore hits that are mostly pointed downward, so we don't shove things we're standing on top of.")]
    [Range(-1f, 0f)]
    public float downwardHitTolerance = -0.3f;
 
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!pushEnabled) return;
        TryPushHitBody(hit);
    }
 
    private void TryPushHitBody(ControllerColliderHit hit)
    {
        Rigidbody hitBody = hit.collider.attachedRigidbody;
        if (hitBody == null || hitBody.isKinematic) return;
 
        if (!IsLayerIncluded(hitBody.gameObject.layer, pushableLayers)) return;
 
        // ignore contacts that came from mostly above/below to avoid pushing things we're standing on
        if (hit.moveDirection.y < downwardHitTolerance) return;
 
        Vector3 planarPushDirection = Vector3.ProjectOnPlane(hit.moveDirection, Vector3.up);
        hitBody.AddForce(planarPushDirection * pushForce, ForceMode.Impulse);
    }
 
    private static bool IsLayerIncluded(int layer, LayerMask mask)
    {
        return (mask.value & (1 << layer)) != 0;
    }
}
