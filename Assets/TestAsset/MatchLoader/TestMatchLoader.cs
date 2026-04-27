using UnityEngine;
using UnityEngine.SceneManagement;

public class TestMatchLoader : MonoBehaviour
{
    public CharAssembleInfo CharAssemble;
    public PlayerClient client;

    void Start()
    {
        CharAssemble = client.CharacterReference.GetComponent<CharAssembler>().assembleInfo;
    }
    

    public void LoadMatch()
    {
        SceneManager.LoadScene(1);
    }
}
