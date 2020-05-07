using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public enum PowerUp {
    SelfSoothing,
    CompulsiveRiskTaking,
    SelfHarm,
    Escape
}

public class PlayerBaseAbilities : MonoBehaviour {
    [SerializeField]
    float maxWillPower=100;
    [SerializeField]
    public float willPower;
    public PowerUp powerUp;
    public PlayerStats playerStats;
    public DataSet dataSet;

    public Image willPowerImage;
    public Text powerUpText;

    void Awake () {
        playerStats = GetComponent<PlayerStats>();
        playerStats.playerBaseAbilities = this;
        willPower = maxWillPower;

    }

    void Update () {
        willPowerImage.fillAmount =willPower / maxWillPower;
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.LeftAlt)) {
            willPower += 5;
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            SaveData();
        }
#endif
    }

    public void SetPowerUp (PowerUp index) {
        //powerUp = index;
        powerUpText.text = index.ToString();
        switch (index) {
            case PowerUp.SelfSoothing: {
                    if (willPower > 0) {
                        dataSet.numericalValues[4] += 1;
                        dataSet.numericalValues[0]++;
                        //we can change the values later on
                        willPower -= 1;
                        playerStats.SetPlayerStats(2, 1f, 2, 1);
                        playerStats.SetCountDown();
                        break;
                    }
                    else
                        break;
                }
            case PowerUp.CompulsiveRiskTaking: {
                    if (willPower > 2) {
                        dataSet.numericalValues[4] += 3;
                        dataSet.numericalValues[1] += 1;
                        willPower -= 3;
                        playerStats.SetPlayerStats(2, 2.5f, 2, 2);
                        playerStats.SetCountDown();
                        break;
                    }
                    else break;
                }

            case PowerUp.SelfHarm: {
                    if (willPower > 3) {
                        dataSet.numericalValues[4] += 4;
                        dataSet.numericalValues[2]++;
                        willPower -= 4;
                        playerStats.TakeDamage(playerStats.PlayerHealth() / 2, true);
                        playerStats.SetPlayerStats(1, 2.5f, 2.5f, 2f);
                        playerStats.SetCountDown();
                        break;
                    }
                    else
                        break;
                }
            case PowerUp.Escape: {
                    if (willPower > 4) {
                        dataSet.numericalValues[4] += 5;
                        dataSet.numericalValues[3]++;
                        willPower -= 5;
                        playerStats.SetCountDown();
                        break;
                    }
                    else
                        break;
                }
        }
#if UNITY_EDITOR
        playerStats.livesText.text = willPower.ToString();
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
        willPower -= 1;
        playerStats.PlayerHealth();
    }

    public float GetReserveLives () {
        return willPower;
    }
}