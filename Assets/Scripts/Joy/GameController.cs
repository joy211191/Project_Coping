using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    bool pause;
    public GameObject pauseScreen;
    PlayerBaseAbilities playerBaseAbilities;

    private void Awake () {
        playerBaseAbilities = FindObjectOfType<PlayerBaseAbilities>();
    }

    private void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            pause = !pause;
        }

        pauseScreen.SetActive(pause);
        if (pause) {
            
            Time.timeScale = 0;
        }
        else {
            Time.timeScale = 1;
        }
    }

    public void ApplicationExit (bool feedback) {
        playerBaseAbilities.SaveData();
        if (feedback)
            Application.OpenURL("https://docs.google.com/forms/d/187L3bbnpsSdI5ec6AISorBXrZlwrXXYog9XzHo1O7l4/edit");

        Application.Quit();
    }

}
