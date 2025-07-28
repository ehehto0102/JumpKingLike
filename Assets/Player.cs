using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D m_rigidbody;
    [SerializeField] private float m_speed = 5.0f;
    [SerializeField] private float m_jumpForce = 5.0f;

    [SerializeField] private RectTransform m_rectTransform_ProgressBar;

    private bool m_bisJumping = false;
    private bool m_bisJumpCharging = false;

    private bool m_bisLeft = false;

    [SerializeField] private float m_maxJumpChargeTime = 2.0f;
    private float m_curJumpChargeTime = 0.0f;


    public void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        if (!m_bisJumping && !m_bisJumpCharging)
        {
            m_rigidbody.velocity = new Vector2()
            {
                x = moveHorizontal * m_speed,
                y = m_rigidbody.velocity.y
            };
            m_bisLeft = moveHorizontal < 0.0f ? true : false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !m_bisJumping && !m_bisJumpCharging)
        {
            m_bisJumpCharging = true;
            m_rigidbody.velocity = Vector2.zero;
        }
        else if (Input.GetKey(KeyCode.Space) && m_bisJumpCharging && !m_bisJumping)
        {
            m_curJumpChargeTime += Time.deltaTime;

            if (m_curJumpChargeTime > m_maxJumpChargeTime)
            {
                m_curJumpChargeTime = m_maxJumpChargeTime;
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space) && !m_bisJumping)
        {
            m_bisJumping = true;

            m_rigidbody.AddForce(new Vector2(m_bisLeft ? -m_speed : m_speed, (m_curJumpChargeTime / m_maxJumpChargeTime) * m_jumpForce), ForceMode2D.Impulse);
            m_curJumpChargeTime = 0.0f;
        }

        m_rectTransform_ProgressBar.localScale = new Vector3()
        {
            x = m_curJumpChargeTime / m_maxJumpChargeTime,
            y = 1.0f,
            z = 1.0f
        };
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        m_bisJumpCharging = false;
        m_bisJumping = false;
        m_curJumpChargeTime = 0.0f;
    }
}