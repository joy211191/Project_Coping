using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public Transform startPoint;
    public Animator animator;
    Transform playerTransform;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<Rigidbody2D>().simulated = false;
            playerTransform = other.transform;
            StartCoroutine("PositionChange");
        }
    }

    public IEnumerator PositionChange () {
        animator.SetTrigger("Transition");
        yield return new WaitForSecondsRealtime(0.5f);
        playerTransform.position = startPoint.position;
        playerTransform.GetComponent<Rigidbody2D>().simulated = true;
        //animator.SetTrigger("Transition");
    }
}
