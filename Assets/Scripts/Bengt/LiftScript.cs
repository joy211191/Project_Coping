using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LiftScript : MonoBehaviour
{
    LevelGenerationManager m_LevelGenerationManager;
    GameObject m_interactionPrompt;
    GameObject m_costHeader;
    GameObject m_levelHeader;
    GameObject m_LiftUI;
    GameObject m_player;
    GameObject m_warning;
    GameObject m_Tutorial;

    Text m_costText;
    Text m_levelText;

    PlayerBaseAbilities m_playerBaseAbilities;

    bool m_playerIsByLift;

    protected int m_selectedFloor = 2;

    void Awake()
    {
        m_LevelGenerationManager = GameObject.Find("GameManager").GetComponent<LevelGenerationManager>();
        m_interactionPrompt = GameObject.FindGameObjectWithTag("InteractionPrompt");
        m_levelHeader = GameObject.Find("/Canvas/Lift UI/Panel/Level Panel/Level Header");
        m_levelText = GameObject.Find("/Canvas/Lift UI/Panel/Level Panel/Level Text").GetComponent<Text>();
        m_costHeader = GameObject.Find("/Canvas/Lift UI/Panel/Cost Panel/Cost Header");
        m_costText = GameObject.Find("/Canvas/Lift UI/Panel/Cost Panel/Cost Text").GetComponent<Text>();
        m_LiftUI = GameObject.Find("/Canvas/Lift UI");
        m_warning = GameObject.Find("/Canvas/Lift UI/Panel/Warning");
        m_Tutorial = GameObject.Find("/Canvas/Lift UI/Panel/Tutorial");

        m_player = GameObject.Find("Player");
        m_playerBaseAbilities = m_player.GetComponent<PlayerBaseAbilities>();

        m_interactionPrompt.SetActive(false);
        HideWarning();
        m_LiftUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && m_playerIsByLift &&
            m_LevelGenerationManager.m_currentSectionNumber > 0)
            ShowUI();
        else if (Input.GetKeyDown(KeyCode.Q) && m_playerIsByLift)
            Debug.Log("The lift is broken, take the stairs"); //TODO: Add something that shows that the elevator is broken and if the player cannot afford to take the lift

        //TODO: Maybe a tutorial as well?

        if (m_LiftUI.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                HideUI();
            else if (Input.GetKeyDown(KeyCode.A) && m_selectedFloor > 2)
            {
                m_selectedFloor--;
                m_levelText.text = m_selectedFloor.ToString();
                m_costText.text = GetWillpowerCost(m_selectedFloor - 1).ToString();
                if (m_playerBaseAbilities.willPower < GetWillpowerCost(m_selectedFloor - 1))
                    ShowWarning();
                else
                    HideWarning();
            }
            else if (Input.GetKeyDown(KeyCode.D) && m_selectedFloor < m_LevelGenerationManager.m_playerWillpowerChange.Count + 1)
            {

                m_selectedFloor++;
                m_levelText.text = m_selectedFloor.ToString();
                m_costText.text = GetWillpowerCost(m_selectedFloor - 1).ToString();
                if (m_playerBaseAbilities.willPower < GetWillpowerCost(m_selectedFloor - 1))
                    ShowWarning();
                else
                    HideWarning();
            }
            else if (Input.GetKeyDown(KeyCode.Space) && m_playerBaseAbilities.willPower >= GetWillpowerCost(m_selectedFloor - 1))
                UseLift();


        }

    }

    private float GetWillpowerCost(int inFloor)
    {
        float tempCost = 0;

        for (int i = 0; i < inFloor; i++)
            tempCost += m_LevelGenerationManager.m_playerWillpowerChange[i];

        return tempCost;
    }

    private void ShowUI()
    {
        m_interactionPrompt.SetActive(false);
        m_LiftUI.SetActive(true);
        m_Tutorial.SetActive(true);
        m_player.GetComponent<PlayerController>().enabled = false;
        m_player.GetComponent<PlayerAnimator>().enabled = false;
        m_levelText.text = m_selectedFloor.ToString();
        m_costText.text = GetWillpowerCost(m_selectedFloor - 1).ToString();

        if (m_playerBaseAbilities.willPower < GetWillpowerCost(m_selectedFloor - 1))
            ShowWarning();
    }

    private void HideUI()
    {
        m_interactionPrompt.SetActive(true);
        m_LiftUI.SetActive(false);
        m_Tutorial.SetActive(false);
        m_player.GetComponent<PlayerController>().enabled = true;
        m_player.GetComponent<PlayerAnimator>().enabled = true;
    }

    private void ShowWarning()
    {
        m_warning.SetActive(true);
    }
    private void HideWarning()
    {
        m_warning.SetActive(false);
    }

    private void UseLift()
    {
        HideUI();
        m_LevelGenerationManager.UseLift(m_selectedFloor);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            m_playerIsByLift = true;
            m_interactionPrompt.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
        {
            if (col.tag == "Player")
            {
                m_playerIsByLift = false;
                m_interactionPrompt.SetActive(false);
            }
    }
}
