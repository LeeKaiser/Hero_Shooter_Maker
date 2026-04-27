using UnityEngine;
using TMPro;

public class DamageNumberScript : InGameTxtDisplay
{
    

    [SerializeField] private float remainingDuration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        remainingDuration -= Time.deltaTime;
        if (remainingDuration <= 0)
        {
            Destroy(gameObject);
        }
    }
}
