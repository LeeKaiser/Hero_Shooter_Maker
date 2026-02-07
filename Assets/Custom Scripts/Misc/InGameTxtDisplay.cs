using UnityEngine;
using TMPro;

public class InGameTxtDisplay : MonoBehaviour
{
    public GameObject owningPlayer;
    [SerializeField] protected string value;
    TextMeshPro textDisplay;
    Transform playerCam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Init(GameObject owner, string value)
    {
        owningPlayer = owner;
        this.value = value;
        textDisplay = gameObject.GetComponentInChildren<TextMeshPro>();
        textDisplay.text = value;
        playerCam = owningPlayer.GetComponentInChildren<Camera>().transform;
    }

    // Update is called once per frame
    public void Update()
    {
        Vector3 awayDir =  transform.position - playerCam.position;
        transform.rotation = Quaternion.LookRotation(awayDir);
        textDisplay.text = value;
        
    }
}
