using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float projectileSpeed = 25f;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * projectileSpeed; // Units per second
        }
    }

}
