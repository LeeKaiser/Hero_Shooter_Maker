using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using AbilityClassification;

/*
AbilityManager
stores all abilities belonging to a character
responsible for storing abilities, tying abilities to input, 
*/
public class AbilityManager : MonoBehaviour
{
    //Variables - Public
    [Tooltip("All abilities in character")]
    [SerializeField] private List<Ability> abilitiesList;
    
    [Tooltip("reference to player's character information")]
    public PlayableCharCore playerRef;

    [Tooltip("reference to player's UI canvas")]
    public Transform playerCanvas;

    //dictionary of ability class to percent of cooldown left for it
    public Dictionary <AbilityClass, int> abilClassDict = new Dictionary<AbilityClass, int>();

    //dictionary that converts from ability to input for active abilities. Used by AI to use active abilities
    public Dictionary <Ability, List<InputOptions.Input>> abilToInput = new Dictionary<Ability, List<InputOptions.Input>>();
    
    [Tooltip("Reference to character's input event caller")]
    public InputEventCaller inputEventCaller;
    
    //Variables - Private
    //reference to current active ability
    private Ability currentlyActiveAbility;

    //Methods
    //Called when character is created
    void Awake()
    {
        foreach (AbilityClass a in Enum.GetValues(typeof(AbilityClass)))
        {
            if (a == AbilityClass.None) continue;
            abilClassDict[a] = 0;
        }
        inputEventCaller = transform.Find("PlayerArmature").GetComponent<InputEventCaller>();
    }

    //called every frame.
    void Update()
    {
        //reload all abilities per update call:
        foreach (var ability in abilitiesList)
        {
            ability.ActivateReload();
            ability.ReloadOverTime(Time.deltaTime);
            //call ability's ui to update
            ability.abilUIRef.UpdateUI();
        }
    }

    //returns list of abilities
    public List<Ability> GetAbilList(){return abilitiesList;}

    //returns ability class dict
    public Dictionary <AbilityClass, int> GetAbilClassDict() {return abilClassDict;}

    //called when character is disabled or program stops. cleans up all abilities in list
    void OnDisable()
    {
        foreach (var ability in abilitiesList)
        {
            if (ability != null)
                ability.Cleanup();
        }
    }

    //returns bool based on if the ability is allowed to be used, based on factors from other abilities.
    public bool CanUseAbility(Ability ability)
    {
        //returns if ability is set as usable in the current system
        return currentlyActiveAbility == null || ability.abilityStat.canInterruptOthers;
    }

    //sets ability as being used
    public void NotifyAbilityStarted(Ability ability)
    {
        if (currentlyActiveAbility != null && ability.abilityStat.canInterruptOthers)
        {
            // Optionally add cancellation logic here
            Debug.Log($"{ability.name} is interrupting {currentlyActiveAbility.name}");
        }

        currentlyActiveAbility = ability;
    }
    //sets ability as nolonger being used
    public void NotifyAbilityEnded(Ability ability)
    {
        if (currentlyActiveAbility == ability)
            currentlyActiveAbility = null;
    }
    //called when adding a new ability to a character
    public void AddAbility(GameObject newAbility)
    {
        // Make a copy of the prefab and attach it to the player
        //GameObject abilityObj = Instantiate(abilityPrefab, transform);

        // Grab the Ability script on that prefab
        Ability ability = newAbility.GetComponent<Ability>();
        if (ability == null)
        {
            Debug.LogError("The prefab does not have an Ability component!");
            return;
        }
        ability.Initialize(this, playerRef);

        GameObject abilUI = Instantiate(ability.abilityStat.abilUIPrefab);


        AbilityUI abilUIScript = abilUI.GetComponent<AbilityUI>();
        abilUIScript.transform.SetParent(playerCanvas, false);
        if (abilUIScript != null)
        {
            abilUIScript.abilityRef = ability;
            abilUIScript.Initialize();
        }
        ability.abilUIRef = abilUIScript;
        abilitiesList.Add(ability);
        //add to ability class dictionary
        foreach(AbilityClass a in Enum.GetValues(typeof(AbilityClass)))
        {
            if (a == AbilityClass.None)
            {
                continue;

            }

            else if (ability.CurrentAbilClass.HasFlag(a))
            {
                abilClassDict[a] += 1;
            }
        }
    }
    //called when setting up active ability's input
    public void SetupInput(Ability ability, PlayerActiveAbilID abilID, List<InputOptions.Input> abilInput)
    {
        inputEventCaller.InputDict.Add(abilInput, abilID);
        abilToInput.Add(ability, abilInput);
    }
}
