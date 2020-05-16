using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerAnimator : PlayerController {
    public Animator m_animator;
    public PlayerBaseAbilities playerBaseAbilities;
    [SerializeField]
    int powerUpIndex;
    public List<KeyCode> powerUpSelection = new List<KeyCode>();
    [SerializeField]
    float dashDistance;
    [SerializeField]
    SpriteRenderer spriteRenderer;
    [SerializeField]
    Transform attackPoint;
    [SerializeField]
    Transform shieldTransform;
    [SerializeField]
    float attackRange;
    public LayerMask enemyLayers;
    Vector3 attackPointPosition;
    Vector3 shieldPosition;

    public bool playerDown;

    [Tooltip("The x is the first damage, y the second and z the third. This is in respect to the sword swings of the player during the combat")]
    [SerializeField]
    Vector3 damageVectors;

    Rigidbody2D rb2D;

    [SerializeField]
    float rayCastLength;

    public float distanceCheck;

    public bool doubleDash;
    public bool doubleJump;

    [SerializeField]
    float dashTimeRecharge;
    float lastDashed;
    int maxDashes=1;
    int maxJumps=1;

    int jumpCounter=0;
    int dashCounter=0;

    bool climbable;

    [SerializeField]
    Vector3 offsetVector;

    [SerializeField]
    Vector3 raycastVectorOffset;

    bool powerActivated;
    float countDown;

    public bool dead;

    void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerStats = GetComponent<PlayerStats>();
        m_speed = playerStats.SpeedChange();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<PlayerSensor>();
        m_animator = GetComponent<Animator>();
        playerBaseAbilities = GetComponent<PlayerBaseAbilities>();
        powerUpSelection.Add(KeyCode.Alpha1);
        powerUpSelection.Add(KeyCode.Alpha2);
        powerUpSelection.Add(KeyCode.Alpha3);
        powerUpSelection.Add(KeyCode.Alpha4);
        attackPointPosition = attackPoint.localPosition;
        shieldPosition = shieldTransform.localPosition;
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void SetCountDown (float maxCountDown) {
        powerActivated = true;
        countDown = maxCountDown;
    }

    public void EscapeMechanicUpdate (bool activate) {
        doubleDash = activate;
        doubleJump = activate;
        if (activate)
            dashTimeRecharge = dashTimeRecharge / 2;
        else
            dashTimeRecharge = dashTimeRecharge * 2;
    }

    // Update is called once per frame
    void Update () {
        if (!dead) {

            if (countDown > 0) {
                countDown -= Time.deltaTime;
                if (countDown <= 0) {
                    powerActivated = false;
                    EscapeMechanicUpdate(false);
                    playerBaseAbilities.powerUpImage.GetComponent<Animator>().SetBool("Activate", false);
                }
            }
            //Dash
#if UNITY_EDITOR
            Debug.DrawRay(transform.position + raycastVectorOffset, transform.right * BoolToInteger() * distanceCheck, Color.green);
#endif
            if (Input.GetKeyDown(KeyCode.LeftAlt) && Time.time > lastDashed + dashTimeRecharge * maxDashes && dashCounter < maxDashes) {
                RaycastHit2D hit_Combat = Physics2D.Raycast(transform.position + raycastVectorOffset, transform.right * BoolToInteger(), distanceCheck);
                if (hit_Combat.transform == null || hit_Combat.transform.tag != "Environment") {
                    Vector3 newPosition = transform.position + new Vector3(dashDistance * BoolToInteger(), 0, 0);
                    playerBaseAbilities.willPower -= 5;
                    m_animator.SetTrigger("Dash");
                    transform.DOMove(newPosition, 0.5f);
                    lastDashed = Time.time;
                    dashCounter++;
                }
            }
            else
                dashCounter = 0;

            if ((Input.GetButtonDown("Jump") && m_grounded) || (Input.GetButtonDown("Jump") && jumpCounter < maxJumps)) {
                jumpCounter++;
                m_animator.SetTrigger("Jump");
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                m_groundSensor.Disable(0.2f);
            }
            else if (m_grounded)
                jumpCounter = 0;

            m_animator.SetBool("Grounded", m_grounded);
            //Set AirSpeed in animator
            m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);

            #region ATTACK
            if (m_grounded) {
                //Attack
                if (Input.GetMouseButtonDown(0) && !m_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
                    m_animator.SetTrigger("FirstAttack");
                    HitEnemy(damageVectors.x);
                }
                else if (Input.GetMouseButtonDown(0) && m_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
                    m_animator.SetTrigger("Attack");
                    HitEnemy(damageVectors.y + Random.Range(1f, 5f));
                }

                //Heavy Attack
                if (Input.GetMouseButtonDown(1)) {
                    m_animator.SetTrigger("HeavyAttack");
                    HitEnemy(damageVectors.z);
                }
                if (!m_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
                    float inputX = Input.GetAxis("Horizontal");
                    //Run
                    if (Mathf.Abs(inputX) > Mathf.Epsilon)
                        m_animator.SetInteger("Speed", (int)rb2D.velocity.magnitude);
                }
            }
            #endregion
            #region POWER_UP_SELCETION
            if (Input.GetAxis("Mouse ScrollWheel") > 0) {
                if (powerUpIndex < 2)
                    powerUpIndex++;
                else
                    powerUpIndex = 0;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0) {
                if (powerUpIndex > 0)
                    powerUpIndex--;
                else
                    powerUpIndex = 2;
            }
            playerBaseAbilities.powerUp = (PowerUp)powerUpIndex;
            foreach (KeyCode keyStroke in powerUpSelection) {
                if (Input.GetKeyDown(keyStroke)) {
                    switch (keyStroke) {
                        case KeyCode.Alpha1: {
                                powerUpIndex = 0;
                                break;
                            }
                        case KeyCode.Alpha2: {
                                powerUpIndex = 1;
                                break;
                            }
                        case KeyCode.Alpha3: {
                                powerUpIndex = 2;
                                break;
                            }
                    }
                }
            }
            playerBaseAbilities.powerUpImage.sprite = playerBaseAbilities.powerUpSprites[powerUpIndex];
            playerStats.countdownTimerImage.sprite = playerBaseAbilities.powerUpSprites[powerUpIndex];
			#endregion
			//Activating the powerup
			if (Input.GetKeyDown(KeyCode.E) && !playerStats.powerActivated)
			{
				m_animator.SetBool("Cope", true);
				playerBaseAbilities.SetPowerUp((PowerUp)powerUpIndex);
			}
			else m_animator.SetBool("Cope", false);

            if (!playerDown && playerStats.PlayerHealth() <= 0) {
                playerDown = true;
                m_animator.SetTrigger("Death");
            }

            if (playerDown) {
                if (Input.GetKeyDown(KeyCode.E) && playerBaseAbilities.GetReserveLives() > 3) {
                    m_animator.SetTrigger("Heal");
                    playerBaseAbilities.Revive();
                }
            }
            if (spriteRenderer.flipX) {
                attackPoint.localPosition = new Vector2(-attackPointPosition.x, attackPointPosition.y);
                shieldTransform.localPosition = new Vector2(-shieldPosition.x, shieldPosition.y);
            }
            else {
                attackPoint.localPosition = new Vector2(attackPointPosition.x, attackPointPosition.y);
                shieldTransform.localPosition = new Vector2(shieldPosition.x, shieldPosition.y);
            }

            if (Input.GetKeyDown(KeyCode.F) && playerStats.potionCounter > 0 && playerStats.health < playerStats.maxHealth) {
                playerStats.HealPlayer();
            }

            #region CLIMBING
            RaycastHit2D hit;
            if (spriteRenderer.flipX)
                hit = Physics2D.Raycast(transform.position + offsetVector, -transform.right * 5);
            else
                hit = Physics2D.Raycast(transform.position + offsetVector, transform.right * 5);
            climbable = hit.transform.tag == "Climbable";
            if (climbable && Input.GetAxis("Vertical") > 0) {
                rb2D.simulated = false;
                transform.position += new Vector3(0, 2, 0);
            }
            else if (climbable && Input.GetAxis("Vertical") < 0) {
                rb2D.simulated = false;
                transform.position += new Vector3(0, -2, 0);
            }
            #endregion
        }
    }

    #region COLLISION DETECTIONS
    void OnCollisionEnter2D (Collision2D collider2D) {
        if (collider2D.transform.tag == "Ground") {
            m_grounded = true;
        }
    }

    void OnCollisionExit2D (Collision2D collider2D) {
        if (collider2D.transform.tag == "Ground") {
            m_grounded = false;
        }
    }

    #endregion

    void HitEnemy (float damageValue) {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies) {
            if (enemy.tag == "Enemy") {
                enemy.GetComponent<Enemy>().TakeDamage(damageValue);
            }
        }
    }

    int BoolToInteger () {
        if (spriteRenderer.flipX)
            return -1;
        else
            return 1;
    }

    void OnDrawGizmos () {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void SetAbilities () {
        if (doubleJump)
            maxJumps = 2;
        else
            maxJumps = 1;
        if (doubleDash)
            maxDashes = 2;
        else
            maxDashes = 1;
    }
}