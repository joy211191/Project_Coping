using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrySystem : MonoBehaviour
{

    BoxCollider2D boxCollider2D;
    PlayerAnimator playerAnimator;


    void Awake () {
        boxCollider2D = GetComponent<BoxCollider2D>();
        playerAnimator = GetComponentInParent<PlayerAnimator>();
    }

    void Update () {
        boxCollider2D.enabled= Input.GetKey(KeyCode.R);
        playerAnimator.m_animator.SetBool("Block",Input.GetKey(KeyCode.LeftShift));
    }


    void OnTriggerEnter2D(Collider2D col) {
        if (Input.GetKeyDown(KeyCode.R)) {
            col.GetComponent<Animator>().SetTrigger("Damage");
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            //play attack cancel on enemy
        }
    }
}
