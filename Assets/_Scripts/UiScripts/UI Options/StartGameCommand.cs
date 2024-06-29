using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameCommand : MonoBehaviour, ICommand 
{
    public void Execute(object _parameter)
    {
        SceneManager.LoadScene(1);
    }
}
