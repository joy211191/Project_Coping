using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                    reserveLives -= 1;
                    playerStats.SetPlayerStats(2, 1f, 1, 1);
                    playerStats.SetCountDown();
                    break;
                }
            case PowerUp.Numbing: {
                    reserveLives -= 1;
                    playerStats.playHurtAnim = false;
                    playerStats.SetCountDown();
                    break;
                }
            case PowerUp.CompulsiveRiskTaking: {
                    reserveLives -= 3;
                    playerStats.SetPlayerStats(2, 2.5f, 1, 2);
                    playerStats.SetCountDown();
                    break;
                }

            case PowerUp.SelfHarm: {
                    reserveLives -= 4;
                    playerStats.TakeDamage(playerStats.PlayerHealth() / 2);
                    playerStats.SetPlayerStats(1, 2.5f, 2.5f, 2f);
                    playerStats.SetCountDown();
                    break;
                }
            case PowerUp.Escape: {
                    reserveLives -= 5;
                    playerStats.SetCountDown();
                    break;
                }
        }
#if UNITY_EDITOR
        playerStats.livesText.text = reserveLives.ToString();
#endif
    }
}