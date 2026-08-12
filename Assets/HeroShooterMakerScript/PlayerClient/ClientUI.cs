using UnityEngine;
using System.Collections.Generic;
using HeroShooterMaker.CharacterEvents;
using UnityEngine.UI;
using HeroShooterMaker.Abilities;
using HeroShooterMaker.EventBus;

public class ClientUI : MonoBehaviour
{
    public CharCore characterReference;
    public List<Ability> AbilitiesInUI;

    void Update()
    {
        foreach (Ability ability in AbilitiesInUI)
        {
            ability.GetAbilityUI().UpdateUI();
        }
    }

    //erase all existing UI and add new ones
    public void SetUpNewUI()
    {
        foreach (Ability ability in AbilitiesInUI)
        {
            GameObject abilUI = ability.GetAbilityUI().gameObject;
            ability.SetAbilityUI(null);
            Destroy(abilUI);
        }
        AbilitiesInUI.Clear();
        foreach (Ability ability in characterReference.AbilityManage.GetAbilList())
        {
            AddAbilityUI(ability);
        }
    }

    //add ability to ui when a new ability is added to manager
    public void AbilityAddedToManager(AddNewAbility addAbility)
    {
        
        Ability ability = addAbility.AddedAbility;
        if (characterReference != addAbility.PlayerIdentity || AbilitiesInUI.Contains(ability)){return;}

        AddAbilityUI(ability);

    }

    //add ability's ui
    public void AddAbilityUI(Ability ability)
    {

        GameObject abilUI = Instantiate(ability.Stats.AbilityUIPrefab, transform);
        AbilityUI abilUIScript = abilUI.GetComponent<AbilityUI>();
        //abilUIScript.transform.SetParent(transform, false);
        if (abilUIScript != null)
        {
            abilUIScript.AbilityReference = ability;
            abilUIScript.Initialize();
            ability.SetAbilityUI(abilUIScript);

            //change UI position if overlapping
            AbilityUI[] childUIs = GetComponentsInChildren<AbilityUI>(false);
            foreach(AbilityUI uis in childUIs)
            {
                abilUIScript.ShiftIfOverlapping(uis.GetComponent<RectTransform>());
            }

            AbilitiesInUI.Add(ability);
        }
        
    }

    void OnEnable()
    {
        EventBus<AddNewAbility>.Subscribe(AbilityAddedToManager);
    }

    void OnDisable()
    {
        EventBus<AddNewAbility>.Unsubscribe(AbilityAddedToManager);
    }
}
