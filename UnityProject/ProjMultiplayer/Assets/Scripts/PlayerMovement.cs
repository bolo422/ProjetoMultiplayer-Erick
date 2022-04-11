using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class PlayerMovement : EntityBehaviour<IPlayerState>
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    float movementMultiplier = 10f;
    [SerializeField] float airMultiplier = 0.4f;

    [Header("Jumping")]
    public float jumpForce = 5f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    [Header("Drag")]
    float groundDrag = 6f;
    float airDrag = 2f;

    

    float playerHeight = 2f;
    float horizontalMovement;
    float verticalMovement;

    bool isGrounded;

    Vector3 moveDirection;

    Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    //Start()
    public override void Attached()
    {
        state.SetTransforms(state.Transform, transform);
        rb.freezeRotation = true;
    }

    //Upadte()
    public override void SimulateOwner()
    {
        


    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;
    }

    void MovePlayer()
    {
        if(isGrounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        else
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight / 2 + 0.1f);

        MyInput();
        ControlDrag();

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }
    }

    void ControlDrag()
    {
        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = airDrag;
    }

    void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

}
