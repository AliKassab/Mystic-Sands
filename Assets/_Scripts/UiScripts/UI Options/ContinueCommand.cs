using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueCommand : ICommand
{
    GameObject canvas;
    public void Execute(object _parameter)
    {
        Time.timeScale = 1;
        
        canvas = (GameObject) _parameter;
        canvas.SetActive(false);
    }
}
