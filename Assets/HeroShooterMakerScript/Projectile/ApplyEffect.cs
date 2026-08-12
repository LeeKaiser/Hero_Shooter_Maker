using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HeroShooterMaker.CharacterEvents;
using HeroShooterMaker.Character;

public abstract class ApplyEffect : MonoBehaviour
{
    [Tooltip("number of targets it can hit at a time")]
    public int MaxHits = 1;
    [Tooltip("if true, summon an object in projectile info ObjectToSpawn variable")]
    public bool SpawnsObjectInInfo;
    [Tooltip("resets max hit every hit reset time. set to 0 if you want no reset.")]
    public float HitResetTime = 0.5f;

    [Tooltip("Affects enemies")]
    public bool ApplyToEnemy = true;
    [Tooltip("Affects teammates")]
    public bool ApplyToAllies = false;
    [Tooltip("Affects Self")]
    public bool ApplyToSelf = true;

    protected ProjectileInfo info;
    protected List<CharCore> alreadyHitPlayers = new List<CharCore>();
    

    void Start()
    {
        info = GetComponent<ProjectileInfo>();
        StartCoroutine(CheckForCollision());
        StartCoroutine(ResetHits());
    }
    
    public void ResetHitPlayers()
    {
        alreadyHitPlayers.Clear();
    }

    IEnumerator ResetHits()
    {
        if (HitResetTime != 0f)
        {
            while (true)
            {
                yield return new WaitForSeconds(HitResetTime);
                ResetHitPlayers();
            }
        }
    }

    IEnumerator CheckForCollision()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (ApplyToEnemy)
            {
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
                        //apply effect
                        ActivateEffect(enemy);

                        if (SpawnsObjectInInfo)
                        {
                            info.SpawnObject(target.point);
                        }

                        //add to already hit player so that it does not continuously hit the player.
                        alreadyHitPlayers.Add(enemy);
                        MaxHits -= 1;
                    }
                    
                }
            }
            if (ApplyToAllies || ApplyToSelf)
            {
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
                        if ((ApplyToSelf && ally == info.OwningPlayer) || (ApplyToAllies && ally != info.OwningPlayer))
                        {
                            //filter for if it is self when targetting self or when allies and targetting allies

                            //apply effect
                            ActivateEffect(ally);

                            if (SpawnsObjectInInfo)
                            {
                                info.SpawnObject(target.point);
                            }

                            //add to already hit player so that it does not continuously hit the player.
                            alreadyHitPlayers.Add(ally);
                            MaxHits -= 1;
                            
                        }
                        
                    }
                    
                }
            }
            
        }
        
    }

    protected abstract void ActivateEffect(CharCore targetPlayer);
}
