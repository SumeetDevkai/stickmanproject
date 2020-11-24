using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPunCallbacks, IPunObservable
{
    public new PhotonView photonView;

    private float HorizontalInputDirection;
    private float turnTimer;

    private int amountOfJumpsLeft;
    private int facingDirection = 1;

    private bool isFacingRight = true;
    private bool isWalking;
    private bool isAttacking;
    private bool isGrounded;
    private bool canJump;
    private bool canMove;
    private bool canFlip;

    private Rigidbody2D rb;
    private Animator anim;
    public float Health = 5;
    public int amountOfJumps = 1;
    [SerializeField]
    private int GravityScale;


    public float attackDamage;
    public float movementSpeed;
    public float jumpForce;
    public float groundCheckRadius;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float turnTimerSet = 0.1f;

    public Transform groundCheck;

    public LayerMask whatIsGround;

    public Transform attackPoint;
    public float attackRange;
    public LayerMask attackLayers;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        ApplyMovement();
        CheckSurroundings();
    }


    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    private void CheckIfCanJump()
    {
        if ((isGrounded && rb.velocity.y <= 0))
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        if (amountOfJumpsLeft <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }

    }
   
    private void CheckMovementDirection()
    {
        if (isFacingRight && HorizontalInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && HorizontalInputDirection > 0)
        {
            Flip();
        }

        if (rb.velocity.x != 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void UpdateAnimations()
    {
        //anim.SetBool("isWalking", isWalking);
        //anim.SetBool("isGrounded", isGrounded);
        //anim.SetFloat("yVelocity", rb.velocity.y);
        //anim.SetBool("isWallSliding", isWallSliding);
        anim.SetBool("attack", isAttacking);
    }

    private void CheckInput()
    {
        HorizontalInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Horizontal"))
        {
            if (!isGrounded && HorizontalInputDirection != facingDirection)
            {
                canMove = false;
                canFlip = false;
            }
        }

        
        if (!canMove)
        {
            turnTimer -= Time.deltaTime;

            if (turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetButtonUp("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Attack();
        }
    }

    private void Attack()
    {
        isAttacking = true;

        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, attackLayers);

        foreach (Collider2D enemy in enemies)
        {
                enemy.GetComponent<PlayerMovement>().TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(float value)
    {
        Debug.Log("attacked");
        Health -= value;
        
    }

    private void Jump()
    {
        if (canJump )
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
        }
    }

    private void ApplyMovement()
    {

        if (!isGrounded && HorizontalInputDirection == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
        else if (canMove)
        {
            rb.velocity = new Vector2(movementSpeed * HorizontalInputDirection, rb.velocity.y);
        }

    }

    private void Flip()
    {
        if (canFlip)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    public void Attackbool()
    {
        isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);   
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Health);
        }
        else
        {
            this.Health = (int)stream.ReceiveNext();
        }
    }
}
