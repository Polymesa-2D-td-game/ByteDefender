using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Upgrade",menuName ="Upgrade",order =0)]
//Saves all upgrade data
public class SimpleUpgrade : ScriptableObject
{
    [Header("Misc")]
    public int upgradeCost;
    public string upgradeName;
    public string description;
    [Header("Apearence and Spawns")]
    public Sprite spriteUpgrade;
    public GameObject spawnObjectUpgrade;
    [Header("Stats")]
    public float powerUpgrade;
    public float speedUpgrade;
    public float rangeUpgrade;
    public float emmitForceUpgrade;
}
