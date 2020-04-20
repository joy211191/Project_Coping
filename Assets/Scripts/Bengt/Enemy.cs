using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    protected float m_speed;
    [SerializeField]
    protected float m_attackDamage;
    [SerializeField]
    protected Transform m_player;
    [SerializeField]
    protected int m_aggroRange;
    [SerializeField]
    Transform attackPoint;
    [SerializeField]
    float attackRange;
    [SerializeField]
    protected Transform m_patrolTarget1;
    [SerializeField]
    protected Transform m_patrolTarget2;
    [SerializeField]
    protected float m_startWaitTime = 5;


    public LayerMask playerLayers;

    protected Rigidbody2D m_body2d;
    protected bool m_seenPlayer = false;
    protected bool m_isDead = false;
    protected bool m_isFacingRight = false;
    protected bool m_inMelee = false;

    protected float m_waitTime = 0;

    //TODO find better way of adding patrol targets, or find another way to patrol
    
    [SerializeField]
    protected Transform m_target;

    // Start is called before the first frame update
    void Awake()
    {
        m_body2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LookForPlayer();

        //Checks if Player is in melee, if they are then attack, if not then move
        if(m_inMelee)
            Attack();
        else 
            Move();

    }

    private void ChangeDirection()
    {

        m_isFacingRight = !m_isFacingRight;

        //finds attack point
        Transform attackPoint = transform.Find("AttackPoint");

        //Flips sprite and changes side of the attack point
        if (m_isFacingRight)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            attackPoint.transform.localPosition = new Vector3(1, 0.35f, 0);
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
            attackPoint.transform.localPosition = new Vector3(-1, 0.35f, 0);
        }


    }

    private void Attack()
    {
        //Prints if circle intersects with something on player layer
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);
        foreach (Collider2D player in hitPlayer)
            Debug.Log("Hit player");

    }

    private void LookForPlayer()
    {
        //Checks distance to player
        float distToPlayer = Vector2.Distance(transform.position, m_player.position);

        //Follows player if within aggro range
        if (distToPlayer < m_aggroRange)
        {
            m_waitTime = 0;
            m_seenPlayer = true;
            m_target = m_player;
        }
        else if (m_seenPlayer && distToPlayer > m_aggroRange + 2)
        {
            m_seenPlayer = false;
            m_target = null;
        }

        if (Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayers))
            m_inMelee = true;
        else
            m_inMelee = false;
    }

    private void Move()
    {
        //Placeholder Patrol 
        //TODO: Make this better
        if (!m_seenPlayer)
        {

            if (m_target == null)
                m_target = m_patrolTarget1;

            if(transform.position.x > m_target.position.x - 0.2f && transform.position.x < m_target.position.x + 0.2f)
            {
                //Resets wait time and counts down
                if (m_waitTime <= 0)
                    m_waitTime = m_startWaitTime;
                else
                    m_waitTime -= Time.deltaTime;

                //Find new target after waiting
                if(m_waitTime <= 0)
                {
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

    void OnDrawGizmos()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
