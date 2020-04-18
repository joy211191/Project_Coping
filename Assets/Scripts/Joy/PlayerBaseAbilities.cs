using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public enum PowerUp {
    SelfSoothing,
    Numbing,
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
    }

    public void SetPowerUp (PowerUp index) {
        switch (index) {
            case PowerUp.SelfSoothing: {
                    dataSet.livesUsed += 1;
                    dataSet.selfSoothing++;
                    reserveLives -= 1;
                    playerStats.SetPlayerStats(2, 1f, 1, 1);
                    playerStats.SetCountDown();
                    break;
                }
            case PowerUp.Numbing: {
                    dataSet.livesUsed += 1;
                    dataSet.numbing += 1;
                    reserveLives -= 1;
                    playerStats.playHurtAnim = false;
                    playerStats.SetCountDown();
                    break;
                }
            case PowerUp.CompulsiveRiskTaking: {
                    dataSet.livesUsed += 3;
                    dataSet.compulsiveRiskTaking += 1;
                    reserveLives -= 3;
                    playerStats.SetPlayerStats(2, 2.5f, 1, 2);
                    playerStats.SetCountDown();
                    break;
                }

            case PowerUp.SelfHarm: {
                    dataSet.livesUsed += 4;
                    dataSet.selfHarm++;
                    reserveLives -= 4;
                    playerStats.TakeDamage(playerStats.PlayerHealth() / 2);
                    playerStats.SetPlayerStats(1, 2.5f, 2.5f, 2f);
                    playerStats.SetCountDown();
                    break;
                }
            case PowerUp.Escape: {
                    dataSet.livesUsed += 5;
                    dataSet.escape++;
                    reserveLives -= 5;
                    playerStats.SetCountDown();
                    break;
                }
        }
#if UNITY_EDITOR
        playerStats.livesText.text = reserveLives.ToString();
#endif
    }
    public void SaveData () {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + " / GameData.Group3")) {
            FileStream fileStream = new FileStream(Application.persistentDataPath + "/GameData.Group3", FileMode.Open);
            binaryFormatter.Serialize(fileStream, dataSet.selfSoothing);
            binaryFormatter.Serialize(fileStream, dataSet.numbing);
            binaryFormatter.Serialize(fileStream, dataSet.compulsiveRiskTaking);
            binaryFormatter.Serialize(fileStream, dataSet.selfHarm);
            binaryFormatter.Serialize(fileStream, dataSet.escape);
            fileStream.Close();
        }
        else {
            FileStream fileStream = new FileStream(Application.persistentDataPath + "/GameData.Group3", FileMode.Create);
            binaryFormatter.Serialize(fileStream, dataSet.selfSoothing);
            binaryFormatter.Serialize(fileStream, dataSet.numbing);
            binaryFormatter.Serialize(fileStream, dataSet.compulsiveRiskTaking);
            binaryFormatter.Serialize(fileStream, dataSet.selfHarm);
            binaryFormatter.Serialize(fileStream, dataSet.escape);
            fileStream.Close();
        }
    }
}