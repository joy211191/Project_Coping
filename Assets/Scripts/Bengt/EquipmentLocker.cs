using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentLocker : MonoBehaviour
{

    [SerializeField]
    GameObject m_interactionPrompt;
    [SerializeField]
    InventoryUIScript m_inventoryUIScript;

    bool m_playerIsByLocker = true;
    bool m_playerInLocker   = false;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && m_playerIsByLocker && !m_playerInLocker)
            m_inventoryUIScript.ShowUI();
        else if (Input.GetKeyDown(KeyCode.F) && m_playerInLocker)
            m_inventoryUIScript.HideUI();
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
