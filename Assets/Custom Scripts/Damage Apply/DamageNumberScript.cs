using UnityEngine;
using TMPro;

public class DamageNumberScript : MonoBehaviour
{
    public GameObject owningPlayer;
    public int value;
    TextMeshPro dmgText;
    Transform playerCam;

    [SerializeField] private float remainingDuration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Init(GameObject owner, int dmg)
    {
        owningPlayer = owner;
        value = dmg;
        dmgText = gameObject.GetComponentInChildren<TextMeshPro>();
        dmgText.text = value + "";
        playerCam = owningPlayer.GetComponentInChildren<Camera>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 awayDir =  transform.position - playerCam.position;
        transform.rotation = Quaternion.LookRotation(awayDir);
        remainingDuration -= Time.deltaTime;
        if (remainingDuration <= 0)
        {
            Destroy(gameObject);
        }
    }
}
