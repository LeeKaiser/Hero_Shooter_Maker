using UnityEngine;
using HeroShooterMaker.Character;
using System.Collections;
using HeroShooterMaker.Projectile;

public class ProjectileShooter : MonoBehaviour
{

    public GameObject SubjectProjectile;
    
    public CharCore TestOwner;
    IEnumerator SpawnProjectile()
    {
        GameObject attackObj = Instantiate(SubjectProjectile, transform.position, transform.rotation);

        ProjectileInfo atkInfo = attackObj.GetComponent<ProjectileInfo>();
        atkInfo.OwningPlayer = TestOwner;
        atkInfo.AttackAllegience = TestOwner.PlayerAllegience;
        yield return new WaitForSeconds(1);
        StartCoroutine(SpawnProjectile());
    }

    void Start()
    {
        StartCoroutine(SpawnProjectile());
    }
}
