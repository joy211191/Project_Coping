using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.U2D;

public class LightFlicker : MonoBehaviour {
    Light2D lightComponent;
    public Vector2 minMaxIntensity;
    //public Vector2 minMaxFalloff;
    void Awake () {
        lightComponent = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void LateUpdate () {
        lightComponent.intensity = Random.Range(minMaxIntensity.x, minMaxIntensity.y);
        //lightComponent. = Random.Range(minMaxFalloff.x, minMaxFalloff.y);
    }
}