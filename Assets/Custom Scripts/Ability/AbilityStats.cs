using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using System;

[CreateAssetMenu(fileName = "AbilityStats", menuName = "Scriptable Objects/AbilityStats")]
public class AbilityStats : ScriptableObject
{
    //variables
    [Tooltip("name of the ability")]
    public string abilityName = "";
    public GameObject abilUIPrefab = null;

    [Header("Cooldown variables")]
    [Tooltip("maximum amount of charge that can be held")]
    public int maxCharge = 1;

    [Tooltip("Amount of charge to gain in order to complete a charge once")]
    public float chargePointsRequired = 100;

    [Tooltip("amount of charge point gained per second ")]
    public float chargePointsPerSec = 100;

    [Tooltip("amount of charge gained per full charge point")]
    public int chargeGainPerFullRecharge = 1;

    [Header("use time related variables")]
    [Tooltip("if using ability disables use of other abilities")]
    public bool canInterruptOthers = false;
    [Tooltip("amount of time ability use is disabled for when using the ability")]
    public float useTime = 0.2f;
    [Tooltip("amount of time ability use is disabled for when attempting to use ability and it fails")]
    public float useFailTime = 0.0f;

    [Header("charge variables")]
    [Tooltip("amount of time to charge up ability")]
    public float maxChargeTime;

    [Header("hold fire variables")]
    [Tooltip("amount of time ability is used per second")]
    public float usePerSec;

}
