using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//
//[CreateAssetMenu(fileName = "Item", menuName = "EquipmentItems")]
[System.Serializable]
public class Item //: ScriptableObject
{
    public string itemName;
    public float healthIncrase;
    public float numbnessPoolIncrease;
    public float numbnessDamagePercentage;
    public float movementSpeedIncrease;
    public float willpowerIncrease;
    public bool doubleJump;
    public bool doubleDash;
    public Sprite sprite;

    public void RandomizeValues () {
        //set values
    }
}
