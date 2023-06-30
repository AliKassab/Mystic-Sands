using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCursorFollower : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    private float yaw = -149.495f, pitch = 8.329f;

    // Update is called once per frame
    void Update()
    {
        yaw += Input.GetAxis("Mouse X") * moveSpeed;
        pitch -= Input.GetAxis("Mouse Y") * moveSpeed;
        if (yaw <= -150f) 
        {
            yaw = -150f;
        }
        if (yaw >= -148f)
        {
            yaw = -148f;
        }
        if (pitch <= 7f)
        {
            pitch = 7f;
        }
        if (pitch >= 12f)
        {
            pitch = 12f;
        }

        transform.eulerAngles = new Vector3(pitch, yaw, -3.209f);
    }
}
