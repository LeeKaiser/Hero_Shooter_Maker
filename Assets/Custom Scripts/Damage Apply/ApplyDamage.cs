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
        CharCore enemy = collision.transform.parent.GetComponent<CharCore>();

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
            int damageDealt = (int) (atkInfo.baseDamage * atkInfo.owningPlayer.GetDamageMult());
            // deal damage to enemy player
            damageDealt = enemy.DealDamage(damageDealt);

            GameObject damageNoVis = Instantiate(atkInfo.DamageNumberPrefab, transform.position, Quaternion.identity);
            damageNoVis.GetComponent<DamageNumberScript>().Init(atkInfo.owningPlayer.gameObject, ""+damageDealt);
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
