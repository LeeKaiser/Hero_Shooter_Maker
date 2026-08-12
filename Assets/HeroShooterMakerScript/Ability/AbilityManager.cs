using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using HeroShooterMaker.Controls;
using HeroShooterMaker.CharacterEvents;

/*
Ability Manager
manages ability's input, cooldown, and UI systems
easily find player's abilities through ability manager
*/
namespace HeroShooterMaker.Abilities
{
    public class AbilityManager : MonoBehaviour
    {
        [SerializeField] private List<Ability> abilitiesList;
        private Ability currentlyActiveAbility;
        public CharCore PlayerReference;

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
            
        }
        void Start()
        {
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
                    InputConvert.InputDict.Remove(AbiltyToInputDictionary[ability]);
                    AbiltyToInputDictionary.Remove(ability);

                }
                Destroy(ability.gameObject);
            }
        }

        public void SetupInput(Ability ability, ActiveAbilityID abilID, InputUnit abilInput)
        {
            //don't add if same input
            if (InputConvert.InputDict.ContainsKey(abilInput)) return;
            InputConvert.InputDict.Add(abilInput, abilID);
            AbiltyToInputDictionary.Add(ability, abilInput);
        }
    }

}
