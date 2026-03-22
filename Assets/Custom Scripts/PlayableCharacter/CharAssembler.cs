using UnityEngine;

public class CharAssembler : MonoBehaviour
{
    public CharAssembleInfo assembleInfo;

    public CharCore playerRef;
    public InputEventCaller inputCall;

    public void Init()
    {
        //put char stat in char core

        //tie actives to input
        // foreach (KeyValuePair<Ability,List<InputOptions.Input>> abil in assembeInfo.activeAbil)
        // {
        //     inputCall.InputDict.Add(abil.Key, abil.Value);
        // }
        //add other abilities
    }
}
