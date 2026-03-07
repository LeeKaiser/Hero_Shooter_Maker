using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using AbilityClassification;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private List<Ability> abilitiesList;
    private Ability currentlyActiveAbility;
    public PlayableCharCore playerRef;

    public Transform playerCanvas;

    public Dictionary <AbilityClass, int> abilClassDict = new Dictionary<AbilityClass, int>();

    void Awake()
    {
        foreach (AbilityClass a in Enum.GetValues(typeof(AbilityClass)))
        {
            if (a == AbilityClass.None) continue;
            abilClassDict[a] = 0;
        }
    }

    void Start()
    {
    }

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

    public List<Ability> GetAbilList(){return abilitiesList;}

    public Dictionary <AbilityClass, int> GetAbilClassDict() {return abilClassDict;}

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
        return currentlyActiveAbility == null || ability.abilityStat.canInterruptOthers;
    }

    public void NotifyAbilityStarted(Ability ability)
    {
        if (currentlyActiveAbility != null && ability.abilityStat.canInterruptOthers)
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


}
