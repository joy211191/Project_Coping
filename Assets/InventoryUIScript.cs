using System.Collections;
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
    List<Transform> m_InventoryIcons = new List<Transform>();
    [SerializeField]
    List<GameObject> m_EquippedIndicators = new List<GameObject>();
    List<bool> m_ItemEquipped = new List<bool>();

    // Start is called before the first frame update
    void Awake()
    {
        m_InventoryBackground.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !m_InventoryBackground.activeSelf)
            ShowUI();
        else if (Input.GetKeyDown(KeyCode.I) && m_InventoryBackground.activeSelf)
            HideUI();
    }

    protected void ShowUI()
    {
        //Make so that the player can't move with the locker open
        m_player.GetComponent<PlayerController>().enabled = false;
        m_player.GetComponent<PlayerAnimator>().enabled = false;

        //Show as many items as we need
        for (int i = 0; i < m_player.GetComponent<InventorySystem>().items.Count; i++)
        {

            if (m_player.GetComponent<PlayerStats>().equippedItems.Contains(m_player.GetComponent<InventorySystem>().items[i]))
            {
                m_ItemEquipped.Add(true);
                m_EquippedIndicators[i].SetActive(true);
            }
            else
            {
                m_ItemEquipped.Add(false);
                m_EquippedIndicators[i].SetActive(false);
            }

            m_InventoryBackground.SetActive(true);
            //m_InventoryIcons.sprite = m_player.GetComponent<InventorySystem>().items[i].sprite; //Show the sprite of the object
            Color tempColour = m_InventoryIcons[i].GetComponent<Image>().color;
            tempColour.a = 1f;
            m_InventoryIcons[i].GetComponent<Image>().color = tempColour;
            m_InventoryIcons[i].GetComponent<Button>().enabled = true;
        }
    }

    protected void HideUI()
    {
        //Let the player move again
        m_player.GetComponent<PlayerController>().enabled = true;
        m_player.GetComponent<PlayerAnimator>().enabled = true;

        m_ItemEquipped.Clear();

        for (int i = 0; i < m_player.GetComponent<InventorySystem>().items.Count; i++)
        {
            m_InventoryBackground.SetActive(false);
        }
    }

    public void EquipItem(int p_inItem)
    {
        if (!m_ItemEquipped[p_inItem])
        {
            m_player.GetComponent<InventorySystem>().EquipItem(m_player.GetComponent<InventorySystem>().items[p_inItem]);
            m_ItemEquipped[p_inItem] = true;
            m_EquippedIndicators[p_inItem].SetActive(true);
        }
        else
        {
            m_player.GetComponent<InventorySystem>().UnequipItem(m_player.GetComponent<InventorySystem>().items[p_inItem]);
            m_ItemEquipped[p_inItem] = false;
            m_EquippedIndicators[p_inItem].SetActive(false);
        }

    }   
}
