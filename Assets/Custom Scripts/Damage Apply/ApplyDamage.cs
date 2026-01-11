using UnityEngine;


public class ApplyDamage : MonoBehaviour
{
    AttackInfo atkInfo;

    void Start()
    {
        atkInfo = GetComponent<AttackInfo>();
    }
    void OnTriggerEnter(Collider collision)
    {
        // check if collision has playable character core
        PlayableCharCore enemy = collision.transform.parent.GetComponent<PlayableCharCore>();

        // check if enemy player allegience is different from the player
        if (enemy != null)
        {
            Debug.Log("hit");

            if (enemy.playerAllegience == atkInfo.attackAllegience)
            {
                //projectile and target on same team, ignore unless friendly fire on
                return;
            }
            // get attack's damage
            int DamageDealt = (int) (atkInfo.baseDamage * atkInfo.owningPlayer.GetComponent<PlayableCharCore>().GetDamageMult());
            // deal damage to enemy player
            DamageDealt = enemy.DealDamage(DamageDealt);

            GameObject damageNoVis = Instantiate(atkInfo.DamageNumberPrefab, transform.position, Quaternion.identity);
            damageNoVis.GetComponent<DamageNumberScript>().Init(atkInfo.owningPlayer, DamageDealt);
        }
        
        //self destruct when hitting enemy or environment
        if (enemy != null)
        {
            atkInfo.DestroySelf();
        }
        else if (collision.gameObject.layer == atkInfo.groundLayers)
        {
            atkInfo.DestroySelf();
        }
    }
}
