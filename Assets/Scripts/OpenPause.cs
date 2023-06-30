using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPause : MonoBehaviour
{

    [SerializeField] GameObject canvas;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            canvas.SetActive(true);
            Time.timeScale= 0f;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
