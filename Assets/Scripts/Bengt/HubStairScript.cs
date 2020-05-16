using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubStairScript : MonoBehaviour
{

    GameObject m_LevelGenerationManager;
    GameObject m_interactionPrompt;
    [SerializeField]
    GameObject m_player;


    bool m_playerIsByStairs;

    void Awake()
    {
        m_LevelGenerationManager = GameObject.Find("GameManager");
        m_interactionPrompt      = FindInActiveObjectByTag("InteractionPrompt");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && m_playerIsByStairs)
        {
            m_LevelGenerationManager.GetComponent<LevelGenerationManager>().ResetLevelGeneration();

            m_player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            m_playerIsByStairs = true;
            m_interactionPrompt.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            m_playerIsByStairs = false;
            m_interactionPrompt.SetActive(false);
        }
    }

    GameObject FindInActiveObjectByTag(string tag)
    {

        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].CompareTag(tag))
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }

}

