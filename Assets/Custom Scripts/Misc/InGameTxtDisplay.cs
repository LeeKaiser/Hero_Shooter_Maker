using UnityEngine;
using TMPro;

public class InGameTxtDisplay : MonoBehaviour
{
    [SerializeField] protected string value;
    public TextMeshPro textDisplay;
    public float ReferenceDistance = 1;
    Transform playerCam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Init(Transform camera, string valueTxt)
    {
        value = valueTxt;
        textDisplay.text = value;
        playerCam = camera;
        
    }

    // Update is called once per frame
    public void Update()
    {
        if (!(playerCam == null))
        {
            Vector3 awayDir =  transform.position - playerCam.position;
            transform.rotation = Quaternion.LookRotation(awayDir);
            textDisplay.text = value;
            float textScale = awayDir.magnitude / ReferenceDistance;
            transform.localScale = Vector3.one * textScale;
            Debug.Log(transform.localScale);
        }
        
        
    }
}
