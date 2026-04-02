using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using System;

/*
AbilityStats
contains stats for abilities. 
*/
[CreateAssetMenu(fileName = "AbilityStats", menuName = "Scriptable Objects/AbilityStats")]
public class AbilityStats : ScriptableObject
{
    //variables
    [Tooltip("name of the ability")]
    public string AbilityName = "";
    public GameObject AbilityUIPrefab = null;

    [Header("Cooldown variables")]
    [Tooltip("maximum amount of charge that can be held")]
    public int MaxCharge = 1;

    [Tooltip("Amount of charge to gain in order to complete a charge once")]
    public float ChargePointsRequired = 100;

    [Tooltip("amount of charge point gained per second ")]
    public float ChargePointsPerSec = 100;

    [Tooltip("amount of charge gained per full charge point")]
    public int ChargeGainPerFullRecharge = 1;

    [Header("use time related variables")]
    [Tooltip("if using ability disables use of other abilities")]
    public bool CanInterruptOthers = false;

    [Header("charge variables")]
    [Tooltip("amount of time to charge up ability")]
    public float MaxChargeTime;

    [Header("hold fire variables")]
    [Tooltip("amount of time ability is used per second")]
    public float UsePerSec;

}
