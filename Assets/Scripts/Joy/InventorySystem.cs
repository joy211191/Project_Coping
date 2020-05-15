using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class InventorySystem : MonoBehaviour {
    public List<Item> items = new List<Item>();
    public PlayerStats playerStats;

    void Awake () {
        playerStats = FindObjectOfType<PlayerStats>();
        LoadData();
    }

    void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "PickUp") {
            AddItemToList(other.GetComponent<Item>());
        }
    }

    public void AddItemToList(Item item) {
        items.Add(item);
        SaveInventoryData();
    }

    public void EquipItem (Item equipItem) {
        playerStats.equippedItems.Add(equipItem);
        playerStats.ItemEffects();
    }

    public void UnequipItem(Item equipItem)
    {
        playerStats.equippedItems.Remove(equipItem);
        playerStats.ResetEffects();
    }

    void LoadData () {
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/ Inventory.group3")) {
            FileStream fileStream = new FileStream(Application.persistentDataPath + "/ Inventory.group3", FileMode.Open);
            items = (List<Item>)bf.Deserialize(fileStream);
            playerStats.potionCounter = (int)bf.Deserialize(fileStream);
            fileStream.Close();
        }
    }

    public void SaveInventoryData () {
        BinaryFormatter bf = new BinaryFormatter();
        if (!File.Exists(Application.persistentDataPath + "/ Inventory.group3")) {
            FileStream fileStream = new FileStream(Application.persistentDataPath + "/ Inventory.group3", FileMode.Create);
            bf.Serialize(fileStream, items);
            bf.Serialize(fileStream, playerStats.potionCounter);
            fileStream.Close();
        }
        else {
            FileStream fileStream = new FileStream(Application.persistentDataPath + "/ Inventory.group3", FileMode.Open);
            bf.Serialize(fileStream, items);
            bf.Serialize(fileStream, playerStats.potionCounter);
            fileStream.Close();
        }
    }
}