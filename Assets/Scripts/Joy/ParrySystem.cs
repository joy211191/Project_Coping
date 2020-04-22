using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrySystem : MonoBehaviour {
    void OnCollisionEnter2D (Collision collision) {
        collision.gameObject.GetComponent<Animator>().SetTrigger("AttackCancel");
    }

    void OnTriggerEnter2D (Collider2D other) {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            other.GetComponent<Animator>().SetTrigger("Stun");
        }
    }
}