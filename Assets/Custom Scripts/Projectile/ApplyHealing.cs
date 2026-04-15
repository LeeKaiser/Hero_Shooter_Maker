using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayerEvents;

public class ApplyHealing : MonoBehaviour
{
    public int MaxHits = 1;
    public int BaseHealing;

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
            if (info.AllyHit.Count > 0)
            {
                foreach(var target in info.AllyHit)
                {
                    if (MaxHits <= 0)
                    {
                        break;
                    }
                    CharCore ally = target.transform.parent.GetComponent<CharCore>();
                    if (alreadyHitPlayers.Contains(ally))
                    {
                        continue;
                    }
                    int healthHealed = (int) (BaseHealing /** info.OwningPlayer.GetDamageMult()*/);
                    // heal ally
                    healthHealed = ally.HealHealth(healthHealed, info.OwningPlayer);

                    //add to already hit player so that it does not continuously hit the player.
                    alreadyHitPlayers.Add(ally);
                    MaxHits -= 1;
                }
                
            }
        }
        
    }
}
