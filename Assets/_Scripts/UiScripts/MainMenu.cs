using System.Collections.Generic;
using Unity.Services.CloudSave;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] List<MenuItem> menuItems;
    [SerializeField] GameObject instructionPanel;
    [SerializeField] GameObject settingsPanel;

    private void Start()
    {
        menuItems[0].SetCommand(new StartGameCommand());
        menuItems[1].SetCommand(new InstructionsCommand(), instructionPanel);
        menuItems[2].SetCommand(new SettingsCommand(), settingsPanel);
        menuItems[3].SetCommand(new QuitCommand()); 
    }
}
