using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftAttendant : MonoBehaviour
{
    [SerializeField]
    GameObject m_dialoguePrompt;
    [SerializeField]
    GameObject m_dialogueTest;
    [SerializeField]
    GameObject m_player;

    protected bool m_canTalk = false;
    protected bool m_talking = false;

    // Update is called once per frame
    void Update()
    {
        if (m_canTalk && Input.GetKeyDown(KeyCode.Q))
        {
            m_dialoguePrompt.SetActive(false);
            m_dialogueTest.SetActive(true);
            m_player.GetComponent<PlayerController>().enabled = false;
            m_player.GetComponent<PlayerAnimator>().enabled = false;
            m_talking = true;
            m_canTalk = false;
        }
        else if (m_talking && Input.GetKeyDown(KeyCode.Q))
        {
            m_dialogueTest.SetActive(false);
            m_player.GetComponent<PlayerController>().enabled = true;
            m_player.GetComponent<PlayerAnimator>().enabled = true;
            m_talking = false;
            m_canTalk = true;
        }
    }


    void OnTriggerEnter2D(Collider2D m_col)
    {
        if (m_col.gameObject.name == "LightBandit");
        {
            m_dialoguePrompt.SetActive(true);
            m_canTalk = true;
        }
    }

    void OnTriggerExit2D(Collider2D m_col)
    {
        m_dialoguePrompt.SetActive(false);
        m_canTalk = false;
    }

}
