﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
    float attackPower=10f;
    float health = 100f;
    float speed;
    float damageMultiplier;
    PlayerAnimator playerAnimator;
    public bool playHurtAnim = true;
    public float countDown;
    public float maxCountDown;
    List<float> originalValues = new List<float>();

    public void SetPlayerStats (float healthMultiplier, float damageTaken, float speedChange, float attackMultiplier) {
        originalValues.Clear();
        originalValues.Add(health);
        originalValues.Add(damageMultiplier);
        originalValues.Add(speed);
        originalValues.Add(attackPower);
        attackPower *= attackMultiplier;
        health *= healthMultiplier;
        damageMultiplier = damageTaken;
        speed = speedChange;
        playerAnimator.GetSpeed();
    }

    public void TakeDamage (float damage) {
        health -= damage * damageMultiplier;
        if (playHurtAnim)
            playerAnimator.m_animator.SetTrigger("Hurt");
    }

    public float PlayerHealth () {
        return health;
    }

    public float SpeedChange () {
        return speed;
    }

    void Awake () {
        playerAnimator = GetComponent<PlayerAnimator>();
        SetPlayerStats(1, 1, 1.5f, 1);
    }

    public void SetCountDown () {
        countDown = maxCountDown;
    }

    void Update () {
        
        if (countDown > 0) {
            countDown -=Time.deltaTime;
            if (countDown <= 0) {
                playHurtAnim = true;
                SetPlayerStats(originalValues[0] / health, originalValues[1], originalValues[2], originalValues[3] / attackPower);
                originalValues.Clear();
            }
        }
    }
}