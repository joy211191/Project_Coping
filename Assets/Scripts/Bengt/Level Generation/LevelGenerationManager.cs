using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerationManager : MonoBehaviour
{
    [SerializeField]
    int                 m_sectionStartLength; //Length of the first section
    [SerializeField]
    int                 m_sectionsBeforeIncrease;
    [SerializeField]
    List<GameObject>    m_RoomPrefabsList       = new List<GameObject>(); //Room Class? Store roomtype?
    [SerializeField]
    GameObject          m_StartRoom;
    [SerializeField]
    GameObject          m_EndRoom;
    [SerializeField]
    GameObject          m_player;


    protected int       m_sectionNumber             = 1; //Current section of section group, ie the group of sections with an equal amount of rooms
    protected int       m_sectionLength             = 0; //Length of section
    public int          m_currentSectionNumber      = 0; //Current section overall

    [SerializeField]
    protected float     m_playerStartWillpower      = 0; //Player Willpower at start of section
    [SerializeField]
    protected float     m_playerEndWillpower        = 0; //Player willpower at end of section
    [SerializeField]
    protected float     m_tempPlayerWillpowerChange = 0;

    [SerializeField]
    public List<float>  m_playerWillpowerChange     = new List<float>(); //Player willpower change for each section, use this for cost-calculation


    [SerializeField]
    List<GameObject>    m_CurrentSectionRooms       = new List<GameObject>();   //Rooms in the current section
    List<GameObject>    m_NextSectionRooms          = new List<GameObject>();   //Rooms in the next section
    [SerializeField]
    GameObject          m_playerSpawn;                                      //The point where the player will spawn when they enter that section

    //protected bool      m_sectionHasBossRoom    = false; No bossrooms :(
    protected bool      m_sectionHasClimberRoom     = false;
    protected bool      m_sectionHasAmbushRoom      = false;


    void Awake()
    {
        m_sectionLength = m_sectionStartLength;

        ////Create Current and next section
        GenerateSection(m_sectionLength, m_RoomPrefabsList, m_CurrentSectionRooms);
        GenerateSection(m_sectionLength, m_RoomPrefabsList, m_NextSectionRooms);

        m_playerSpawn = m_CurrentSectionRooms[0].transform.Find("Player Spawn").gameObject;
    }

    //Generates a section of rooms
    private void GenerateSection(int p_inLength, List<GameObject> p_inList, List<GameObject> p_inListDest)
    {
        float m_tempXdif = 0;

        for (int i = 0; i <= p_inLength + 1; i++)
        {
            float currRoomYDiff = 0;
            float prevRoomYDiff = 0;
            float totalRoomYDiff = 0;

            if (i == 0)//If first room, instantiate start room
                p_inListDest.Add(Instantiate(m_StartRoom, new Vector2(m_tempXdif, m_sectionNumber * 100), Quaternion.identity));
            else if (i == p_inLength + 1) //If last room, instantiate end room
                p_inListDest.Add(Instantiate(m_EndRoom, new Vector2(m_tempXdif, m_sectionNumber * 100), Quaternion.identity));
            else //if neither, instantiate a random non-start non-end room
            {
                GameObject tempRoom = p_inList[RandomNumber(0, m_RoomPrefabsList.Count)];

                if (tempRoom.tag == "Ambush Room" && m_sectionHasAmbushRoom)
                    while (tempRoom.tag == "Ambush Room")
                        tempRoom = p_inList[RandomNumber(0, m_RoomPrefabsList.Count)];

                else if (tempRoom.tag == "Climber Room" && m_sectionHasClimberRoom)
                    while (tempRoom.tag == "Climber Room")
                        tempRoom = p_inList[RandomNumber(0, m_RoomPrefabsList.Count)];

                if (tempRoom.tag == "Ambush Room")
                    m_sectionHasAmbushRoom = true;
                else if (tempRoom.tag == "Climber Room")
                    m_sectionHasClimberRoom = true;

                p_inListDest.Add(Instantiate(tempRoom, new Vector2(m_tempXdif, m_sectionNumber * 100), Quaternion.identity));



            }

            if (i > 0)
            {
                currRoomYDiff -= p_inListDest[i].transform.Find("Entrance").transform.position.y; //Find difference in Y position between Entrance and center of the current room and invert
                prevRoomYDiff += p_inListDest[i - 1].transform.Find("Exit").transform.position.y; //Find difference in Y position between Exit and center of the previous room
                totalRoomYDiff = prevRoomYDiff + currRoomYDiff; //add both together
            }

			m_tempXdif += (p_inListDest[i].transform.Find("Grid").transform.Find("Tilemap").GetComponent<TilemapCollider2D>().bounds.size.x / 2); //Change m_tempXdif to width of the newly insatntiated room

			if (i > 0)
				m_tempXdif += (p_inListDest[i - 1].transform.Find("Grid").transform.Find("Tilemap").GetComponent<TilemapCollider2D>().bounds.size.x / 2);

			p_inListDest[i].transform.position = new Vector3(m_tempXdif, p_inListDest[i].transform.position.y + totalRoomYDiff, p_inListDest[i].transform.position.z); //Change room position depending on RoomYDiff
			
        }

        m_sectionNumber++; //Increase section number after instantiating a full section

        m_sectionHasClimberRoom = false;
        m_sectionHasAmbushRoom  = false;

        if (m_sectionNumber > m_sectionsBeforeIncrease)
        {
            m_sectionLength++;
            m_sectionNumber = 1;
        }

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

    public void TeleportPlayer()
    {
        m_playerSpawn = m_NextSectionRooms[0].transform.Find("Player Spawn").gameObject; //Change spawn/teleport location


        GenerateNewSection();

        m_player.transform.position = m_playerSpawn.transform.position; //Teleport player

        m_playerEndWillpower = m_player.GetComponent<PlayerBaseAbilities>().willPower; //Save the willpower the player had at the end of the level they just left

        m_tempPlayerWillpowerChange = m_playerStartWillpower - m_playerEndWillpower; //temporarily store the difference in willpower between the start and end of that level

        if (m_currentSectionNumber <= m_playerWillpowerChange.Count() && Math.Abs(m_playerWillpowerChange[m_currentSectionNumber - 1]) > Math.Abs(m_tempPlayerWillpowerChange)) //If the player has already completed that level and used less willpower
            m_playerWillpowerChange[m_currentSectionNumber - 1] = m_tempPlayerWillpowerChange; //Change that element of the list with the new willpower change
        else if (m_currentSectionNumber > m_playerWillpowerChange.Count())  //If the player hasn't finished this level before
            m_playerWillpowerChange.Add(m_tempPlayerWillpowerChange);


        m_playerStartWillpower = m_player.GetComponent<PlayerBaseAbilities>().willPower;    //Save start willpower for this section

        m_currentSectionNumber++;   //Increment section
    }

    public void UseLift(int p_inFloor)
    {

        for (int i = 0; i < p_inFloor - 1; i++)
            m_player.GetComponent<PlayerBaseAbilities>().willPower -= m_playerWillpowerChange[i];


        m_playerStartWillpower = m_player.GetComponent<PlayerBaseAbilities>().willPower;

        Debug.Log("Hello there");

        m_currentSectionNumber = p_inFloor;

        double temp = Convert.ToDouble(p_inFloor) / Convert.ToDouble(m_sectionsBeforeIncrease);

        m_sectionLength = m_sectionStartLength + (Convert.ToInt32(Math.Ceiling(temp)) - 1);

        if(temp - Math.Floor(temp) < 0.5)
            m_sectionNumber = 1;
        else if (temp - Math.Floor(temp) < 1)
            m_sectionNumber = 2;
        else if (temp - Math.Floor(temp) == 1)
            m_sectionNumber = 3;

        for (int i = 0; i < m_CurrentSectionRooms.Count; i++)
            Destroy(m_CurrentSectionRooms[i]);

        m_CurrentSectionRooms.Clear();

        for (int i = 0; i < m_NextSectionRooms.Count; i++)  
            Destroy(m_NextSectionRooms[i]);

        m_NextSectionRooms.Clear();

        GenerateSection(m_sectionLength, m_RoomPrefabsList, m_CurrentSectionRooms);

        GenerateSection(m_sectionLength, m_RoomPrefabsList, m_NextSectionRooms);

        m_playerSpawn = m_CurrentSectionRooms[0].transform.Find("Player Spawn").gameObject;
        m_player.transform.position = m_playerSpawn.transform.position;
    }

    private void GenerateNewSection()
    {
        //Replace current room list with new room list
        for (int i = 0; i < m_CurrentSectionRooms.Count; i++)
            Destroy(m_CurrentSectionRooms[i]);

        m_CurrentSectionRooms.Clear();

        for (int i = 0; i < m_NextSectionRooms.Count; i++)
            m_CurrentSectionRooms.Add(m_NextSectionRooms[i]);

        m_NextSectionRooms.Clear();

        GenerateSection(m_sectionLength, m_RoomPrefabsList, m_NextSectionRooms);

    }

    public void ResetLevelGeneration()
    {
        m_sectionLength             = m_sectionStartLength;
        m_sectionNumber             = 1;
        m_currentSectionNumber      = 1;

        m_player.GetComponent<PlayerBaseAbilities>().willPower = 0;
        m_playerStartWillpower  = m_player.GetComponent<PlayerBaseAbilities>().willPower;
        m_playerEndWillpower    = 0;

        for (int i = 0; i < m_CurrentSectionRooms.Count; i++)
            Destroy(m_CurrentSectionRooms[i]);

        for (int i = 0; i < m_NextSectionRooms.Count; i++)
            Destroy(m_NextSectionRooms[i]);

        m_CurrentSectionRooms.Clear();
        m_NextSectionRooms.Clear();

        GenerateSection(m_sectionLength, m_RoomPrefabsList, m_CurrentSectionRooms);
        GenerateSection(m_sectionLength, m_RoomPrefabsList, m_NextSectionRooms);

        m_playerSpawn               = m_CurrentSectionRooms[0].transform.Find("Player Spawn").gameObject;
        m_player.transform.position = m_playerSpawn.transform.position;
    }
}