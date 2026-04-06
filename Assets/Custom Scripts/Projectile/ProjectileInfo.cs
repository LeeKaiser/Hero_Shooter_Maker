using UnityEngine;

public class ProjectileInfo : MonoBehaviour
{
    public CharCore OwningPlayer;
    public TeamManager AttackAllegience;

    public LayerMask GroundLayer, TeamLayer, EnemyLayer;
    public GameObject DestroyEffect;

    public RaycastHit[] EnemyHit;
    public RaycastHit[] AllyHit;
    public RaycastHit[] ObstacleHit;
    
    void Start()
    {
        TeamLayer = AttackAllegience.TeamLayer;
        EnemyLayer = AttackAllegience.EnemyLayer; 
    }

    public void DestroySelf(Vector3 position)
    {
        GameObject destroyEffect = Instantiate(DestroyEffect, position, transform.rotation);
        Destroy(destroyEffect,1f);
        Destroy(gameObject);
    }

}
