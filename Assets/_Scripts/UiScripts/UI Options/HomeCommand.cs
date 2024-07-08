using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeCommand : MonoBehaviour, ICommand 
{
    UiUtility settingsUtility;
    public void Execute(object _parameter)
    {
        settingsUtility = new UiUtility();
        SceneManager.LoadSceneAsync(0);
        settingsUtility.ToggleCanvases(_parameter, false);
    }
}