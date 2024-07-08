using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseCommand : ICommand
{
    GameObject canvas;
    public void Execute(object _parameter)
    {
        Time.timeScale = 0;
        
        canvas = (GameObject) _parameter;
        canvas.SetActive(false);
    }
}
