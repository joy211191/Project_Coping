using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipement : MonoBehaviour {

    public List<Item> item;

    public float healthIncrase;
    public float numbnessPoolIncrease;
    public float numbnessDamagePercentage;
    public float movementSpeedIncrease;
    public float willpowerIncrease;
    public bool doubleJump;
    public bool doubleDash;

    void Awake () {
        //healthIncrase = item.healthIncrase;
    }
}
