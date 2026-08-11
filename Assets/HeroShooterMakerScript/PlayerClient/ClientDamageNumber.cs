using UnityEngine;
using HeroShooterMaker.CharacterEvents;
using System.Collections.Generic;
using System.Collections;

public class ClientDamageNumber : MonoBehaviour
{
    public CharCore CharacterReference;
    public Transform PlayerCamera;
    public GameObject DamageNumberPrefab;
    public GameObject HealNumberPrefab;

    Dictionary<CharCore, int> characterToDamage = new Dictionary<CharCore, int>();
    Dictionary<CharCore, int> characterToHeal = new Dictionary<CharCore, int>();
    Dictionary<CharCore, int> recentDamageTaken = new Dictionary<CharCore, int>();
    Dictionary<CharCore, int> recentHealingTaken = new Dictionary<CharCore, int>();

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

    IEnumerator GenerateHealNumber()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            
            foreach (KeyValuePair<CharCore, int> character in characterToHeal)
            {
                Vector3 damageNumPos = character.Key.PlayerArmature.transform.position;
                damageNumPos.y += character.Key.PlayerArmature.GetComponent<CharacterController>().height;
                damageNumPos += new Vector3(Random.Range(-0.3f,0.3f),Random.Range(-0.3f,0.3f),Random.Range(-0.3f,0.3f));
                GameObject damageNoVis = Instantiate(HealNumberPrefab, damageNumPos , Quaternion.identity);
                damageNoVis.GetComponent<DamageNumberScript>().Init(PlayerCamera, ""+character.Value);
            }
            
            characterToHeal.Clear();
        }
    }

    //TODO: change to make it show multiple damage number for different sources
    IEnumerator GenerateDamageTakenNumber()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            
            foreach (KeyValuePair<CharCore, int> character in recentDamageTaken)
            {
                Vector3 damageNumPos = CharacterReference.PlayerArmature.transform.position;
                damageNumPos.y += CharacterReference.PlayerArmature.GetComponent<CharacterController>().height;
                damageNumPos += new Vector3(Random.Range(-0.3f,0.3f),Random.Range(-0.3f,0.3f),Random.Range(-0.3f,0.3f));
                GameObject damageNoVis = Instantiate(DamageNumberPrefab, damageNumPos , Quaternion.identity);
                damageNoVis.GetComponent<DamageNumberScript>().Init(PlayerCamera, ""+character.Value);
            }
            
            recentDamageTaken.Clear();
        }
    }

    IEnumerator GenerateHealTakenNumber()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            
            foreach (KeyValuePair<CharCore, int> character in recentHealingTaken)
            {
                Vector3 damageNumPos = CharacterReference.PlayerArmature.transform.position;
                damageNumPos.y += CharacterReference.PlayerArmature.GetComponent<CharacterController>().height;
                damageNumPos += new Vector3(Random.Range(-0.3f,0.3f),Random.Range(-0.3f,0.3f),Random.Range(-0.3f,0.3f));
                GameObject damageNoVis = Instantiate(HealNumberPrefab, damageNumPos , Quaternion.identity);
                damageNoVis.GetComponent<DamageNumberScript>().Init(PlayerCamera, ""+character.Value);
            }
            
            recentHealingTaken.Clear();
        }
    }

    public void ShowDamageDealt(PlayerTakeDamage dealDamage)
    {
        if (dealDamage.DamageDealer != CharacterReference || dealDamage.PlayerIdentity == CharacterReference)
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

    public void ShowHealingDone(PlayerHealHealth healHealth)
    {
        if (healHealth.Healer != CharacterReference || healHealth.PlayerIdentity == CharacterReference)
        {
            return;
        }
        if (characterToHeal.ContainsKey(healHealth.PlayerIdentity))
        {
            characterToHeal[healHealth.PlayerIdentity] += healHealth.Healing;
        }
        else
        {
            characterToHeal.Add(healHealth.PlayerIdentity, healHealth.Healing);
        }
    }

    public void ShowTakenDamage(PlayerTakeDamage takeDamage)
    {
        if (takeDamage.PlayerIdentity != CharacterReference)
        {
            return;
        }
        if (recentDamageTaken.ContainsKey(takeDamage.DamageDealer))
        {
            recentDamageTaken[takeDamage.DamageDealer] += takeDamage.Damage;
        }
        else
        {
            recentDamageTaken.Add(takeDamage.DamageDealer, takeDamage.Damage);
        }
    }

    public void ShowTakenHealing(PlayerHealHealth healHealth)
    {
        if (healHealth.PlayerIdentity != CharacterReference)
        {
            return;
        }
        if (recentHealingTaken.ContainsKey(healHealth.Healer))
        {
            recentHealingTaken[healHealth.Healer] += healHealth.Healing;
        }
        else
        {
            recentHealingTaken.Add(healHealth.Healer, healHealth.Healing);
        }
    }


    void OnEnable()
    {
        EventBus<PlayerTakeDamage>.Subscribe(ShowDamageDealt);
        EventBus<PlayerTakeDamage>.Subscribe(ShowTakenDamage);
        EventBus<PlayerHealHealth>.Subscribe(ShowHealingDone);
        EventBus<PlayerHealHealth>.Subscribe(ShowTakenHealing);
        StartCoroutine(GenerateDamageNumber());
        StartCoroutine(GenerateDamageTakenNumber());
        StartCoroutine(GenerateHealNumber());
        StartCoroutine(GenerateHealTakenNumber());
    }

    void OnDisable()
    {
        EventBus<PlayerTakeDamage>.Unsubscribe(ShowDamageDealt);
        EventBus<PlayerTakeDamage>.Unsubscribe(ShowTakenDamage);
        EventBus<PlayerHealHealth>.Unsubscribe(ShowHealingDone);
        EventBus<PlayerHealHealth>.Unsubscribe(ShowTakenHealing);
    }
}
