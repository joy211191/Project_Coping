﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateLock : MonoBehaviour
{
    void Awake () {
        Application.targetFrameRate = 30;
    }
}
