using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour {
    public List<Item> items = new List<Item>();
    public PlayerStats playerStats;


    void Awake () {
        playerStats = FindObjectOfType<PlayerStats>();
    }

    void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "PickUp") {
            AddItemToList(other.GetComponent<Item>());
        }
    }

    public void AddItemToList(Item item) {
        items.Add(item);
    }

    void EquipItem (Item equipItem) {
        playerStats.equippedItems.Add(equipItem);
        playerStats.ItemEffects();
    }
}