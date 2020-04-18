using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private static CheckpointManager instance;
    public Vector2 lastCheckpointPosition;
    public Transform playerTransform;

    void Awake () {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
            Destroy(gameObject);
    }

    void LoadCheckpoint () {
        playerTransform.position = lastCheckpointPosition;
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.L))
            LoadCheckpoint();
    }
}
