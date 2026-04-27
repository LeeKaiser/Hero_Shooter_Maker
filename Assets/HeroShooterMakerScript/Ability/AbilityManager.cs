using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using AbilityClassification;
using InputOptions;
using PlayerEvents;

/*
Ability Manager
manages ability's input, cooldown, and UI systems
easily find player's abilities through ability manager
*/
public class AbilityManager : MonoBehaviour
{
    [SerializeField] private List<Ability> abilitiesList;
    private Ability currentlyActiveAbility;
    public CharCore PlayerReference;

    //public Transform PlayerCanvas;

    public Dictionary <AbilityClass, int> AbilityClassDictionary = new Dictionary<AbilityClass, int>();

    public Dictionary <Ability, InputUnit> AbiltyToInputDictionary = new Dictionary<Ability, InputUnit>();
    public InputConverter InputConvert;

    void Awake()
    {
        foreach (AbilityClass a in Enum.GetValues(typeof(AbilityClass)))
        {
            if (a == AbilityClass.None) continue;
            AbilityClassDictionary[a] = 0;
        }
        InputConvert = PlayerReference.PlayerArmature.GetComponent<InputConverter>();
    }

    void Update()
    {
        //reload all abilities per update call:
        foreach (var ability in abilitiesList)
        {
            ability.ActivateReload();
            ability.ReloadOverTime(Time.deltaTime);
            ability.ProgressUnpause(Time.deltaTime);
        }
    }

    public List<Ability> GetAbilList(){return abilitiesList;}

    public Dictionary <AbilityClass, int> GetAbilityClassDictionary() {return AbilityClassDictionary;}

    void OnDisable()
    {
        foreach (var ability in abilitiesList)
        {
            if (ability != null)
                ability.Cleanup();
        }
    }

    public bool CanUseAbility(Ability ability)
    {
        //returns if ability is set as usable in the current system
        return currentlyActiveAbility == null || ability.Stats.CanInterruptOthers;
    }

    public void NotifyAbilityStarted(Ability ability)
    {
        if (currentlyActiveAbility != null && ability.Stats.CanInterruptOthers)
        {
            // Optionally add cancellation logic here
            Debug.Log($"{ability.name} is interrupting {currentlyActiveAbility.name}");
        }

        currentlyActiveAbility = ability;
    }

    public void NotifyAbilityEnded(Ability ability)
    {
        if (currentlyActiveAbility == ability)
            currentlyActiveAbility = null;
    }

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
        
        ability.Initialize(this, PlayerReference);
        
        AddNewAbility addAbilityEvent = new AddNewAbility();
        addAbilityEvent.PlayerIdentity = PlayerReference;
        addAbilityEvent.AddedAbility = ability;
        EventBus<AddNewAbility>.Invoke(addAbilityEvent);
        
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
                AbilityClassDictionary[a] += 1;
            }
        }
    }

    public void RemoveAbility(Ability ability)
    {
        //remove on ability list, ability class dictionary, and ability input dictionary
        if (abilitiesList.Contains(ability))
        {
            abilitiesList.Remove(ability);
            foreach(AbilityClass a in Enum.GetValues(typeof(AbilityClass)))
            {
                if (a == AbilityClass.None)
                {
                    continue;

                }

                else if (ability.CurrentAbilClass.HasFlag(a))
                {
                    AbilityClassDictionary[a] -= 1;
                }
            }
            if (AbiltyToInputDictionary.ContainsKey(ability))
            {
                AbiltyToInputDictionary.Remove(ability);
            }
            Destroy(ability.gameObject);
        }
    }

    public void SetupInput(Ability ability, ActiveAbilityID abilID, InputUnit abilInput)
    {
        InputConvert.InputDict.Add(abilInput, abilID);
        AbiltyToInputDictionary.Add(ability, abilInput);
    }
}
