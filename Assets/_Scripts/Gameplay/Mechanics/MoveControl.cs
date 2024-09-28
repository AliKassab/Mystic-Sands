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
        speedControl();
        rgd.drag = drag;

        RotatePlayer(); // Rotate player based on movement
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rgd.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);

        if (moveDirection.magnitude > 0)
            animator.SetBool("Walking", true);
        else
            animator.SetBool("Walking", false);
    }

    void RotatePlayer()
    {
        if (moveDirection.magnitude > 0)
        {
            // Rotate the player towards the move direction smoothly
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void speedControl()
    {
        Vector3 flatVel = new Vector3(rgd.velocity.x, 0, rgd.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rgd.velocity = new Vector3(limitedVel.x, rgd.velocity.y, limitedVel.z);
        }
    }
}
