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
    float attackRange;
    public LayerMask enemyLayers;
    Vector3 attackPointPosition;

    public bool playerDown;


    [Tooltip("The x is the first damage, y the second and z the third. This is in respect to the sword swings of the player during the combat")]
    [SerializeField]
    Vector3 damageVectors;

    Rigidbody2D rb2D;

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
        powerUpSelection.Add(KeyCode.Alpha5);
        attackPointPosition = attackPoint.localPosition;
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {
        m_animator.SetBool("Grounded", m_grounded);
        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);
        if (Input.GetButtonDown("Jump"))
            m_animator.SetTrigger("Jump");

        //Attack
        if (Input.GetMouseButtonDown(0) && !m_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
            m_animator.SetTrigger("FirstAttack");
            HitEnemy(damageVectors.x);
        }
        else if (Input.GetMouseButtonDown(0) && m_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
            m_animator.SetTrigger("Attack");
            HitEnemy(damageVectors.y);
        }
        if (m_animator.GetCurrentAnimatorStateInfo(0).IsTag("FinalAttack")) {
            HitEnemy(damageVectors.z);
        }
        if (Input.GetMouseButtonDown(1)) {
            m_animator.SetTrigger("HeavyAttack");
        }
        if (!m_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
            float inputX = Input.GetAxis("Horizontal");
            //Run
            if (Mathf.Abs(inputX) > Mathf.Epsilon)
                m_animator.SetInteger("Speed", (int)rb2D.velocity.magnitude);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            if (powerUpIndex < 4)
                powerUpIndex++;
            else
                powerUpIndex = 0;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            if (powerUpIndex > 0)
                powerUpIndex--;
            else
                powerUpIndex = 4;
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
                    case KeyCode.Alpha4: {
                            powerUpIndex = 3;
                            break;
                        }
                    case KeyCode.Alpha5: {
                            powerUpIndex = 4;
                            break;
                        }
                }
            }

        }
        //Activating the powerup
        if (Input.GetKeyDown(KeyCode.E) && !playerStats.powerActivated) {
            playerBaseAbilities.SetPowerUp((PowerUp)powerUpIndex);
        }

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



        //dash
        if (Input.GetKeyDown(KeyCode.LeftAlt)) {
            Vector3 newPosition = transform.position + new Vector3(dashDistance * BoolToInteger(), 0, 0);
            m_animator.SetTrigger("Dash");
            transform.DOMove(newPosition, 0.5f);
        }
        if (spriteRenderer.flipX)
            attackPoint.localPosition = new Vector2(-attackPointPosition.x, attackPointPosition.y);
        else
            attackPoint.localPosition = new Vector2(attackPointPosition.x, attackPointPosition.y);
    }

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
}