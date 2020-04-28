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
    [SerializeField]
    GameObject m_StartRoom;
    [SerializeField]
    GameObject m_EndRoom;

    protected int       m_sectionNumber         = 1;

    List<GameObject>    m_CurrentSectionRooms   = new List<GameObject>(); //Rooms in the current section
    List<GameObject>    m_NextSectionRooms      = new List<GameObject>(); //Rooms in the next section

    //TODO: Some way to increase rooms in future sections?
    //Exponential curve? Some other type of curve?
    //make sure that we only get certain amaounts of certain types of rooms?
    //Have the player "teleport" to the next section when reaching the end

    void Awake()
    {
        //Create Current and next section
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
        float m_tempXdif = 0;

        for (int i = 0; i <= p_inLength + 2; i++)
        {
            float currRoomYDiff = 0;
            float prevRoomYDiff = 0;
            float totalRoomYDiff = 0;

            if (i == 0)//If first room, instantiate start room
                p_inListDest.Add(Instantiate(m_StartRoom, new Vector2(i * m_tempXdif, m_sectionNumber * 100), Quaternion.identity));
            else if (i == p_inLength + 2) //If last room, instantiate end room
                p_inListDest.Add(Instantiate(m_EndRoom, new Vector2(i * m_tempXdif, m_sectionNumber * 100), Quaternion.identity));
            else //if neither, instantiate a random non-start non-end room
                p_inListDest.Add(Instantiate(p_inList[RandomNumber(0, m_RoomPrefabsList.Count - 1)], new Vector2(i * m_tempXdif, m_sectionNumber * 100), Quaternion.identity));

            if (i > 0)
            {
                currRoomYDiff -= p_inListDest[i].transform.Find("Entrance").transform.position.y; //Find difference in Y position between Entrance and center of the current room and invert
                prevRoomYDiff += p_inListDest[i - 1].transform.Find("Exit").transform.position.y; //Find difference in Y position between Exit and center of the previous room
                totalRoomYDiff = prevRoomYDiff + currRoomYDiff; //add both together
            }

            p_inListDest[i].transform.position = new Vector3(p_inListDest[i].transform.position.x, p_inListDest[i].transform.position.y + totalRoomYDiff, p_inListDest[i].transform.position.z); //Change room position depending on RoomYDiff
            m_tempXdif = p_inListDest[i].transform.Find("Sprites").GetComponent<SpriteRenderer>().size.x; //Change m_tempXdif to width of the newly insatntiated room

        }

        m_sectionNumber++; //Increase section number after instantiating a full section

       }

    //Function to get a random number 
    private static readonly System.Random random = new System.Random();
    private static readonly object syncLock = new object();
    public static int RandomNumber(int min, int max)
    {
        lock (syncLock)
        {
            return random.Next(min, max);
        }
    }
}