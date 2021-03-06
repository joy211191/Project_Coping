﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIScript : MonoBehaviour
{
    [SerializeField]
    GameObject m_player;

    [Header("UI")]
    [SerializeField]
    GameObject m_InventoryBackground;
    [SerializeField]
    GameObject m_InfoPanel;
    [SerializeField]
    List<Transform> m_InventoryIcons = new List<Transform>();
    [SerializeField]
    List<GameObject> m_EquippedIndicators = new List<GameObject>();
    [SerializeField]
    Text m_ItemName;
    [SerializeField]
    Text m_ItemInfo;
    [SerializeField]
    GameObject m_interactionPrompt;

    [SerializeField]
    List<Sprite> m_itemSprites = new List<Sprite>();


    List<bool> m_ItemEquipped = new List<bool>();

    InventorySystem m_inventorySystem;

    bool m_playerIsByLocker = false;
    bool m_playerInLocker = false;

    // Start is called before the first frame update
    void Awake()
    {
        m_InventoryBackground.SetActive(false);
        m_InfoPanel.SetActive(false);
        m_inventorySystem = m_player.GetComponent<InventorySystem>();
        m_ItemInfo.text = "";
        m_ItemName.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && m_playerIsByLocker && !m_playerInLocker)
            ShowUI();
        else if (Input.GetKeyDown(KeyCode.F) && m_playerInLocker)
            HideUI();
    }

    public void ShowUI()
    {
        m_playerInLocker = true;
        m_interactionPrompt.SetActive(false);

        //Make so that the player can't move with the locker open
        m_player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        m_player.GetComponent<PlayerController>().enabled = false;
        m_player.GetComponent<PlayerAnimator>().enabled = false;

        m_InventoryBackground.SetActive(true);
        m_InfoPanel.SetActive(true);
        //Show as many items as we need
        for (int i = 0; i < m_inventorySystem.items.Count; i++)
        {


            if (m_player.GetComponent<PlayerStats>().equippedItems.Contains(m_inventorySystem.items[i]))
            {
                m_ItemEquipped.Add(true);
                m_EquippedIndicators[i].SetActive(true);
            }
            else
            {
                m_ItemEquipped.Add(false);
                m_EquippedIndicators[i].SetActive(false);
            }


           
            m_InventoryIcons[i].gameObject.SetActive(true);
            Color tempColour = m_InventoryIcons[i].GetComponent<Image>().color;
            tempColour.a = 1f;
            m_InventoryIcons[i].GetComponent<Image>().color = tempColour;
            m_InventoryIcons[i].GetComponent<Button>().enabled = true;
            //m_InventoryIcons[i].GetComponent<Image>().sprite = m_inventorySystem.items[i].itemSprite;

            if (m_inventorySystem.items[i].itemName == '"' + "Housewarming Gift" + '"')
                m_InventoryIcons[i].GetComponent<Image>().sprite = m_itemSprites[0];
            if (m_inventorySystem.items[i].itemName == "Bull Pendant")
                m_InventoryIcons[i].GetComponent<Image>().sprite = m_itemSprites[1];
            if (m_inventorySystem.items[i].itemName == "Good Luch Charm")
                m_InventoryIcons[i].GetComponent<Image>().sprite = m_itemSprites[2];
            if (m_inventorySystem.items[i].itemName == "Light Ring")
                m_InventoryIcons[i].GetComponent<Image>().sprite = m_itemSprites[3];

        }
    }

    public void HideUI()
    {
        m_playerInLocker = false;
        m_interactionPrompt.SetActive(true);

        //Let the player move again
        m_player.GetComponent<PlayerController>().enabled = true;
        m_player.GetComponent<PlayerAnimator>().enabled = true;

        m_ItemEquipped.Clear();

        for (int i = 0; i < m_inventorySystem.items.Count; i++)
        {
            m_InventoryIcons[i].gameObject.SetActive(false);
        }

        m_InventoryBackground.SetActive(false);
        m_InfoPanel.SetActive(false);
    }

    public void EquipItem(int p_inItem)
    {

        if (!m_ItemEquipped[p_inItem])
        {
            m_player.GetComponent<InventorySystem>().EquipItem(m_inventorySystem.items[p_inItem]);
            m_ItemEquipped[p_inItem] = true;
            m_EquippedIndicators[p_inItem].SetActive(true);
        }
        else
        {
            m_player.GetComponent<InventorySystem>().UnequipItem(m_inventorySystem.items[p_inItem]);
            m_ItemEquipped[p_inItem] = false;
            m_EquippedIndicators[p_inItem].SetActive(false);
        }

    }   

    public void GetInfo(int p_inItem)
    {
        m_ItemName.text = m_inventorySystem.items[p_inItem].itemName;

        if (m_inventorySystem.items[p_inItem].healthIncrase > 0)
            m_ItemInfo.text += "Max Health Increase: " + m_inventorySystem.items[p_inItem].healthIncrase.ToString() + "\n";
        if (m_inventorySystem.items[p_inItem].numbnessPoolIncrease > 0)
            m_ItemInfo.text += "Max Numbness Increase: " + m_inventorySystem.items[p_inItem].numbnessPoolIncrease.ToString() + "\n";
        if (m_inventorySystem.items[p_inItem].numbnessDamagePercentage > 0)
            m_ItemInfo.text += "Numbness Damage Increase: " + m_inventorySystem.items[p_inItem].numbnessDamagePercentage.ToString() + "% \n";
        if (m_inventorySystem.items[p_inItem].movementSpeedIncrease > 0)
            m_ItemInfo.text += "Movement Speed Increase: " + m_inventorySystem.items[p_inItem].movementSpeedIncrease.ToString() + "\n";
        if (m_inventorySystem.items[p_inItem].willpowerIncrease > 0)
            m_ItemInfo.text += "Max Willpower Increase: " + m_inventorySystem.items[p_inItem].willpowerIncrease.ToString() + "\n";
    }

    public void ClearInfo()
    {
        m_ItemName.text = "";
        m_ItemInfo.text = "";
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            m_playerIsByLocker = true;
            m_interactionPrompt.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            m_playerIsByLocker = false;
            m_interactionPrompt.SetActive(false);
        }
    }
}
