using UnityEngine;
using System.Collections;
using HeroShooterMaker.Character;
using HeroShooterMaker.Abilities;
using HeroShooterMaker.Projectile;

public class ProjectileTester : MonoBehaviour
{
    public GameObject SubjectProjectile;
    
    public CharCore TestOwner;
    IEnumerator SpawnProjectile()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            GameObject attackObj = Instantiate(SubjectProjectile, transform.position, transform.rotation);

            ProjectileInfo atkInfo = attackObj.GetComponent<ProjectileInfo>();
            if (atkInfo != null)
            {
                atkInfo.OwningPlayer = TestOwner;
                atkInfo.AttackAllegience = TestOwner.PlayerAllegience;
            }
        }
    }

    void Start()
    {
        StartCoroutine(SpawnProjectile());
    }
}
