using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour {
    public List<Item> items = new List<Item>();

    void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "PickUp") {
            items.Add(other.GetComponent<Item>());
        }
    }
}