using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class ApplyDamage : MonoBehaviour
{
    public int MaxHits = 1;
    public int BaseDamage;
    public GameObject DamageNumberPrefab;

    ProjectileInfo info;
    List<CharCore> alreadyHitPlayers = new List<CharCore>();
    

    void Start()
    {
        info = GetComponent<ProjectileInfo>();
    }
    
    void FixedUpdate()
    {
        StartCoroutine(CheckForCollision());
    }

    void ResetHitPlayers()
    {
        alreadyHitPlayers.Clear();
    }

    IEnumerator CheckForCollision()
    {
        yield return new WaitForFixedUpdate();
        if (info.EnemyHit.Length > 0)
        {
            foreach(var target in info.EnemyHit)
            {
                if (MaxHits <= 0)
                {
                    break;
                }
                CharCore enemy = target.transform.parent.GetComponent<CharCore>();
                if (alreadyHitPlayers.Contains(enemy))
                {
                    continue;
                }
                int damageDealt = (int) (BaseDamage * info.OwningPlayer.GetDamageMult());
                // deal damage to enemy player
                damageDealt = enemy.DealDamage(damageDealt);

                GameObject damageNoVis = Instantiate(DamageNumberPrefab, transform.position, Quaternion.identity);
                damageNoVis.GetComponent<DamageNumberScript>().Init(info.OwningPlayer.gameObject, ""+damageDealt);
                alreadyHitPlayers.Add(enemy);
                MaxHits -= 1;
            }
            
        }
        
        
    }
}
