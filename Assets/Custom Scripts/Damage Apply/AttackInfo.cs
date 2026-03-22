using UnityEngine;

public class AttackInfo : MonoBehaviour
{
    public CharCore owningPlayer;
    public TeamManager attackAllegience;

    public int baseDamage;

    public LayerMask groundLayers;

    public GameObject DamageNumberPrefab;

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

}
