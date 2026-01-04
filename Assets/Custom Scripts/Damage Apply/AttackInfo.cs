using UnityEngine;

public class AttackInfo : MonoBehaviour
{
    public GameObject owningPlayer;
    public TeamManager attackAllegience;

    public int baseDamage;

    public LayerMask groundLayers;

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

}
