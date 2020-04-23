using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ItemType {
    Portions,
    Equipment,
    WillTokens,
    RepairKit
}


[CreateAssetMenu(fileName = "Item", menuName = "InventoryItems", order = 1)]
public class Item : ScriptableObject
{
    public ItemType itemType;
    public string nameOfItem;
    //public float 
}
