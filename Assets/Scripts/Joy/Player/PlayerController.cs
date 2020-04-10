using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    protected float m_speed;
    [SerializeField]
    protected float m_jumpForce = 2.0f;


    protected Rigidbody2D m_body2d;
    protected PlayerSensor m_groundSensor;
    protected bool m_grounded = false;
    protected bool m_combatIdle = false;
    protected bool m_isDead = false;
    protected PlayerStats playerStats;
	
	// Update is called once per frame
	void FixedUpdate () {
        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State()) {
            m_grounded = true;
            
        }

        //Check if character just started falling
        if(m_grounded && !m_groundSensor.State()) {
            m_grounded = false;
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
            GetComponent<SpriteRenderer>().flipX = true;
        else if (inputX < 0)
            GetComponent<SpriteRenderer>().flipX = false;

        // Move
        m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);


        //Jump
        if (Input.GetButtonDown("Jump") && m_grounded) {

            m_grounded = false;
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }
    }

    public void GetSpeed () {
        m_speed = playerStats.SpeedChange();
    }
}
