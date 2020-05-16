using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public float healthIncrase;
    public float numbnessPoolIncrease;
    [Range(0f,1f)]
    public float numbnessDamagePercentage;
    public float movementSpeedIncrease;
    public float willpowerIncrease;
    public bool doubleJump;
    public bool doubleDash;
    //public UnityEngine.Sprite itemSprite;
}
