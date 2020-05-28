using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MainMenuScript : MonoBehaviour
{
    /*
     * This script/game object is responsible for managing the UI of the main menu.
     * 
     * It also holds information about player 
     * 
     * */

    //Canvases
    GameObject[] allCanvases;
    [SerializeField]
    internal GameObject StartMenuCanvas;
    [SerializeField]
    internal GameObject ModeSelectMenuCanvas;
    [SerializeField]
    internal GameObject RegularSpellSelectCanvas;
    [SerializeField]
    internal GameObject StageSelectCanvas;

    //Buttons to default menu cursors at
    [SerializeField]
    internal GameObject RegularBattleButton;
    [SerializeField]
    internal GameObject PlaySandboxButton;
    [SerializeField]
    internal GameObject BackToModeSelectButton;
    [SerializeField]
    internal GameObject SandboxStageButton;

    [SerializeField]
    internal GameObject[] playerSelectCursors;

    private void Awake()
    {
        allCanvases = new GameObject[]
        {
            StartMenuCanvas,ModeSelectMenuCanvas,RegularSpellSelectCanvas
        };
        playerSelectCursors = new GameObject[4];

        //Default to the start menu being active
        foreach (GameObject g in allCanvases)
        {
            g.SetActive(false);
        }
        StartMenuCanvas.SetActive(true);
    }

    private void Update()
    {
        //We only care about handling player spell select cursors if the regular 
        if (RegularSpellSelectCanvas && RegularSpellSelectCanvas.activeSelf == true)
        {
            HandleSpellSelectCursors();
        }
    }

    void HandleSpellSelectCursors()
    {
        //Every frame, we need to check if a gamepad controller was unplugged. or plugged in
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            //If there was a gamepad that was plugged in at one point, and thus has a cursor, and is no longer plugged in, destroy that cursor
            if (Gamepad.all[i] == null && playerSelectCursors[i] != null)
            {
                Destroy(playerSelectCursors[i]);
            }
            //If a gamepad has been plugged in, but there is no associated cursor, create one
            if (Gamepad.all[i] != null && playerSelectCursors[i] == null)
            {
                GameObject g = Instantiate(Resources.Load<GameObject>("Prefabs/SpellSelectCursor"));
                g.GetComponent<SpellSelectScript>().SetPlayerNumber(i);
                playerSelectCursors[i] = g;
            }
        }
    }

    bool CheckRegularBattleReady()
    {
        bool ready = true;
        //Check if all players are ready for regular battle
        //For each player cursor
        for (int i = 0; i < playerSelectCursors.Length; i++)
        {
            if (playerSelectCursors[i] != null)
            {
                GameObject[] playerSpells = playerSelectCursors[i].GetComponent<SpellSelectScript>().GetSpellSelection();
                for (int s = 0; s < playerSpells.Length; s++)
                {
                    if (playerSpells[s] == null)
                    {
                        ready = false;
                    }
                }
            }
        }
        return ready;
    }

    //****
    //BUTTON FUNCTIONS
    //****
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
        foreach (GameObject g in allCanvases)
        {
            g.SetActive(false);
        }
        ModeSelectMenuCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(RegularBattleButton);
    }

    public void ButtonRegularBattle()
    {
        //So we need to do a couple of things
        //First we need to determine how many players are currently playing, and create entites for each player that allow them to select spells.
        //Then, we need to enable the spell selection canvas. The gameobjects that represent the different spells will be present on the canvas.
        //The spell selection entities, (tied to each active player) will then be able to select spells by clicking on the icons.
        foreach (GameObject g in allCanvases)
        {
            g.SetActive(false);
        }
        RegularSpellSelectCanvas.SetActive(true);
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            if (Gamepad.all[i] != null)
            {
                //create a player select cursor and keep track of it
                GameObject g = Instantiate(Resources.Load<GameObject>("Prefabs/SpellSelectCursor"));
                g.GetComponent<SpellSelectScript>().SetPlayerNumber(i);
                playerSelectCursors[i] = g;
            }
        }
        //Set the default button
        EventSystem.current.SetSelectedGameObject(BackToModeSelectButton);
    }

    public void ButtonStageSelection()
    {
        if (CheckRegularBattleReady())
        {
            //Takes us to the stage select screen
            foreach (GameObject g in allCanvases)
            {
                g.SetActive(false);
            }
            StageSelectCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(SandboxStageButton);

            //Do NOT delete the cursors during the stage select, as that is where we are storing
            //player selection data. Just make it so that they don't appear.
            HideSpellCursors();
            EventSystem.current.SetSelectedGameObject(SandboxStageButton);
        }
    }

    public void ButtonPlaySandboxStage()
    {
        SceneManager.LoadScene("SandboxScene");
    }

    //Return buttons
    public void ReturnToSpellSelectButton()
    {
        //Takes us from the stage select screen back to spell select
        foreach (GameObject g in allCanvases)
        {
            g.SetActive(false);
        }
        RegularSpellSelectCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(BackToModeSelectButton);

        //Re-enable the player select cursor, since we disabled them earlier
        ShowSpellCursors();
           // foreach (GameObject g in playerSelectCursors)
           // {
           // if (g != null)
           //  g.GetComponent<MeshRenderer>().enabled = false;
           // }
    }

    public void ReturnToStartButton()
    {
        foreach (GameObject g in allCanvases)
        {
            g.SetActive(false);
        }
        StartMenuCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(PlaySandboxButton);
    }

    public void ReturnToModeSelectButton()
    {
        foreach (GameObject g in allCanvases)
        {
            g.SetActive(false);
        }
        ModeSelectMenuCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(RegularBattleButton);
        //Going back to mode select from the regular battle spell select, so gotta get rid of the spell select cursors
        for (int i = 0; i < playerSelectCursors.Length; i++)
        {
            Destroy(playerSelectCursors[i]);
        }
    }

    public void HideSpellCursors()
    {
        foreach (GameObject g in playerSelectCursors)
        {
            if (g != null)
            {
                g.GetComponent<MeshRenderer>().enabled = false;
                //also disable the icons
                for(int c = 0; c < g.transform.childCount; c++)
                {
                    g.transform.GetChild(c).gameObject.SetActive(false);
                }
            }
        }
    }


    public void ShowSpellCursors()
    {
        foreach (GameObject g in playerSelectCursors)
        {
            if (g != null)
            {
                g.GetComponent<MeshRenderer>().enabled = true;
                //also disable the icons
                for (int c = 0; c < g.transform.childCount; c++)
                {
                    g.transform.GetChild(c).gameObject.SetActive(true);
                }
            }
        }
    }
}