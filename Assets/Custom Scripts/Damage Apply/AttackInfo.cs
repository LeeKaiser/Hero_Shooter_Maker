using UnityEngine;

public class AttackInfo : MonoBehaviour
{
    public PlayableCharCore owningPlayer;
    public TeamManager attackAllegience;

    public int baseDamage;

    public LayerMask groundLayers;

    public GameObject DamageNumberPrefab;

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

}
