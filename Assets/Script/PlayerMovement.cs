using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    private CharacterController character;
    [SerializeField]
    private FixButtonMovement fixButton;
    public Joystick joystick;
    [SerializeField]
    private float ySpeed;
    [SerializeField]
    private float gravityModifier;
    [SerializeField]
    private bool isGrounded;
    [SerializeField]
    private Transform groundChecker;
    [SerializeField]
    private float groundCheckRadius;
    public LayerMask groundCheckLayers;
    private float? lastGroundedTime;
    private float? jumpPressTime;
    public Animator animator;
    private bool isIdle;
    private bool isJump;
    private bool isFalling;
    public float jumpPeriod;
    public float jumpHeight;
    public float rotationSpeed;
    public float jumpHorizontalSpeed;
    void Start()
    {
        fixButton = FindObjectOfType<FixButtonMovement>();
        joystick = FindObjectOfType<Joystick>();
        
    }

    
    void Update()
    {
        float horizontalInput = joystick.Horizontal;
        float verticalInput = joystick.Vertical;

        isGrounded = Physics.CheckSphere(groundChecker.position,groundCheckRadius,groundCheckLayers);
        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);
        float inputMagnitude = Mathf.Clamp01(moveDirection.magnitude);
        moveDirection.Normalize();
        float gravity = Physics.gravity.y * gravityModifier;
        if (isJump && ySpeed > 0 && fixButton.isPressing == false)
        {
            gravity *= 2;
        }

        ySpeed += gravity * Time.deltaTime;

        if (character.isGrounded)
        {
            lastGroundedTime = Time.time;
        }
        if (fixButton.isPressing)
        {
            jumpPressTime = Time.time;
        }
        if (Time.time - lastGroundedTime <= jumpPeriod)
        {
            ySpeed = -0.5f;
            isGrounded = true;
            isJump = false;
            isFalling = false;

            if (Time.time - jumpPressTime <= jumpPeriod)
            {
                ySpeed = Mathf.Sqrt(jumpHeight * -3 * gravity);
                isJump = true;
                jumpPressTime = null;
                lastGroundedTime = null;
            }
        }
        else
        {
            isGrounded = false;
            if ((isJump && ySpeed < 0) || ySpeed < -2)
            {
                isFalling = true;
            }
        }
        if (moveDirection != Vector3.zero)
        {
            isIdle = false;
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            isIdle = true;
        }

        if (isGrounded == false)
        {
            Vector3 velocity = moveDirection * inputMagnitude * jumpHorizontalSpeed;
            velocity.y = ySpeed;
            character.Move(velocity * Time.deltaTime);
        }
        animator.SetFloat("MoveSpeed", inputMagnitude);
        animator.SetBool("Idle", isIdle);
        animator.SetBool("Jump",isJump);
        animator.SetBool("Falling",isFalling);
    }

}
