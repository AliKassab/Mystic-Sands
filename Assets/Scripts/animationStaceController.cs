using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStaceController : MonoBehaviour
{
    private Animator animator;

    private int isWalkingHash;


    CharacterController Controller;
    public float speed = 5;
    Transform cam;
    float gravity = 50;
    float verticalVelocity = 10;
    public float jumpValue = 10;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");

        Controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //bool isWalking = animator.GetBool(isWalkingHash);
        //bool forwardPressed = Input.GetKey("w");

        //if (!isWalking && forwardPressed)
        //{

        //    animator.SetBool(isWalkingHash, true);
        //}

        //if (isWalking && !forwardPressed)
        //{

        //    animator.SetBool(isWalkingHash, false);
        //}



        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool isSprint = Input.GetKey(KeyCode.LeftShift);
        float sprint;
        //float sprint = isSprint ? 1.7f : 1;

        if (isSprint)
            sprint = 1.7f;
        else
            sprint = 1;
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);
        if (isSprint)
        {
            animator.SetFloat("Speed", moveDirection.magnitude);
        }
        else
        {
            animator.SetFloat("Speed", Mathf.Clamp(moveDirection.magnitude, 0, 0.5f));

        }
        if (Controller.isGrounded)
        {
            if (Input.GetAxis("Jump") > 0)
                verticalVelocity = jumpValue;
        }
        else
            verticalVelocity -= gravity * Time.deltaTime;
        if (moveDirection.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        moveDirection = cam.TransformDirection(moveDirection);
        moveDirection = new Vector3(moveDirection.x * speed * sprint, verticalVelocity, moveDirection.z * speed * sprint);
        Controller.Move(moveDirection * Time.deltaTime);
    }

}
