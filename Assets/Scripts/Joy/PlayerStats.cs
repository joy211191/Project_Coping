﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {
    [SerializeField]
    float attackPower=10f;
    [SerializeField]
    float health = 100f;
    [SerializeField]
    float speed;
    [SerializeField]
    float damageMultiplier;
    PlayerAnimator playerAnimator;
    public bool playHurtAnim = true;
    public float countDown;
    public float maxCountDown;
    List<float> originalValues = new List<float>();
    public PlayerBaseAbilities playerBaseAbilities;
    [Space(5)]
    [Header("Numbness Pool Settings")]
    [SerializeField]
    [Range(0, 1)]
    float numbnessDamageReduction;
    [SerializeField]
    float numbnessPool;
    [SerializeField]
    float maxNumbnessPoolValue;
    [SerializeField]
    float numbnessPoolDelayTime;
    [SerializeField]
    float numbnessPoolDecayValue;

    //Debug
    public Text speedText, damageMultiplierText, healthText, attackPowerText, powerUpName, livesText;

    void Awake () {
        playerAnimator = GetComponent<PlayerAnimator>();
        SetPlayerStats(1, damageMultiplier, speed, 1);
    }

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

    public void TakeDamage (float damage,bool selfHarm=false) {
        StopCoroutine("NumbnessPoolDecay");
        if (!selfHarm) {
            if (numbnessPool < maxNumbnessPoolValue) {
                numbnessPool += damage / numbnessDamageReduction;
                health -= damage * damageMultiplier / (1 - numbnessDamageReduction);
            }
            else {
                health -= damage * damageMultiplier;
            }
        }
        if (playHurtAnim) {
            playerAnimator.m_animator.SetTrigger("Hurt");
        }
    }

    public IEnumerator NumbnessPoolDecay () {
        yield return new WaitForSeconds(numbnessPoolDelayTime);
        while (numbnessPool > 0)
            numbnessPool -= numbnessPoolDecayValue;
    }

    public float PlayerHealth () {
        return health;
    }

    public float SpeedChange () {
        return speed;
    }

    public void SetCountDown () {
        countDown = maxCountDown;
    }

    void Update () {

#if UNITY_EDITOR
        speedText.text = "Speed: " +speed.ToString();
        damageMultiplierText.text = "Damage Multiplier: "+damageMultiplier.ToString();
        healthText.text = "Health: "+health.ToString();
        attackPowerText.text = "AttackPower" +attackPower.ToString();
        powerUpName.text = playerBaseAbilities.powerUp.ToString();
#endif
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