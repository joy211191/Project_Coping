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
        else if (!m_canTalk && m_talking && Input.GetKeyDown(KeyCode.Q) && m_dialogueNum < m_dialogueLines.Length)
            ContinueDialogue();
        else if (m_talking && Input.GetKeyDown(KeyCode.Q) && m_dialogueNum == m_dialogueLines.Length)
        {
            DeactivateDialogue();
            m_dialogueNum = 0;
        }
    }

    void ActivateDialogue()
    {
        m_nameText.text     = gameObject.name;
        TextfileToList();
        //m_dialogueText.text = ReadString();
        if (m_dialogueText == null)
            Debug.Log("Fuck");
        m_dialogueText.text = m_dialogueLines[m_dialogueNum];
        m_dialogueNum++;

        m_dialogueObject.SetActive(true);
        m_player.GetComponent<PlayerController>().enabled   = false;
        m_player.GetComponent<PlayerAnimator>().enabled     = false;
        m_talking = true;
        m_canTalk = false;
    }

    void ContinueDialogue()
    {
        m_dialogueText.text = m_dialogueLines[m_dialogueNum];
        m_dialogueNum++;
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

    void TextfileToList()
    {
        int i = 0;

        try
        {
            Debug.Log("help");

            m_dialogueLines = File.ReadAllLines(m_dialoguePath, Encoding.UTF8);
            //using (StreamReader m_reader = new StreamReader(m_dialoguePath))
            //{
            //    Debug.Log("help2");


            //    string m_line;

            //    while ((m_line = m_reader.ReadLine()) != null)
            //    {
            //        Debug.Log("help");
            //        m_dialogueLines[i] = m_line;
            //        i++;
            //        Debug.Log(m_line);
            //        Debug.Log(i);

            //    }
            //}

        }
        catch
        {
            Debug.Log("Could not read file");
        }

    }

}