using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : PlayerController {

    public Animator m_animator;
    public PlayerBaseAbilities playerBaseAbilities;
    [SerializeField]
    int powerUpIndex;
    public List<KeyCode> powerUpSelection=new List<KeyCode>();

    void Awake() {
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
    }

    // Update is called once per frame
    void Update()
    {
        m_animator.SetBool("Grounded", m_grounded);
        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);
        if(Input.GetButtonDown("Jump"))
        m_animator.SetTrigger("Jump");

        //Attack
        if (Input.GetMouseButtonDown(0)) {
            m_animator.SetTrigger("Attack");
        }
        float inputX = Input.GetAxis("Horizontal");
        //Run
        if (Mathf.Abs(inputX) > Mathf.Epsilon)
            m_animator.SetInteger("AnimState", 2);

        //Combat Idle
        else if (m_combatIdle)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);

        if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            if (powerUpIndex < 4)
                powerUpIndex++;
            else
                powerUpIndex=0;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            if (powerUpIndex > 0)
                powerUpIndex--;
            else
                powerUpIndex = 4;
        }
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
        if (Input.GetKeyDown(KeyCode.E)) {
            playerBaseAbilities.SetPowerUp((PowerUp)powerUpIndex);
        }
    }
}
