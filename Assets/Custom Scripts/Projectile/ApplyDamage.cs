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
        StartCoroutine(CheckForCollision());
    }
    
    void ResetHitPlayers()
    {
        alreadyHitPlayers.Clear();
    }

    IEnumerator CheckForCollision()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (info.EnemyHit.Count > 0)
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

                    //TODO: manage damage number separately 
                    Vector3 damageNumPos = enemy.PlayerArmature.transform.position;
                    damageNumPos.y += enemy.PlayerArmature.GetComponent<CharacterController>().height;
                    damageNumPos += new Vector3(Random.Range(-0.3f,0.3f),Random.Range(-0.3f,0.3f),Random.Range(-0.3f,0.3f));
                    GameObject damageNoVis = Instantiate(DamageNumberPrefab, damageNumPos , Quaternion.identity);
                    damageNoVis.GetComponent<DamageNumberScript>().Init(info.OwningPlayer.gameObject, ""+damageDealt);

                    //add to already hit player so that it does not continuously hit the player.
                    alreadyHitPlayers.Add(enemy);
                    MaxHits -= 1;
                }
                
            }
        }
        
    }
}
