using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuConfig : MonoBehaviour
{

    [SerializeField] GameObject canvas;

    private void Start()
    {
        canvas.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void continueBTN()
    {
        canvas.SetActive(false);
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    public void quitButton()
    {
        Application.Quit();
    }

    public void mainMenu()
    {
        SceneManager.LoadScene(0);
    }



}
