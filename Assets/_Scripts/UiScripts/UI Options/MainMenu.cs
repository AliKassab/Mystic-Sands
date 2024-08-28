using System;
using System.Collections.Generic;
using Unity.Services.CloudSave;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] List<MenuItem> menuItems;
    [SerializeField] List<GameObject> canvases;
    UiUtility settingsUtility;
    bool isPaused = false;
    List<GameObject> pausePanel;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {   
        settingsUtility = new UiUtility();
        
        pausePanel = new List<GameObject>();
        pausePanel.Add(canvases[3]);
        //GameObject tempCanvas = canvases[4];
        //remove canvas from list then return it
        //canvases.Remove(canvases[4]);
        menuItems[0].SetCommand(new StartGameCommand(), canvases);
        menuItems[1].SetCommand(new InstructionsCommand(), canvases[1]);
        menuItems[2].SetCommand(new SettingsCommand(), canvases[2]);
        menuItems[3].SetCommand(new QuitCommand()); 
        menuItems[4].SetCommand(new QuitCommand());
        menuItems[5].SetCommand(new ContinueCommand(), canvases[3]);
        //canvases.Add(canvases[4]);
        menuItems[6].SetCommand(new HomeCommand(), canvases); 
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex != 0){
            Cursor.lockState = isPaused ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = isPaused;
            Time.timeScale = isPaused ? 1 : 0;
            settingsUtility.ToggleCanvases(pausePanel, !isPaused);
            isPaused = !isPaused;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) { 
        if (scene.buildIndex == 1)
            settingsUtility.ToggleCanvases(canvases, false); 
    } 

}
