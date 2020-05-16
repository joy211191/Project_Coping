using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClimberSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject m_climber; //Climber
    [SerializeField]
    string m_climberName;


    // Start is called before the first frame update
    void Awake()
    {
        m_climber = GameObject.Find(m_climberName);
        m_climber.transform.position = transform.position;
        Debug.Log(transform.position);
    }

    private void Start()
    {
        m_climber.transform.position = transform.position;
    }

    private static readonly System.Random random = new System.Random();
    private static readonly object syncLock = new object();
    public static int RandomNumber(int min, int max)
    {
        lock (syncLock)
        {
            return random.Next(min, max);
        }
    }

    private void OnDestroy()
    {
        //Removing this will stop the errors but it will cause problems 
        //such as the NPC climbers not switching to the correct dialogue
        //or appearing where they shouldn't
        //¯\_(ツ)_/¯
        if (m_climber != null) {
            m_climber.transform.position = new Vector2(-50, -50);
            if (m_climberName == "Veteran")
                m_climber.GetComponent<VeteranScript>().NextDialogue();
            else if (m_climberName == "Storyteller")
                m_climber.GetComponent<StoryTellerScript>().NextDialogue();
        }
    }
}
