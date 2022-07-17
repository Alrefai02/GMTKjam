using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public enum MenuState { START, OPTIONS, EXIT}
public enum OptionsState {RES, FUL, VOL }

public class UIStuff : MonoBehaviour
{
    public GameObject rightArrow;
    public GameObject leftArrow;
    public GameObject mainMenu;
    public GameObject optionsMenu;

    public GameObject resolution;
    public GameObject fullscreen;
    public GameObject volume;


    public Text button;
    public Text optionsText;
    public Text title;
    public Text title2;


    public Dropdown resDropdown;

    public AudioMixer audioMixer;

    public Resolution[] resolutions;

    public MenuState menuState;
    public OptionsState optionsState;

    void Start()
    {
        // creating resolution thingy, thanks brackeys
        resolutions = Screen.resolutions;
        
        resDropdown.ClearOptions();

        List<string> options = new List<string>();

        int curResIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                curResIndex = i;
        }
        resDropdown.AddOptions(options);
        resDropdown.value = curResIndex;
        resDropdown.RefreshShownValue();

        menuState = MenuState.START;
        StartMenu();
    }

    void StartMenu()
    {
        button.text =  "Start";
        rightArrow.SetActive(true);
        leftArrow.SetActive(false);
    }

    public void NextState()
    {
        if (menuState == MenuState.START)
        {
            menuState = MenuState.OPTIONS;
            OptionsMenu();
        }
        else if (menuState == MenuState.OPTIONS)
        {
            menuState = MenuState.EXIT;
            ExitMenu();
        }

        if (optionsState == OptionsState.RES)
        {
            optionsState = OptionsState.FUL;
            FulMenu();
        }
        else if (optionsState == OptionsState.FUL)
        {
            optionsState = OptionsState.VOL;
            VolMenu();
        }
    }

    public void PrevState()
    {
        if (menuState == MenuState.OPTIONS)
        {
            menuState = MenuState.START;
            StartMenu();
        }
        else if (menuState == MenuState.EXIT)
        {
            menuState = MenuState.OPTIONS;
            OptionsMenu();
        }

        if (optionsState == OptionsState.FUL)
        {
            optionsState = OptionsState.RES;
            ResMenu();
        }
        else if (optionsState == OptionsState.VOL)
        {
            optionsState = OptionsState.FUL;
            FulMenu();
        }
    }

    void OptionsMenu()
    {
        button.text = "Options";
        rightArrow.SetActive(true);
        leftArrow.SetActive(true);
    }

    void ExitMenu()
    {
        button.text =  "Exit";
        rightArrow.SetActive(false);
        leftArrow.SetActive(true);
    }

    public void OpenOptions()
    {
        title.text = "Options";
        title2.text = "Options";
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        optionsState = OptionsState.RES;
        ResMenu();
    }

    void ResMenu()
    {
        optionsText.text = "Resolution";
        fullscreen.SetActive(false);
        volume.SetActive(false);
        resolution.SetActive(true);
        rightArrow.SetActive(true);
        leftArrow.SetActive(false);
    }

    void FulMenu()
    {
        optionsText.text = "Fullscreen";
        volume.SetActive(false);
        resolution.SetActive(false);
        fullscreen.SetActive(true);
        rightArrow.SetActive(true);
        leftArrow.SetActive(true);
    }

    void VolMenu()
    {
        optionsText.text = "Volume";
        resolution.SetActive(false);
        fullscreen.SetActive(false);
        volume.SetActive(true);
        rightArrow.SetActive(false);
        leftArrow.SetActive(true);
    }

    public void buttonPress()
    {
        if (menuState == MenuState.OPTIONS)
        {

            OpenOptions();
        }
    }

    public void SetVolume(float volumeSlider)
    {
        audioMixer.SetFloat("volume", volumeSlider);
    }

    public void setFullScreen (bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution (int resIndex)
    {
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
