using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private FixButtonMovement fixButton;
    private Rigidbody rb;
    public Joystick joystick;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private bool isGrounded;
    [SerializeField]
    private Transform groundChecker;
    [SerializeField]
    private float groundCheckRadius;
    public LayerMask groundCheckLayers;


    void Start()
    {
        fixButton = FindObjectOfType<FixButtonMovement>();
        joystick = FindObjectOfType<Joystick>();
        rb = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position,groundCheckRadius,groundCheckLayers);
        rb.velocity = new Vector3(joystick.Horizontal * moveSpeed, rb.velocity.y, joystick.Vertical * moveSpeed);

        if(isGrounded&& fixButton.isPressing)
        {
            rb.velocity += Vector3.up * jumpForce;
        }
    }
}
