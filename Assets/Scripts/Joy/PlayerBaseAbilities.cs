﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum PowerUp {
    SelfSoothing,
    CompulsiveRiskTaking,
    SelfHarm,
    Escape
}

public class PlayerBaseAbilities : MonoBehaviour {
    [SerializeField]
    int reserveLives;
    public PowerUp powerUp;
    public PlayerStats playerStats;
    public DataSet dataSet;

    void Awake () {
        playerStats = GetComponent<PlayerStats>();
        playerStats.playerBaseAbilities = this;
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.LeftAlt)) {
            reserveLives += 5;
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            SaveData();
        }
    }

    public void SetPowerUp (PowerUp index) {
        powerUp = index;
        switch (index) {
            case PowerUp.SelfSoothing: {
                    if (reserveLives > 0) {
                        dataSet.livesUsed += 1;
                        dataSet.selfSoothing++;
                        reserveLives -= 1;
                        playerStats.SetPlayerStats(2, 1f, 1, 1);
                        playerStats.SetCountDown();
                        break;
                    }
                    else
                        break;
                }
            case PowerUp.CompulsiveRiskTaking: {
                    if (reserveLives > 2) {
                        dataSet.livesUsed += 3;
                        dataSet.compulsiveRiskTaking += 1;
                        reserveLives -= 3;
                        playerStats.SetPlayerStats(2, 2.5f, 1, 2);
                        playerStats.SetCountDown();
                        break;
                    }
                    else break;
                }

            case PowerUp.SelfHarm: {
                    if (reserveLives > 3) {
                        dataSet.livesUsed += 4;
                        dataSet.selfHarm++;
                        reserveLives -= 4;
                        playerStats.TakeDamage(playerStats.PlayerHealth() / 2, true);
                        playerStats.SetPlayerStats(1, 2.5f, 2.5f, 2f);
                        playerStats.SetCountDown();
                        break;
                    }
                    else
                        break;
                }
            case PowerUp.Escape: {
                    if (reserveLives > 4) {
                        dataSet.livesUsed += 5;
                        dataSet.escape++;
                        reserveLives -= 5;
                        playerStats.SetCountDown();
                        break;
                    }
                    else
                        break;
                }
        }
#if UNITY_EDITOR
        playerStats.livesText.text = reserveLives.ToString();
#endif
    }
    public void SaveData () {
        if (File.Exists(Application.persistentDataPath + " / GameplayData.json")) {
            string jsonstring = JsonUtility.ToJson(dataSet);
            File.WriteAllText(Application.persistentDataPath + "/GameplayData.json", jsonstring);
        }
        else {
            FileStream fileStream = new FileStream(Application.persistentDataPath + "/GameplayData.json", FileMode.Create);
            fileStream.Close();
            string jsonstring = JsonUtility.ToJson(dataSet);
            File.WriteAllText(Application.persistentDataPath + "/GameplayData.json", jsonstring);
        }
    }

    //Revive Mechanic
    public void Revive () {
        reserveLives -= 1;
        playerStats.PlayerHealth();
    }

    public int GetReserveLives () {
        return reserveLives;
    }
}