using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Enemy : MonoBehaviour {

    [SerializeField]
    protected float m_speed;
    [SerializeField]
    float enemyMaxHealth;
    [SerializeField]
    protected float m_attackDamage;
    [SerializeField]
    protected float m_attackSpeed;
    [SerializeField]
    float attackRange;
    [SerializeField]
    protected int m_aggroRange;
    [SerializeField]
    protected float m_startWaitTime = 5;
    [SerializeField]
    protected GameObject m_player;
    [SerializeField]
    Transform attackPoint;
    [SerializeField]
    protected Transform m_patrolTarget1;
    [SerializeField]
    protected Transform m_patrolTarget2;
    [SerializeField]
    protected LayerMask playerLayers;
    [SerializeField]
    protected Transform m_enemyHealthbar;
    [SerializeField]
    protected Image m_enemyHealthbarFill;
    [SerializeField]
    protected Camera m_camera;

    protected Rigidbody2D m_body2d;

    protected bool m_seenPlayer     = false;
    protected bool m_isDead         = false;
    protected bool m_isFacingRight  = true;
    protected bool m_inMelee        = false;

    protected float m_waitTime      = 0;
    protected float m_attackWait    = 0;
    float           enemyHealth;

    Vector3 attackPointPosition;

    [SerializeField]
    protected Transform m_target;

    SpriteRenderer spriteRenderer;

    Animator m_animator;

    Vector2 m_wantedPos;

    // Start is called before the first frame update
    void Awake () {
        m_body2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackPoint = transform.Find("AttackPoint");
        attackPointPosition = attackPoint.localPosition;
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_animator = GetComponent<Animator>();
        enemyHealth = enemyMaxHealth;
    }

    // Update is called once per frame
    void Update () {

        m_enemyHealthbarFill.fillAmount = enemyHealth / enemyMaxHealth;

        if (enemyHealth <= 0 && !m_isDead)
        {
            m_enemyHealthbar.gameObject.SetActive(false);
            GetComponent<BoxCollider2D>().enabled = false;
            m_body2d.simulated = false;
            m_animator.SetTrigger("Dead");
            m_isDead = true;
        }
        else if(!m_isDead)
        {
            //m_wantedPos = m_camera.WorldToViewportPoint(new Vector2(transform.position.x, transform.position.y + 50));
            //m_enemyHealthbar.position = m_wantedPos;


            LookForPlayer();
            //Checks if Player is in melee, if they are then attack, if not then move
            if (m_inMelee)
            {
                Attack();
            }
            else
                Move();
        }

        //if (!m_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        //{
        //    float inputX = Input.GetAxis("Horizontal");
        //    //Run
        //    if (Mathf.Abs(inputX) > Mathf.Epsilon)
        //        m_animator.SetInteger("Speed", (int)m_body2d.velocity.magnitude);
        //}

    }

    public void TakeDamage (float damageValue) {
        if (enemyHealth > 0)
        {
            enemyHealth -= damageValue;
            m_animator.SetTrigger("Damage");
        }
    }

    private void ChangeDirection () {

        m_isFacingRight = !m_isFacingRight;

        //finds attack point

        //Flips sprite and changes side of the attack point
        if (m_isFacingRight) {
            spriteRenderer.flipX = false;
            attackPoint.transform.localPosition = new Vector3(attackPointPosition.x, attackPointPosition.y);
        }
        else {
            spriteRenderer.flipX = true;
            attackPoint.transform.localPosition = new Vector3(-attackPointPosition.x, attackPointPosition.y);
        }
    }

    private void Attack () {
        //Prints if circle intersects with something on player layer
        if (Time.time > m_attackWait && m_player.GetComponent<PlayerStats>().health > 0)
        {
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);
            foreach (Collider2D player in hitPlayer)
            {
                if (player.tag == "Player")
                {
                    m_animator.SetTrigger("Attack");
                    m_player.GetComponent<PlayerStats>().TakeDamage(m_attackDamage);
                }
                else if (player.tag == "Shield") {
                    m_animator.SetTrigger("Damage");
                }
            }

            m_attackWait = Time.time + m_attackSpeed;
        }
    }


    private void LookForPlayer () {
        //Checks distance to player
        float distToPlayer = Vector2.Distance(transform.position, m_player.transform.position);

        //Follows player if within aggro range
        if (distToPlayer < m_aggroRange && m_player.GetComponent<PlayerStats>().health > 0) {
            m_waitTime = 0;
            m_seenPlayer = true;
            m_target = m_player.transform;
        }
        else if ((m_seenPlayer && distToPlayer > m_aggroRange + 2) || m_player.GetComponent<PlayerStats>().health <= 0) {
            m_seenPlayer = false;
            m_target = null;
        }


        if (Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayers))
            m_inMelee = true;
        else
            m_inMelee = false;
    }

    private void Move () {
        //Placeholder Patrol
        if (!m_seenPlayer) {

            if (m_target == null)
                m_target = m_patrolTarget1;

            if (transform.position.x > m_target.position.x - 0.2f && transform.position.x < m_target.position.x + 0.2f) {
                //Resets wait time and counts down
                if (m_waitTime <= 0)
                    m_waitTime = m_startWaitTime;
                else
                    m_waitTime -= Time.deltaTime;

                //Find new target after waiting
                if (m_waitTime <= 0) {
                    if (m_target == m_patrolTarget1)
                        m_target = m_patrolTarget2;
                    else
                        m_target = m_patrolTarget1;
                    ChangeDirection();
                }
            }
        }
        else //Supposed to be separate
            if (m_target.position.x > transform.position.x && !m_isFacingRight || m_target.position.x < transform.position.x && m_isFacingRight)
            ChangeDirection();


        //Normal Move
        if (m_isFacingRight && m_waitTime <= 0)
            m_body2d.velocity = new Vector2(m_speed, 0);
        else if (!m_isFacingRight && m_waitTime <= 0)
            m_body2d.velocity = new Vector2(-m_speed, 0);



    }

    void OnDrawGizmos () {
        if (attackPoint == null)
            return;
        //Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}