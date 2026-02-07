using UnityEngine;

public class ODTextDisplay : InGameTxtDisplay
{
    public ObjectDetection objDetect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        value = objDetect.toString();
        base.Init(owningPlayer, value);
    }

    // Update is called once per frame
    void Update()
    {
        value = objDetect.toString();
    }
}
