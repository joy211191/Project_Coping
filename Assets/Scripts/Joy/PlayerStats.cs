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
    //public float maxCountDown;
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
    List<float> originalvaluesList = new List<float>();

    public Image countdownTimerImage;
    public float maxCountdownValue;

    //Debug
    public Text speedText, damageMultiplierText, healthText, attackPowerText, powerUpName, livesText;

    void Awake () {
        potionCounter = maxPotions;
        potionsText.text = potionCounter.ToString();
        playerAnimator = GetComponent<PlayerAnimator>();
        numbnessPool = 0;
        SetPlayerStats(1, damageMultiplier, speed, 1);
        ItemEffects();
    }

    public void ItemEffects () {
        originalvaluesList.Add(maxHealth);
        originalvaluesList.Add(speed);
        originalvaluesList.Add(numbnessDamageReduction);
        originalvaluesList.Add(numbnessPool);
        originalvaluesList.Add(playerBaseAbilities.maxWillPower);
        for (int i = 0; i < equippedItems.Count; i++) {
            maxHealth += equippedItems[i].healthIncrase;
            speed += equippedItems[i].movementSpeedIncrease;
            numbnessDamageReduction += equippedItems[i].numbnessDamagePercentage;
            maxNumbnessPoolValue += equippedItems[i].numbnessPoolIncrease;
            playerBaseAbilities.IncreaseWillPower(equippedItems[i].willpowerIncrease);
            //playerAnimator.doubleDash = playerAnimator.doubleDash || 
            //playerAnimator.doubleJump = playerAnimator.doubleJump || equippedItems[i].doubleJump;
            playerAnimator.SetAbilities();
        }
    }

    public void ResetEffects () {
        maxHealth = originalvaluesList[0];
        speed = originalvaluesList[1];
        numbnessDamageReduction = originalvaluesList[2];
        numbnessPool = originalvaluesList[3];
        playerBaseAbilities.maxWillPower = originalvaluesList[4];
        playerAnimator.doubleDash = false;
        playerAnimator.doubleJump = false;
    }

    public void SetPlayerStats (float healthMultiplier, float damageTaken, float speedChange, float attackMultiplier,float numbnessPoolIncrease=0) {
        originalValues.Clear();
        originalValues.Add(health);
        originalValues.Add(damageMultiplier);
        originalValues.Add(speed);
        originalValues.Add(attackPower);
        attackPower *= attackMultiplier;
        health *= healthMultiplier;
        damageMultiplier *= damageTaken;
        speed = speedChange;
        maxNumbnessPoolValue += numbnessPoolIncrease;
        playerAnimator.GetSpeed();
    }

    public void TakeDamage (float damage, bool selfHarm = false) {
        playerBaseAbilities.dataSet.numericalValues[4]++;
        StopCoroutine("NumbnessPoolDecay");
        refillNumbness = false;
        float incrementValue = damage * numbnessDamageReduction;
        if (!selfHarm) {
            if (numbnessPool <maxNumbnessPoolValue) {
                numbnessPool += incrementValue;
                float tempDamage = 1f - numbnessDamageReduction;
                health -= (damage * damageMultiplier) * tempDamage;
            }
            else if(damage / numbnessDamageReduction>(maxNumbnessPoolValue-numbnessPool) ) {
                float tempValue = maxNumbnessPoolValue - numbnessPool;
                numbnessPool = 100;
                float tempDamage = 1f - numbnessDamageReduction;
                health -= (damage * damageMultiplier) * tempDamage;
                health -= incrementValue - tempValue;
            }
            else {
                health -= damage * damageMultiplier;
            }
        }
        if (playHurtAnim) {
            playerAnimator.m_animator.SetTrigger("Hurt");
        }
        if (health <= 0) {
            Respeawn();
        }
        StartCoroutine("NumbnessPoolDecay");
    }

    public void Respeawn () {
        if (playerBaseAbilities.willPower > 10) {
            playerBaseAbilities.willPower -= 10;
            playerAnimator.m_animator.enabled = false;
            playerAnimator.m_animator.enabled = true;

        }
        else {
            playerAnimator.dead = true;
            playerAnimator.m_animator.SetTrigger("Death");
        }
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

    public void SetCountDown (float maxCountDown) {
        maxCountdownValue = maxCountDown;
        powerActivated = true;
        countDown = maxCountDown;
    }

    void Update () {
        if (health<0) {
            if (playerBaseAbilities.willPower > 10) {
                playerBaseAbilities.dataSet.numericalValues[3] += 10;
                playerBaseAbilities.dataSet.numericalValues[5]++;
                health = maxHealth;
                playerAnimator.m_animator.SetTrigger("Heal");
                SetPlayerStats(originalValues[0] / health, originalValues[1], originalValues[2], originalValues[3] / attackPower);
                //originalValues.Clear();
            }
            else {
                ResetPlayer();
            }
        }

        numbnessImage.fillAmount = numbnessPool / maxNumbnessPoolValue;
        healthImage.fillAmount = health / maxHealth;
        if (refillNumbness&&numbnessPool>0) {
            numbnessPool -= numbnessPoolDecayValue;
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
            countdownTimerImage.fillAmount = countDown / maxCountdownValue;
            if (countDown <= 0) {
                powerActivated = false;
                playHurtAnim = true;
                SetPlayerStats(originalValues[0] / health, originalValues[1], originalValues[2], originalValues[3] / attackPower);
                //originalValues.Clear();
                playerBaseAbilities.powerUpImage.GetComponent<Animator>().SetBool("Activate", false);
                playerAnimator.maxDashes = 1;
                playerAnimator.maxJumps = 1;
            }
        }
    }

    private void ResetPlayer()
    {
        playerBaseAbilities.dataSet.numericalValues[5]++;
        health = maxHealth;
        GetComponent<PlayerBaseAbilities>().willPower = GetComponent<PlayerBaseAbilities>().maxWillPower;
        gameObject.transform.position = GameObject.Find("Hub Spawn Point").transform.position;
        playerBaseAbilities.SaveData();
    }

    public void refillPotions()
    {
        potionCounter = maxPotions;
        potionsText.text = potionCounter.ToString();
    }
}