using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsCommand :MonoBehaviour, ICommand
{
    GameObject canvas;

    public void Execute(object _parameter)
    {
        canvas = (GameObject) _parameter;
        canvas.SetActive(true);
    }
}
