using DialogueEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class CamControl : MonoBehaviour
{
    [SerializeField] float sensX;
    [SerializeField] float sensY;

    [SerializeField] float clampAngleY = 75f;

    [SerializeField] Transform orientation;

    float rotationX;
    float rotationY;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Mouse look no longer scales by Time.deltaTime (frame-rate independent), so it
        // would keep rotating while the game is paused. Freeze it when time is stopped.
        if (Time.timeScale == 0f)
            return;

       //if (Input.GetKey(KeyCode.Mouse0))
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * sensY;

            rotationY += mouseX;
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -clampAngleY, clampAngleY);

            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
            orientation.rotation = Quaternion.Euler(0, rotationY, 0);
        }
    }
}
