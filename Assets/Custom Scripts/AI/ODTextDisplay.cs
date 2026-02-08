using UnityEngine;

public class ODTextDisplay : InGameTxtDisplay
{
    public ObjectDetection objDetect;
    float x = 0.6f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        value = "";
        Init(owningPlayer, value);
    }

    // Update is called once per frame
    void Update()
    {
        if (x > 0)
        {
            x -= Time.deltaTime;
        }
        else
        {
            value = objDetect.toString();
        }
        
        //base.Update();
    }
}
