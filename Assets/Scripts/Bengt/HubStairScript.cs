using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubStairScript : MonoBehaviour
{

    GameObject m_LevelGenerationManager;

    bool m_playerIsByStairs;

    void Awake()
    {
        m_LevelGenerationManager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && m_playerIsByStairs)
            m_LevelGenerationManager.GetComponent<LevelGenerationManager>().ResetLevelGeneration();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        m_playerIsByStairs = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        m_playerIsByStairs = false;
    }

}

