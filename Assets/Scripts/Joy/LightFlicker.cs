using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.U2D;

public class LightFlicker : MonoBehaviour {
    Light2D globalLight;
    void Awake () {
        globalLight = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void LateUpdate () {
        globalLight.intensity = Random.Range(0.8f, 1.1f);
    }
}