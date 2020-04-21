using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;

public class LiftAttendant : MonoBehaviour
{
    [SerializeField]
    GameObject      m_dialogueObject;
    [SerializeField]
    GameObject      m_interactPrompt;
    [SerializeField]
    GameObject      m_player;
    [SerializeField]
    string          m_dialoguePath;

    protected bool  m_canTalk = false;
    protected bool  m_talking = false;

    protected Text  m_nameText;
    [SerializeField]
    protected Text  m_dialogueText;
    protected Image m_portrait;

    protected int m_dialogueNum = 0;

    protected string[] m_dialogueLines;

    void Awake()
    {
        //Find all the UI object children and put the in the right place
        m_nameText.text = gameObject.name;
        m_portrait      = m_dialogueObject.transform.Find("Portrait").GetComponent<Image>();
        m_dialogueText  = m_dialogueObject.transform.Find("Main Panel").Find("Dialogue Text").GetComponent<Text>();
        m_nameText      = m_dialogueObject.transform.Find("Name Panel").Find("Name").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_canTalk && Input.GetKeyDown(KeyCode.Q))
            ActivateDialogue();
        else if (!m_canTalk && m_talking && Input.GetKeyDown(KeyCode.Q) && m_dialogueNum < m_dialogueLines.Length)
            ContinueDialogue();
        else if (m_talking && Input.GetKeyDown(KeyCode.Q) && m_dialogueNum == m_dialogueLines.Length)
        {
            DeactivateDialogue();
            m_dialogueNum = 0; //reset dialoguenum
        }
    }

    void ActivateDialogue()
    {
        TextfileToList();
        //Not really necessary but in case it breaks, not really helpful for a build
        if (m_dialogueText == null)
            Debug.Log("Fuck");

        //Add text in first place of array and increment
        m_dialogueText.text = m_dialogueLines[m_dialogueNum];
        m_dialogueNum++;

        //Show dialogue box and stop all player actions, some bool changing as well
        m_dialogueObject.SetActive(true);
        m_player.GetComponent<PlayerController>().enabled   = false;
        m_player.GetComponent<PlayerAnimator>().enabled     = false;
        m_talking = true;
        m_canTalk = false;
    }

    void ContinueDialogue()
    {
        //Next dialogue text
        m_dialogueText.text = m_dialogueLines[m_dialogueNum];
        m_dialogueNum++;
    }

    void DeactivateDialogue()
    {
        //Removes dialogue box and allow the player to move again, change bools back as well
        m_dialogueObject.SetActive(false);
        m_player.GetComponent<PlayerController>().enabled   = true;
        m_player.GetComponent<PlayerAnimator>().enabled     = true;
        m_talking = false;
        m_canTalk = true;
    }


    //Triggered by player
    void OnTriggerEnter2D(Collider2D m_col)
    {
        if (m_col.gameObject.name == "LightBandit");
        {
            m_interactPrompt.SetActive(true);
            m_canTalk = true;
        }
    }

    //No longer triggered by player
    void OnTriggerExit2D(Collider2D m_col)
    {
        m_interactPrompt.SetActive(false);
        m_canTalk = false;
    }


    //Read from .txt file
    void TextfileToList()
    {
        try
        {
            //Read the entirety of the file and puts everything in its own place in the array
            m_dialogueLines = File.ReadAllLines(m_dialoguePath, Encoding.UTF8);

        }
        catch
        {
            //Print if it can't read the file
            Debug.Log("Could not read file. Is the path correct?");
        }

    }

}