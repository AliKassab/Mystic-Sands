using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsCommand :MonoBehaviour, ICommand
{
    GameObject instructionsPanel;
    public void Execute(object _parameter)
    {
        instructionsPanel = (GameObject) _parameter;
        instructionsPanel.SetActive(true);
    }
}
