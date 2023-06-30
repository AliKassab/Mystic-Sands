using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] Light l;
    [SerializeField] GameObject canvas;
    private void Update()
    {
        if (l.enabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                canvas.SetActive(true);
            }
        }

    }
}
