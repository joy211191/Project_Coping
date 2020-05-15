using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {
    public float attackPower = 10f;
    public float maxHealth = 100;
    public float health;
    public float speed;
    public float damageMultiplier;
    PlayerAnimator playerAnimator;
    public bool playHurtAnim = true;
    public float countDown;
    public float maxCountDown;
    List<float> originalValues = new List<float>();
    public PlayerBaseAbilities playerBaseAbilities;
    [Space(5)]
    [Header("Numbness Pool Settings")]
    [SerializeField]
    [Range(0f, 1f)]
    float numbnessDamageReduction;
    [SerializeField]
    float numbnessPool;
    [SerializeField]
    float maxNumbnessPoolValue;
    [SerializeField]
    float numbnessPoolDelayTime;
    [SerializeField]
    float numbnessPoolDecayValue;

    public bool powerActivated;

    [SerializeField]
    int maxPotions;
    public int potionCounter;

    [Space(20)]
    [Header("UI")]
    public Image healthImage;
    public Image numbnessImage;
    public Text potionsText;

    public List<Item> equippedItems = new List<Item>();

    bool refillNumbness;

    //Debug
    public Text speedText, damageMultiplierText, healthText, attackPowerText, powerUpName, livesText;

    void Awake () {
        potionCounter = maxPotions;
        potionsText.text = potionCounter.ToString();
        playerAnimator = GetComponent<PlayerAnimator>();
        numbnessPool = maxNumbnessPoolValue;
        SetPlayerStats(1, damageMultiplier, speed, 1);
        ItemEffects();
    }

    public void ItemEffects () {
        for (int i = 0; i < equippedItems.Count; i++) {
            maxHealth += equippedItems[i].healthIncrase;
            speed += equippedItems[i].movementSpeedIncrease;
            numbnessDamageReduction += equippedItems[i].numbnessDamagePercentage;
            numbnessPool += equippedItems[i].numbnessPoolIncrease;
            playerBaseAbilities.IncreaseWillPower(equippedItems[i].willpowerIncrease);
            playerAnimator.doubleDash = playerAnimator.doubleDash || equippedItems[i].doubleDash;
            playerAnimator.doubleJump = playerAnimator.doubleJump || equippedItems[i].doubleJump;
            playerAnimator.SetAbilities();
        }
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

    public void TakeDamage (float damage, bool selfHarm = false) {
        playerBaseAbilities.dataSet.numericalValues[5]++;
        StopCoroutine("NumbnessPoolDecay");
        refillNumbness = false;
        float decrementValue = damage * numbnessDamageReduction;
        if (!selfHarm) {
            if (numbnessPool >0) {
                numbnessPool -= decrementValue;
                float tempDamage = 1f - numbnessDamageReduction;
                health -= (damage * damageMultiplier) * tempDamage;
            }
            else if(numbnessPool< damage / numbnessDamageReduction) {
                float tempValue = numbnessPool;
                numbnessPool = 0;
                float tempDamage = 1f - numbnessDamageReduction;
                health -= (damage * damageMultiplier) * tempDamage;
                health -= decrementValue - tempValue;
            }
            else {
                health -= damage * damageMultiplier;
            }
        }
        if (playHurtAnim) {
            playerAnimator.m_animator.SetTrigger("Hurt");
        }
        StartCoroutine("NumbnessPoolDecay");
    }

    public IEnumerator NumbnessPoolDecay () {
        yield return new WaitForSeconds(numbnessPoolDelayTime);
        refillNumbness = true;
    }

    public void HealPlayer () {
        potionCounter--;
        health = maxHealth;
        potionsText.text = potionCounter.ToString();
    }

    public float PlayerHealth () {
        return health;
    }

    public float SpeedChange () {
        return speed;
    }

    public void SetCountDown () {
        powerActivated = true;
        countDown = maxCountDown;
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.K))
            ResetPlayer();

        numbnessImage.fillAmount = numbnessPool / maxNumbnessPoolValue;
        healthImage.fillAmount = health / maxHealth;
        if (refillNumbness&&numbnessPool<maxNumbnessPoolValue) {
            numbnessPool += numbnessPoolDecayValue;
        }
#if UNITY_EDITOR
        speedText.text = "Speed: " + speed.ToString();
        damageMultiplierText.text = "Damage Multiplier: " + damageMultiplier.ToString();
        healthText.text = "Health: " + health.ToString();
        attackPowerText.text = "AttackPower" + attackPower.ToString();
        powerUpName.text = playerBaseAbilities.powerUp.ToString();
        if (Input.GetKeyDown(KeyCode.H)) {
            TakeDamage(Random.Range(1f, 10f));
        }

#else
        speedText.text = "";
        damageMultiplierText.text = "";
        healthText.text = "";
        attackPowerText.text = "";
        powerUpName.text = "";
#endif
        if (countDown > 0) {
            countDown -= Time.deltaTime;
            if (countDown <= 0) {
                powerActivated = false;
                playHurtAnim = true;
                SetPlayerStats(originalValues[0] / health, originalValues[1], originalValues[2], originalValues[3] / attackPower);
                originalValues.Clear();
                playerBaseAbilities.powerUpImage.GetComponent<Animator>().SetBool("Activate", false);
            }
        }
    }

    private void ResetPlayer()
    {
        health = maxHealth;
        GetComponent<PlayerBaseAbilities>().willPower = GetComponent<PlayerBaseAbilities>().maxWillPower;
        gameObject.transform.position = GameObject.Find("Hub Spawn Point").transform.position;
    }

    public void refillPotions()
    {
        potionCounter = maxPotions;
        potionsText.text = potionCounter.ToString();
    }
}