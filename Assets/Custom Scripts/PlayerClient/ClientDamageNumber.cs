using UnityEngine;
using PlayerEvents;
using System.Collections.Generic;
using System.Collections;

public class ClientDamageNumber : MonoBehaviour
{
    public CharCore CharacterReference;
    public Transform PlayerCamera;
    public GameObject DamageNumberPrefab;

    Dictionary<CharCore, int> characterToDamage = new Dictionary<CharCore, int>();
    int recentDamageTaken = 0;

    IEnumerator GenerateDamageNumber()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            
            foreach (KeyValuePair<CharCore, int> character in characterToDamage)
            {
                Vector3 damageNumPos = character.Key.PlayerArmature.transform.position;
                damageNumPos.y += character.Key.PlayerArmature.GetComponent<CharacterController>().height;
                damageNumPos += new Vector3(Random.Range(-0.3f,0.3f),Random.Range(-0.3f,0.3f),Random.Range(-0.3f,0.3f));
                GameObject damageNoVis = Instantiate(DamageNumberPrefab, damageNumPos , Quaternion.identity);
                damageNoVis.GetComponent<DamageNumberScript>().Init(PlayerCamera, ""+character.Value);
            }
            
            characterToDamage.Clear();
        }
        
    }

    IEnumerator GenerateDamageTakenNumber()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            
            if (recentDamageTaken > 0)
            {
                Vector3 damageNumPos = CharacterReference.PlayerArmature.transform.position;
                damageNumPos.y += CharacterReference.PlayerArmature.GetComponent<CharacterController>().height;
                damageNumPos += new Vector3(Random.Range(-0.3f,0.3f),Random.Range(-0.3f,0.3f),Random.Range(-0.3f,0.3f));
                GameObject damageNoVis = Instantiate(DamageNumberPrefab, damageNumPos , Quaternion.identity);
                damageNoVis.GetComponent<DamageNumberScript>().Init(PlayerCamera, ""+recentDamageTaken);
                
                recentDamageTaken = 0;
            }
            
        }
    }

    public void ShowDamageDealt(PlayerTakeDamage dealDamage)
    {
        if (dealDamage.DamageDealer != CharacterReference)
        {
            return;
        }
        if (characterToDamage.ContainsKey(dealDamage.PlayerIdentity))
        {
            characterToDamage[dealDamage.PlayerIdentity] += dealDamage.Damage;
        }
        else
        {
            characterToDamage.Add(dealDamage.PlayerIdentity, dealDamage.Damage);
        }
        
        
    }

    public void ShowTakenDamage(PlayerTakeDamage takeDamage)
    {
        if (takeDamage.PlayerIdentity != CharacterReference)
        {
            return;
        }
        recentDamageTaken += takeDamage.Damage;
    }


    void OnEnable()
    {
        EventBus<PlayerTakeDamage>.Subscribe(ShowDamageDealt);
        EventBus<PlayerTakeDamage>.Subscribe(ShowTakenDamage);
        StartCoroutine(GenerateDamageNumber());
        StartCoroutine(GenerateDamageTakenNumber());
    }

    void OnDisable()
    {
        EventBus<PlayerTakeDamage>.Unsubscribe(ShowDamageDealt);
        EventBus<PlayerTakeDamage>.Unsubscribe(ShowTakenDamage);
    }
}
