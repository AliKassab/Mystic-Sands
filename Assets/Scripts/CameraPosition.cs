using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] Transform cameraPosition;
    // Update is called once per frame
    void Update()
    {
        transform.position = cameraPosition.position;
    }
}
