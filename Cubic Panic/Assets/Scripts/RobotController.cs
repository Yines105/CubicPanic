using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    // Components
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    public GameObject m_GrabbedBlock;
    // Player flipping
    private bool m_GoingRight;

    public bool GoingRight
    {
        get { return m_GoingRight; }
        set
        {
            if (m_GoingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            m_GoingRight = value;
        }
    }
    // Inputs
    private float m_Horizontal;
    private float m_Vertical;
    public bool m_GrabPressed;
    private bool m_JumpPressed;
    public bool m_IsActing;
    //Movement variables
    public float m_Speed;
    public float m_JumpForce;
    public float m_GravityScaleJumping = 1f;
    public float m_GravityScaleFalling = 1f;
    public bool m_IsGrounded;
    // Grab variables
    public GameObject m_HorizontalGrabCollider;
    public GameObject m_UpGrabCollider;
    public GameObject m_DownGrabCollider;
    // Start is called before the first frame update
    void Start()
    {
        m_IsActing = false;
        m_GoingRight = true;
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
        HandleJump();
        HandleGrab();
        DropBlock();
    }
    private void FixedUpdate()
    {
        HandleMovement();
    }
    private void HandleInputs()
    {
        m_Horizontal = Input.GetAxis("Horizontal");
        m_Vertical = Input.GetAxis("Vertical");
        m_JumpPressed = Input.GetKeyDown(KeyCode.Space);
        m_GrabPressed = Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift);
    }
    private void HandleJump()
    {
        if (m_JumpPressed && m_IsGrounded && !m_IsActing)
        {
            m_Rigidbody.AddForce(Vector2.up * m_JumpForce);
        }
        if (m_Rigidbody.velocity.y > 0)
        {
            m_Rigidbody.gravityScale = m_GravityScaleJumping;
        }
        else if (m_Rigidbody.velocity.y < 0)
        {
            m_Rigidbody.gravityScale = m_GravityScaleFalling;
        }
    }
    private void HandleMovement()
    {
        if(!m_IsActing)
        {
            // Applying movement
            m_Rigidbody.velocity = new Vector2(m_Horizontal * m_Speed, m_Rigidbody.velocity.y);
            if(GoingRight && m_Horizontal < 0)
            {
                GoingRight = false;
            }
            else if (!GoingRight && m_Horizontal > 0)
            {
                GoingRight = true;
            }
        }


    }
    private void HandleGrab()
    {
        if (m_GrabPressed && m_GrabbedBlock == null)
        {
            if (m_Vertical > 0)
            {
                m_HorizontalGrabCollider.SetActive(false);
                m_UpGrabCollider.SetActive(true);
                m_DownGrabCollider.SetActive(false);
            }
            else if (m_Vertical < 0)
            {
                m_HorizontalGrabCollider.SetActive(false);
                m_UpGrabCollider.SetActive(false);
                m_DownGrabCollider.SetActive(true);
            }
            else
            {
                m_HorizontalGrabCollider.SetActive(true);
                m_UpGrabCollider.SetActive(false);
                m_DownGrabCollider.SetActive(false);
            }
        }
        
    }
    public void GrabBlock(GameObject Block)
    {
        m_GrabbedBlock = Block;
        m_GrabbedBlock.SetActive(false);
        m_HorizontalGrabCollider.SetActive(false);
        m_UpGrabCollider.SetActive(false);
        m_DownGrabCollider.SetActive(false);
    }
    private void DropBlock()
    {
        if (m_GrabPressed && m_GrabbedBlock != null)
        {
            
            if (Input.GetKeyDown(KeyCode.W) || (Input.GetKeyDown(KeyCode.UpArrow)))
            {
                m_GrabbedBlock.transform.position = m_UpGrabCollider.transform.position;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                m_GrabbedBlock.transform.position = m_DownGrabCollider.transform.position;
            }
            else
            {
                m_GrabbedBlock.transform.position = m_HorizontalGrabCollider.transform.position;
            }
            m_GrabbedBlock.SetActive(true);
            m_GrabbedBlock = null;
        }
    }
}
