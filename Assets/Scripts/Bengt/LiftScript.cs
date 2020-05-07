using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LiftScript : MonoBehaviour
{
    GameObject m_LevelGenerationManager;

    [SerializeField]
    GameObject m_interactionPrompt;

    [SerializeField]
    GameObject m_levelHeader;
    [SerializeField]
    Text m_levelText;
    [SerializeField]
    GameObject m_costHeader;
    [SerializeField]
    Text m_costText;
    [SerializeField]
    GameObject m_LiftUI;
    [SerializeField]
    GameObject m_player;

    bool m_playerIsByLift;

    protected int m_selectedFloor = 2;

    void Awake()
    {
        m_LevelGenerationManager    = GameObject.Find("GameManager");
        m_interactionPrompt         = GameObject.Find("/Canvas/Interaction Prompt");
        m_levelHeader               = GameObject.Find("/Canvas/Lift UI/Panel/Level Panel/Level Header");
        m_levelText                 = GameObject.Find("/Canvas/Lift UI/Panel/Level Panel/Level Text").GetComponent<Text>();
        m_costHeader                = GameObject.Find("/Canvas/Lift UI/Panel/Cost Panel/Cost Header");
        m_costText                  = GameObject.Find("/Canvas/Lift UI/Panel/Cost Panel/Cost Text").GetComponent<Text>();
        m_LiftUI                    = GameObject.Find("/Canvas/Lift UI");
        m_player                    = GameObject.Find("Player");

        m_interactionPrompt.SetActive(false);
        m_LiftUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && m_playerIsByLift &&
            m_LevelGenerationManager.GetComponent<LevelGenerationManager>().m_currentSectionNumber > 0)
            ShowUI();
        else if (Input.GetKeyDown(KeyCode.Q) && m_playerIsByLift)
            Debug.Log("The lift is broken, take the stairs");

        if(m_LiftUI.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                HideUI();
            else if (Input.GetKeyDown(KeyCode.A) && m_selectedFloor > 2)
            {
                m_selectedFloor--;
                m_levelText.text = m_selectedFloor.ToString();
                m_costText.text = GetWillpowerCost(m_selectedFloor - 1).ToString();
            }
            else if (Input.GetKeyDown(KeyCode.D) && m_selectedFloor < m_LevelGenerationManager.GetComponent<LevelGenerationManager>().m_playerWillpowerChange.Count + 1)
            {
                m_selectedFloor++;
                m_levelText.text = m_selectedFloor.ToString();
                m_costText.text = GetWillpowerCost(m_selectedFloor - 1).ToString();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
                UseLift();
        }

    }

    private float GetWillpowerCost(int inFloor)
    {
        float tempCost = 0;

        for (int i = 0; i < inFloor; i++)
            tempCost += m_LevelGenerationManager.GetComponent<LevelGenerationManager>().m_playerWillpowerChange[i];

        return tempCost;
    }

    private void ShowUI()
    {
        m_interactionPrompt.SetActive(false);
        m_LiftUI.SetActive(true);
        m_player.GetComponent<PlayerController>().enabled = false;
        m_player.GetComponent<PlayerAnimator>().enabled = false;
        m_levelText.text = m_selectedFloor.ToString();
        m_costText.text = GetWillpowerCost(m_selectedFloor - 1).ToString();

    }

    private void HideUI()
    {
        m_interactionPrompt.SetActive(true);
        m_LiftUI.SetActive(false);
        m_player.GetComponent<PlayerController>().enabled = true;
        m_player.GetComponent<PlayerAnimator>().enabled = true;
    }
    private void UseLift()
    {
        HideUI();
        m_LevelGenerationManager.GetComponent<LevelGenerationManager>().UseLift(m_selectedFloor);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        m_playerIsByLift = true;
        m_interactionPrompt.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        m_playerIsByLift = false;
        m_interactionPrompt.SetActive(false);
    }
}
