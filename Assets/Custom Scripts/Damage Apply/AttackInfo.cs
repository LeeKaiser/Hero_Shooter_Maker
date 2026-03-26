using UnityEngine;

public class AttackInfo : MonoBehaviour
{
    public CharCore owningPlayer;
    public TeamManager attackAllegience;

    public int baseDamage;

    public LayerMask groundLayers;

    public GameObject DamageNumberPrefab;
    public GameObject DestroyEffect;

    public void DestroySelf()
    {
        //TODO: spawn in destroy effect
        GameObject destroyEffect = Instantiate(DestroyEffect, transform.position, transform.rotation);
        Destroy(destroyEffect,1f);
        Destroy(gameObject);
    }

}
