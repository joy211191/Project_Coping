using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairScript : MonoBehaviour
{

    GameObject  m_LevelGenerationManager;
    GameObject  m_player;

    bool        m_playerIsByStairs;


    void Awake()
    {
        m_LevelGenerationManager    = GameObject.Find("GameManager");
        m_player                    = GameObject.Find("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Q) && m_playerIsByStairs &&
            m_player.GetComponent<Rigidbody2D>().velocity == new Vector2(0, 0))
            m_LevelGenerationManager.GetComponent<LevelGenerationManager>().TeleportPlayer();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
                m_playerIsByStairs = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
            m_playerIsByStairs = false;
    }

}
