using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] AudioMixer AudioMixer;
    [SerializeField] Dropdown dropdown;
    Resolution[] resolutions;
    [SerializeField] GameObject canvas;

    private void Start()
    {
        resolutions = Screen.resolutions;

        dropdown.ClearOptions();

        List<string> options= new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
        }
        dropdown.AddOptions(options);
    }

    public void setVolume(float volume)
    {
        AudioMixer.SetFloat("Volume", volume);
    }

    public void setQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }

    public void setRes(int resIndex)
    {
        Resolution resolution= resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void closeClick()
    {
        canvas.SetActive(false);
    }
}
