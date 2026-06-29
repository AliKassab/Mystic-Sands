using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveControl : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Transform orientation;
    [SerializeField] float drag;
    [SerializeField] float rotationSpeed = 10f; // Speed of rotation

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rgd;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rgd = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rgd.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer(); // Rotate player based on movement
    }

    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    void MovePlayer()
    {
        moveDirection = (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized;

        // Drive horizontal velocity directly so the player stops the instant input
        // is released (no coasting), while leaving vertical velocity to gravity.
        Vector3 targetVelocity = moveDirection * moveSpeed;
        rgd.linearVelocity = new Vector3(targetVelocity.x, rgd.linearVelocity.y, targetVelocity.z);

        animator.SetBool("Walking", moveDirection.sqrMagnitude > 0f);
    }

    void RotatePlayer()
    {
        if (moveDirection.magnitude > 0)
        {
            // Rotate the player towards the move direction smoothly via the Rigidbody
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized);
            Quaternion newRotation = Quaternion.Slerp(rgd.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            rgd.MoveRotation(newRotation);
        }
    }

}
