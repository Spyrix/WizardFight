using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField]
    internal GameObject StartMenuCanvas;
    [SerializeField]
    internal GameObject ModeSelectMenuCanvas;
    [SerializeField]
    internal GameObject RegularBattleButton;
    [SerializeField]
    internal GameObject PlaySandboxButton;


    public void ButtonStartSandbox()
    {
        SceneManager.LoadScene("SandboxScene");
    }

    public void ButtonQuit()
    {
        Application.Quit();
    }

    public void ButtonStartGame()
    {
        //Takes us to the mode select Menu
        StartMenuCanvas.SetActive(false);
        ModeSelectMenuCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(RegularBattleButton);
    }

    public void ButtonRegularBattle()
    {
        //So we need to do a couple of things
        //First we need to determine how many players are currently playing, and create entites for each player that allow them to select spells
        //Then, we need to enable the spell selection canvas
        //Then, 
    }

    public void ButtonStageSelection()
    {
        //Takes us to the stage select screen
    }

    public void ReturnToStartButton()
    {
        StartMenuCanvas.SetActive(true);
        ModeSelectMenuCanvas.SetActive(false);
        EventSystem.current.SetSelectedGameObject(PlaySandboxButton);
    }

}