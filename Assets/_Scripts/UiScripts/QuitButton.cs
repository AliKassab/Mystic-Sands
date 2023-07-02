using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitButton : MonoBehaviour
{
    [SerializeField] Light l;
    private void Update()
    {
        if (l.enabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Application.Quit();
            }
        }

    }
}
