using UnityEngine;
using HeroShooterMaker.AI;
using HeroShooterMakerDemo;

//Delete before release
public class ODTextDisplay : InGameTxtDisplay
{
    public ObjectDetection objDetect;
    [SerializeField] private string detectionToString;
    float x = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        value = "Test AI char";
        Init(transform, value);
    }

    // Update is called once per frame
    public override void Update()
    {
        if (x > 0)
        {
            x -= Time.deltaTime;
        }
        else
        {
            detectionToString = objDetect.toString();
        }
        
        base.Update();
    }
}
