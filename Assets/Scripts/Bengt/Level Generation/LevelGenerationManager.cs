using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerationManager : MonoBehaviour
{
    [SerializeField]
    int                 m_sectionLength; //Length of a single section
    [SerializeField]
    List<GameObject>    m_RoomPrefabsList       = new List<GameObject>(); //Room Class? Store roomtype?

    protected int       m_sectionNumber         = 0;

    [SerializeField]
    List<GameObject>    m_CurrentSectionRooms   = new List<GameObject>(); //Rooms in the current section
    [SerializeField]
    List<GameObject>    m_NextSectionRooms      = new List<GameObject>(); //Rooms in the next section


    //TODO: Some way to increase rooms in future sections?
    //Exponential curve? Some other type of curve?


    //int m_nextsectionLength; ?
    //Rooms in Section
    //Rooms in next Section
    //Rooms per section?



    void Awake()
    {
        GenerateSection(m_sectionLength, m_RoomPrefabsList, m_CurrentSectionRooms);
        //GenerateSection(m_sectionLength, m_RoomPrefabsList, m_NextSectionRooms);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateSection(int p_inLength, List<GameObject> p_inList, List<GameObject> p_inListDest)
    {
        float m_tempXdif = 0;

        for(int i = 0; i <= p_inLength; i++)
        {
            float currRoomYDiff = 0;
            float prevRoomYDiff = 0;
            float totalRoomYDiff = 0;

            p_inListDest.Add(Instantiate(p_inList[RandRoom()], new Vector2(i*m_tempXdif, m_sectionNumber*20), Quaternion.identity));
            if (i >= 1)
            {
                currRoomYDiff -= p_inListDest[i].transform.Find("Entrance").transform.position.y;
                prevRoomYDiff += p_inListDest[i - 1].transform.Find("Exit").transform.position.y;

            }
            p_inListDest[i].transform.position = new Vector3(p_inListDest[i].transform.position.x, p_inListDest[i].transform.position.y + totalRoomYDiff, p_inListDest[i].transform.position.z); 
            m_tempXdif = p_inListDest[i].transform.Find("Sprites").GetComponent<SpriteRenderer>().size.x; 
        }
        m_sectionNumber++;
    }

    private int RandRoom()
    {
        System.Random m_rand = new System.Random();


        return m_rand.Next(0, m_RoomPrefabsList.Count);
    }
}