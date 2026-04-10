using UnityEngine;
using PlayerEvents;

public class ClientDamageNumber : MonoBehaviour
{
    public CharCore CharacterReference;
    public Transform PlayerCamera;
    public GameObject DamageNumberPrefab;

    

    public void ShowDamageDealt(PlayerTakeDamage dealDamage)
    {
        if (dealDamage.DamageDealer != CharacterReference)
        {
            return;
        }
        CharCore enemy = dealDamage.PlayerIdentity;
        Vector3 damageNumPos = enemy.PlayerArmature.transform.position;
        damageNumPos.y += enemy.PlayerArmature.GetComponent<CharacterController>().height;
        damageNumPos += new Vector3(Random.Range(-0.3f,0.3f),Random.Range(-0.3f,0.3f),Random.Range(-0.3f,0.3f));
        GameObject damageNoVis = Instantiate(DamageNumberPrefab, damageNumPos , Quaternion.identity);
        damageNoVis.GetComponent<DamageNumberScript>().Init(PlayerCamera, ""+dealDamage.Damage);
    }

    void OnEnable()
    {
        EventBus<PlayerTakeDamage>.Subscribe(ShowDamageDealt);
    }

    void OnDisable()
    {
        EventBus<PlayerTakeDamage>.Unsubscribe(ShowDamageDealt);
    }
}
