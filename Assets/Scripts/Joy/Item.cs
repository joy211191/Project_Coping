using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Item: MonoBehaviour
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

    public void ApplySprite () {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
