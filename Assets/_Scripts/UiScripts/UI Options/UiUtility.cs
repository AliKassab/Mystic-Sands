using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiUtility : MonoBehaviour
{
    public AudioMixer AudioMixer;
    [SerializeField] Dropdown ResolutionDropDown;
    [SerializeField] List<GameObject> canvas;
    List<GameObject> canvasesToHide;
    float volume;
    Resolution[] resolutions;

    private void Start()
    {
        Screen.SetResolution(1920, 1080, Screen.fullScreen);    
        FillResolutionDropDown();
    }
    public void SetVolume(float volume) => AudioMixer.SetFloat("MyExposedParam", volume);
    public void SetQuality(int quality) => QualitySettings.SetQualityLevel(quality);
    public void SetResolution(int resIndex)
    {
        Resolution resolution= resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void ContinueButton(int index)
    {
        canvas[index].SetActive(false);
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void ReturnToMainMenu() => SceneManager.LoadScene(0);
    public void CloseClick(int index) => canvas[index].SetActive(false);
    public void FillResolutionDropDown()
    {
        resolutions = Screen.resolutions;
        ResolutionDropDown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRateRatio + "Hz";
            options.Add(option);
        }
        ResolutionDropDown.AddOptions(options);
    }

    //public void ToggleCanvas( bool toggleBoolean){
    //    canvas[2].SetActive(toggleBoolean);
    //}
    public void ToggleCanvases(object _parameter, bool toggleBoolean){

        canvasesToHide = _parameter as List<GameObject>;
        foreach (var item in canvasesToHide)
            item.SetActive(toggleBoolean);
    }
}
