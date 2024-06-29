using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitCommand :MonoBehaviour, ICommand
{
    public void Execute(object _parameter)
    {
        Application.Quit();
    }
}
