using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerationManager : MonoBehaviour
{
    [SerializeField]
    int                 m_sectionLength; //Length of a single section
    [SerializeField]
    List<GameObject>    m_RoomPrefabsList       = new List<GameObject>(); //Room Class? Store roomtype?

    protected int       m_sectionNumber         = 1;


    List<GameObject>    m_CurrentSectionRooms   = new List<GameObject>(); //Rooms in the current section
    List<GameObject>    m_NextSectionRooms      = new List<GameObject>(); //Rooms in the next section

    [SerializeField]
    GameObject m_StartRoom;
    [SerializeField]
    GameObject m_EndRoom;



    //TODO: Some way to increase rooms in future sections?
    //Exponential curve? Some other type of curve?
    //make sure that we only get certain amaounts of certain types of rooms?


    //int m_nextsectionLength; ?
    //Rooms in Section
    //Rooms in next Section
    //Rooms per section?



    void Awake()
    {
        GenerateSection(m_sectionLength, m_RoomPrefabsList, m_CurrentSectionRooms);
        GenerateSection(m_sectionLength, m_RoomPrefabsList, m_NextSectionRooms);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //Generates a section of rooms
    private void GenerateSection(int p_inLength, List<GameObject> p_inList, List<GameObject> p_inListDest)
    {
        //Add Start room
        //p_inListDest.Add(Instantiate(m_StartRoom, new Vector2(-m_StartRoom.transform.Find("Sprites").GetComponent<SpriteRenderer>().size.x, 50), Quaternion.identity));

        float m_tempXdif = 0;

        

        for (int i = 0; i <= p_inLength + 2; i++)
        {
            float currRoomYDiff = 0;
            float prevRoomYDiff = 0;
            float totalRoomYDiff = 0;

            if (i == 0)
                p_inListDest.Add(Instantiate(m_StartRoom, new Vector2(i * m_tempXdif, m_sectionNumber * 100), Quaternion.identity));
            else if (i == p_inLength + 2)
                p_inListDest.Add(Instantiate(m_EndRoom, new Vector2(i * m_tempXdif, m_sectionNumber * 100), Quaternion.identity));
            else
                p_inListDest.Add(Instantiate(p_inList[RandomNumber(0, m_RoomPrefabsList.Count - 1)], new Vector2(i * m_tempXdif, m_sectionNumber * 100), Quaternion.identity));


            if (i > 0)
            {
                currRoomYDiff -= p_inListDest[i].transform.Find("Entrance").transform.position.y;
                prevRoomYDiff += p_inListDest[i - 1].transform.Find("Exit").transform.position.y;
                totalRoomYDiff = prevRoomYDiff + currRoomYDiff;
            }

            p_inListDest[i].transform.position = new Vector3(p_inListDest[i].transform.position.x, p_inListDest[i].transform.position.y + totalRoomYDiff, p_inListDest[i].transform.position.z); 
            m_tempXdif = p_inListDest[i].transform.Find("Sprites").GetComponent<SpriteRenderer>().size.x;

        }
        m_sectionNumber++;

        //currRoomYDiff -= p_inListDest[p_inListDest.Count - 1].transform.Find("Entrance").transform.position.y;
        //prevRoomYDiff += p_inListDest[p_inListDest.Count - 2].transform.Find("Exit").transform.position.y;

        //p_inListDest.Add(Instantiate(m_EndRoom, new Vector2(p_inListDest.Count * m_tempXdif, 50), Quaternion.identity));
        //p_inListDest[p_inListDest.Count - 1].transform.position = new Vector3(p_inListDest[p_inListDest.Count - 1].transform.position.x, p_inListDest[p_inListDest.Count - 1].transform.position.y + totalRoomYDiff, p_inListDest[p_inListDest.Count - 1].transform.position.z);
    }


    //Old Funtion that did'nt work properly
    //private int RandRoom()
    //{
    //    System.Random m_rand = new System.Random();


    //    return m_rand.Next(0, m_RoomPrefabsList.Count - 1);
    //}

    //Function to get a random number 
    private static readonly System.Random random = new System.Random();
    private static readonly object syncLock = new object();
    public static int RandomNumber(int min, int max)
    {
        lock (syncLock)
        { // synchronize
            return random.Next(min, max);
        }
    }
}