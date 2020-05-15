using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipementSpawner : MonoBehaviour {
    /// <summary>
    /// For this to work, just add in empty objects as the child of this object in the hierarchy and position them as desired and the pickups will be spawned at the positions else it will not spawn and randomize.
    /// Be sure to tag the items as 'PickUp'
    /// </summary>

    public GameObject itemPrefab;
    public List<Item> items=new List<Item>();
    public List<Transform> pickUpSpawnPoints=new List<Transform>();


    [SerializeField]
    bool enabled = false;
    [SerializeField]
    bool randomize=false;

    [Space(20)]
    [Header("Ranges for randomizing values. The x denotes the low end and y denotes the high end")]
    [SerializeField]
    Vector2 healthIncrase;
    [SerializeField]
    Vector2 numbnessPoolIncrease;
    [SerializeField]
    Vector2 numbnessDamagePercentage;
    [SerializeField]
    Vector2 movementSpeedIncrease;
    [SerializeField]
    Vector2 willpowerIncrease;

    [SerializeField]
    List<Sprite> sprites = new List<Sprite>();
    [SerializeField]
    List<string> itemNames = new List<string>();

    void Awake () {

		if (transform.childCount > 0)
		{
			foreach (Transform child in transform)
			{
				pickUpSpawnPoints.Add(child);
			}
		}

		if (enabled) {
            if (pickUpSpawnPoints.Count > 0)
                SpawnPickUps();
        }
    }

    void SpawnPickUps () {
        for (int i = 0; i <pickUpSpawnPoints.Count; i++) {
            GameObject go = Instantiate(itemPrefab, pickUpSpawnPoints[i]);
            Item tempItem = go.GetComponent<Item>();
            if (randomize)
                Randomizer(tempItem);
        }
    }

    void Randomizer (Item item) {
        item.healthIncrase = Random.Range(healthIncrase.x, healthIncrase.y);
        item.numbnessPoolIncrease = Random.Range(numbnessPoolIncrease.x, numbnessPoolIncrease.y);
        item.numbnessDamagePercentage = Random.Range(numbnessDamagePercentage.x, numbnessDamagePercentage.y);
        item.movementSpeedIncrease = Random.Range(movementSpeedIncrease.x, movementSpeedIncrease.y);
        item.willpowerIncrease = Random.Range(willpowerIncrease.x, willpowerIncrease.y);
        item.itemName = itemNames[Random.Range(0, itemNames.Count)];
    }
}
