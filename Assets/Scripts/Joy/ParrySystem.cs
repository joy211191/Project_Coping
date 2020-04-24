using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrySystem : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col) {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            //play enemy stun animation
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            //play attack cancel on enemy
        }
    }
}
