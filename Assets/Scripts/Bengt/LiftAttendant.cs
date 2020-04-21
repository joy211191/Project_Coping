using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System.Security.Cryptography;

public class LiftAttendant : MonoBehaviour
{
    [SerializeField]
    GameObject      m_dialogueObject;
    [SerializeField]
    GameObject      m_interactPrompt;
    [SerializeField]
    GameObject      m_player;

    protected bool  m_canTalk = false;
    protected bool  m_talking = false;

    [SerializeField]
    protected Text  m_nameText;
    [SerializeField]
    protected Text  m_dialogueText;
    [SerializeField]
    protected Image m_portrait;

    void Awake()
    {
        m_portrait      = m_dialogueObject.transform.Find("Portrait").GetComponent<Image>();
        m_dialogueText  = m_dialogueObject.transform.Find("Main Panel").Find("Dialogue Text").GetComponent<Text>();
        m_nameText      = m_dialogueObject.transform.Find("Name Panel").Find("Name").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_canTalk && Input.GetKeyDown(KeyCode.Q))
        {
            ActivateDialogue();
        }
        else if (m_talking && Input.GetKeyDown(KeyCode.Q))
        {
            DeactivateDialogue();
        }
    }

    void ActivateDialogue()
    {
        m_nameText.text     = "The Lift Attendant";
        m_dialogueText.text = "Hello there, I still don't have any cool voice lines or graphics. All of it is just shitty placeholder stuff. What's even worse is that it is all hardcoded. See this text, not even read from a text file. These devs suck.";
        m_interactPrompt.SetActive(false);
        m_dialogueObject.SetActive(true);
        m_player.GetComponent<PlayerController>().enabled   = false;
        m_player.GetComponent<PlayerAnimator>().enabled     = false;
        m_talking = true;
        m_canTalk = false;
    }

    void DeactivateDialogue()
    {
        m_dialogueObject.SetActive(false);
        m_player.GetComponent<PlayerController>().enabled   = true;
        m_player.GetComponent<PlayerAnimator>().enabled     = true;
        m_talking = false;
        m_canTalk = true;
    }

    void OnTriggerEnter2D(Collider2D m_col)
    {
        if (m_col.gameObject.name == "LightBandit");
        {
            m_interactPrompt.SetActive(true);
            m_canTalk = true;
        }
    }

    void OnTriggerExit2D(Collider2D m_col)
    {
        m_interactPrompt.SetActive(false);
        m_canTalk = false;
    }

}