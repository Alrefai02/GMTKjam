using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MenuState { START, OPTIONS, EXIT}
public class UIStuff : MonoBehaviour
{
    public GameObject rightArrow;
    public GameObject leftArrow;

    public Text button;

    public MenuState menuState;

    void Start()
    {
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
}
