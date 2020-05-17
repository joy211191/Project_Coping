using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public enum PowerUp {
    CompulsionRage,
    Numbing,
    Escape
}

public class PlayerBaseAbilities : MonoBehaviour {
    [SerializeField]
    public float maxWillPower=100;
    [SerializeField]
    public float willPower;
    public PowerUp powerUp;
    public PlayerStats playerStats;
    public DataSet dataSet;
    PlayerAnimator playerAnimator;

    public List<Sprite> powerUpSprites = new List<Sprite>();

    public Image willPowerImage;
    public Image powerUpImage;

    void Start () {
        playerStats = GetComponent<PlayerStats>();
        playerStats.playerBaseAbilities = this;
        willPower = maxWillPower;
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    void Update () {
        willPowerImage.fillAmount =willPower / maxWillPower;
#if UNITY_EDITOR
        /*if (Input.GetKeyDown(KeyCode.LeftAlt) && willPower >= 5) {
            willPower -= 5f;
        }*/
        if (Input.GetKeyDown(KeyCode.P)) {
            SaveData();
        }
#endif
    }

    public void IncreaseWillPower(float willpowerIncrementValue) {
        maxWillPower += willpowerIncrementValue;
    }

    public void SetPowerUp (PowerUp index) {
        //powerUp = index;
        powerUpImage.GetComponent<Animator>().SetBool("Activate", true);
        switch (index) {
            case PowerUp.CompulsionRage: {
                    if (willPower > 0) {
                        dataSet.numericalValues[3] += 7;
                        dataSet.numericalValues[0]++;
                        //we can change the values later on 
                        willPower -= 7;
                        playerStats.SetPlayerStats(2, 1f, 10, 2); //Increases damage, increases speed by 50%, 4x Cost 7 Willpower, 15seconds
                        playerStats.SetCountDown(15f);
                        break;
                    }
                    else
                        break;
                }
            /*case PowerUp.Risk: {
                    if (willPower > 2) {
                        dataSet.numericalValues[4] += 3;
                        dataSet.numericalValues[1] += 1;
                        willPower -= 3;
                        playerStats.SetPlayerStats(2, 2.5f, 2, 2); //No design remove??
                        playerStats.SetCountDown();
                        break;
                    }
                    else break;
                }*/

            case PowerUp.Numbing: {
                    if (willPower > 3) {
                        dataSet.numericalValues[3] += 5;
                        dataSet.numericalValues[1]++;
                        willPower -= 5;
                        playerStats.TakeDamage(playerStats.PlayerHealth() / 2, true);
                        playerStats.SetPlayerStats(2, 0.2f, 1, 1f,20); //Increase health, decresase damage taken, Doubles Numbness pool and % - Cost 5 Willpower, 20 seconds    
                        playerStats.SetCountDown(20f);
                        break;
                    }
                    else
                        break;
                }
            case PowerUp.Escape: {
                    if (willPower > 4) {
                        dataSet.numericalValues[3] += 15;
                        dataSet.numericalValues[2]++;
                        willPower -= 15;
                        playerAnimator.EscapeMechanicUpdate(true);//enables double dash, double jump and halves dash timer, Cost 15 Willpower, 8 Seconds
                        playerStats.SetCountDown(8);
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
        if (File.Exists(Application.dataPath + " / GameplayData.json")) {
            string jsonstring = JsonUtility.ToJson(dataSet);
            File.WriteAllText(Application.dataPath + "/GameplayData.json", jsonstring);
        }
        else {
            FileStream fileStream = new FileStream(Application.dataPath + "/GameplayData.json", FileMode.Create);
            fileStream.Close();
            string jsonstring = JsonUtility.ToJson(dataSet);
            File.WriteAllText(Application.dataPath + "/GameplayData.json", jsonstring);
        }
    }

    //Revive Mechanic
    //public void Revive () {
    //    willPower -= 1;
    //    playerStats.PlayerHealth();
    //}

    public float GetReserveLives () {
        return willPower;
    }
}