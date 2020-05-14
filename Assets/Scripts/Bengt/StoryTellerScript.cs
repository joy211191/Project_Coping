using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Collections.Specialized;


public class StoryTellerScript : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField]
    GameObject m_dialogueObject;
    [SerializeField]
    protected List<GameObject> m_dialogueOptionButtons = new List<GameObject>();
    [SerializeField]
    GameObject m_dialogueOptionHolder;
    [SerializeField]
    GameObject m_interactPrompt;
    [SerializeField]
    string m_portraitPath;
    [Header("Dialogue Filepaths")]
    [SerializeField]
    List<string> m_dialogueList = new List<string>();
    [SerializeField]
    string m_endDialoguePath;
    [SerializeField]
    string m_finalDialoguePath;
    [Header("Text Colour")]
    [SerializeField]
    Color m_activeText;
    [SerializeField]
    Color m_inactiveText;
    [Header("Other")]
    [SerializeField]
    GameObject m_player;


    protected bool m_canTalk = false;
    protected bool m_talking = false;
    protected bool m_waitingForInput = false;

    protected Text m_dialogueText;
    protected Text m_nameText;

    [SerializeField]
    protected Image m_portrait;

    protected int m_activeDialogueOption = 0;
    protected int m_dialogueNum = 0;
    protected int m_dialogueOptions = 0;
    protected int m_dialogueCounter = 0;

    protected string m_dialoguePath;
    protected string[] m_dialogueLines;
    protected List<string> m_dialogueFlags = new List<string>();

    //Temporary? TODO: Find a way to remove this, low priority
    int i;


    void Awake()
    {
        //Find all the UI object children and put the in the right place and give name
        m_dialogueText = m_dialogueObject.transform.Find("Dialogue/Main Panel/Dialogue Text").GetComponent<Text>();
        m_nameText = m_dialogueObject.transform.Find("Dialogue/Name Panel/Name").GetComponent<Text>();
        m_portrait = m_dialogueObject.transform.Find("Dialogue/Portrait").GetComponent<Image>();
        m_nameText.text = gameObject.name;

        m_dialoguePath = m_dialogueList[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (m_canTalk && Input.GetKeyDown(KeyCode.Q))
            ActivateDialogue();
        else if (!m_canTalk && m_talking && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) && m_dialogueLines[m_dialogueNum] != "*END*" && !m_waitingForInput && m_dialogueLines[m_dialogueNum] != "*NOPROGRESSEND*")
            ContinueDialogue();
        else if (m_waitingForInput)
            SelectDialogue();
        else if (m_talking && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) && (m_dialogueLines[m_dialogueNum] == "*END*" || m_dialogueLines[m_dialogueNum] == "*NOPROGRESSEND*"))
            DeactivateDialogue();
    }

    void ActivateDialogue()
    {
        //Reset these two again, just in case
        i = 0;
        m_dialogueNum = 0;
        m_activeDialogueOption = 0;

        m_interactPrompt.SetActive(false);

        //Read all the text and put it in a list
        TextfileToList();
        //Not really necessary but in case it breaks
        if (m_dialogueText == null)
            Debug.Log("Cannot find text. Is the file empty?");

        //Add text in first place of array and increment
        m_dialogueText.text = m_dialogueLines[m_dialogueNum].Split('"', '"')[1];
        m_dialogueNum++;

        //Show dialogue box and stop all player actions, some bool changing as well
        m_dialogueObject.SetActive(true);
        m_player.GetComponent<PlayerController>().enabled = false;
        m_player.GetComponent<PlayerAnimator>().enabled = false;
        m_talking = true;
        m_canTalk = false;

        m_portrait.sprite = Resources.Load<Sprite>("Portraits/" + m_portraitPath);

        if (!m_dialogueLines[m_dialogueNum].StartsWith("<"))
        {
            m_dialogueText.text = m_dialogueLines[m_dialogueNum].Split('"', '"')[1];
            PrintContinue();
        }
        else
        {
            PrintContinue();
            m_dialogueOptions = 0;

            while ((m_dialogueNum + 1) < m_dialogueLines.Length && m_dialogueLines[m_dialogueNum + 1].StartsWith("<"))
            {
                m_dialogueNum++;
                string temp = m_dialogueLines[m_dialogueNum];
                PrintDialogueOption(temp);
                m_waitingForInput = true;
            }

            i = 0;//Resets this little thingy, TODO: Replace with something
        }


    }



    void ContinueDialogue()
    {
        //Prints dialogue if next row doesn't start with a '<'
        if (!m_dialogueLines[m_dialogueNum].StartsWith("<"))
        {
            m_dialogueText.text = m_dialogueLines[m_dialogueNum].Split('"', '"')[1];
            PrintContinue();
        }

        if (m_dialogueLines[m_dialogueNum].Contains("{"))   //Checks if we want to run a function
        {
            if (m_dialogueLines[m_dialogueNum].Split('{', '}')[1] == "giveItem") //If the function is giveItem
            {
                Item temp = (Item)this.GetType().GetField(m_dialogueLines[m_dialogueNum].Split('-', '-')[1]).GetValue(this);
                GiveItem(temp);
            }
            else if (m_dialogueLines[m_dialogueNum].Split('{', '}')[1] == "refillPotions") //If the function is giveItem
                RefillPotions();
        }

        //Calls PrintDialogueOption for every dialogue option
        while ((m_dialogueNum + 1) < m_dialogueLines.Length && m_dialogueLines[m_dialogueNum + 1].StartsWith("<"))
        {
            m_dialogueNum++;
            string temp = m_dialogueLines[m_dialogueNum];
            PrintDialogueOption(temp);
            m_waitingForInput = true;
        }

        i = 0;//Resets this little thingy, TODO: Replace with something

        //If you havn't reached the end keep printing dialogue stuff
        if (m_dialogueLines[m_dialogueNum] != "*END*" || m_dialogueLines[m_dialogueNum] != "*NOPROGRESSEND*")
            m_dialogueNum++;
    }

    void DeactivateDialogue()
    {
        //Removes dialogue box and allow the player to move again, change bools back as well
        m_dialogueObject.SetActive(false);

        m_interactPrompt.SetActive(true);

        m_player.GetComponent<PlayerController>().enabled = true; //Allow the player to move again
        m_player.GetComponent<PlayerAnimator>().enabled = true;

        m_talking = false; //Reset the bools
        m_canTalk = true;
        m_waitingForInput = false;

        m_dialogueNum = 0; //reset dialoguenum
        i = 0;//Resets this little thingy, TODO: Replace with something

        if (m_dialogueLines[m_dialogueNum] == "*END*")
            m_dialoguePath = m_endDialoguePath;

        Array.Clear(m_dialogueLines, 0, m_dialogueLines.Length);//Clear the array
    }

    //Prints dialogue options
    void PrintDialogueOption(string p_inText)
    {
        //Unused code, kept for posterity
        //m_dialogueOptionButtons.Add(Instantiate(m_dialogueOptionPrefab, new Vector3(Screen.width / 2, (Screen.height / 2) + (i * 200), 0), Quaternion.identity));
        //m_dialogueOptionButtons[i].transform.Find("Choice Text").GetComponent<Text>().text  = p_inText.Split('<', '>')[1];
        //m_dialogueOptionButtons[i].transform.Find("Choice Text").GetComponent<Text>().color = m_inactiveText;

        //m_dialogueOptionHolder.SetActive(true);
        //m_dialogueOptionButtons[i].SetActive(true);
        //m_dialogueOptionButtons[i].transform.parent = m_dialogueObject.transform;
        m_dialogueOptionButtons[i].GetComponent<Text>().text = p_inText.Split('<', '>')[1];
        m_dialogueOptionButtons[i].GetComponent<Text>().color = m_inactiveText;
        m_dialogueFlags.Add(p_inText.Split('[', ']')[1]);
        m_dialogueOptions++;
        i++;
    }

    //Handles selection of dialogue options
    private void SelectDialogue()
    {
        //Set right font colour, colour can be changed in the menu
        for (int i = 0; i < m_dialogueOptions; i++)
        {
            if (m_activeDialogueOption == i)
                m_dialogueOptionButtons[i].GetComponent<Text>().color = m_activeText;
            else
                m_dialogueOptionButtons[i].GetComponent<Text>().color = m_inactiveText;
        }

        // W or S - Switch between options
        // Return - Select active option
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (m_activeDialogueOption == 0)
                m_activeDialogueOption = m_dialogueOptions - 1;
            else
                m_activeDialogueOption--;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (m_activeDialogueOption == m_dialogueOptions - 1)
                m_activeDialogueOption = 0;
            else
                m_activeDialogueOption++;
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            m_waitingForInput = false;
            FindFlag(m_dialogueFlags[m_activeDialogueOption]);

            //Remove the options
            //for (int i = 0; i < m_dialogueOptionButtons.Count; i++)
            //m_dialogueOptionButtons[i].SetActive(false);

            //m_dialogueOptionHolder.SetActive(false);
            //Destroy(m_dialogueOptionButtons[i]);

            //Left for posterity
            //m_dialogueOptionButtons.Clear();


            //Clear Dialogue flags
            m_dialogueFlags.Clear();

            //Checks if player selected a dialogue ending option
            if (m_dialogueLines[m_dialogueNum] == "*END*" || m_dialogueLines[m_dialogueNum] == "*NOPROGRESSEND*")
                DeactivateDialogue();
            else
            {
                m_dialogueText.text = m_dialogueLines[m_dialogueNum].Split('"', '"')[1];
                PrintContinue();
            }

            m_dialogueNum++;

            m_dialogueOptions = 0;

        }
    }

    void PrintContinue()
    {
        //m_dialogueOptionHolder.SetActive(true);
        //m_dialogueOptionButtons[i].SetActive(true);
        //m_dialogueOptionButtons[i].transform.parent = m_dialogueObject.transform;
        m_dialogueOptionButtons[0].GetComponent<Text>().text = "Continue...";
        m_dialogueOptionButtons[0].GetComponent<Text>().color = m_activeText;
        m_dialogueOptionButtons[1].GetComponent<Text>().text = "";
        m_dialogueOptionButtons[2].GetComponent<Text>().text = "";
        //m_dialogueOptions++;
    }

    //Triggered by player
    void OnTriggerEnter2D(Collider2D m_col)
    {
        if (m_col.gameObject.name == "Player")
        {
            m_interactPrompt.SetActive(true);
            m_canTalk = true;
        }
    }

    //No longer triggered by player
    void OnTriggerExit2D(Collider2D m_col)
    {
        if (m_col.gameObject.name == "Player")
        {
            m_interactPrompt.SetActive(false);
            m_canTalk = false;
        }
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

    void FindFlag(string p_inFlag)
    {
        //Goes throug the list and finds the correct flag
        m_dialogueNum = (Array.IndexOf(m_dialogueLines, '[' + p_inFlag + ']') + 1);
    }

    void GiveItem(Item p_inItem)
    {
        m_player.GetComponent<InventorySystem>().AddItemToList(p_inItem);
    }

    void RefillPotions()
    {

    }

    public void NextDialogue()
    {
        m_dialogueCounter++;

        if (m_dialogueCounter < m_dialogueList.Count)
            m_dialoguePath = m_dialogueList[m_dialogueCounter];
        else
            m_dialoguePath = m_finalDialoguePath;
    }
}
