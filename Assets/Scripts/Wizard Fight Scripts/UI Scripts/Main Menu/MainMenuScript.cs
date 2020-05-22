using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    IMenuState currentState;
    // Start is called before the first frame update
    void Start()
    {
        currentState = new StartMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

/*
 * Menu state Machine:
 * 
 * */
interface IMenuState
{
    void HandleInput();
    void Update();
}

public class StartMenu : IMenuState
{
    /*
     * Menu that goes into selecting a playmode
     * */
    public StartMenu()
    {

    }

    public void HandleInput()
    {

    }

    public void Update()
    {

    }
}

public class PlayModeMenu : IMenuState
{
    /*
     * Selecting a playmode. 
     * Right now, only RegularPick is implemented
     * Eventually there will be more
     * */
    public PlayModeMenu()
    {

    }

    public void HandleInput()
    {

    }

    public void Update()
    {

    }
}

public class RegularPickBattleMenu : IMenuState
{
    /*
     * This mode will either go into the stageselectmenu state, or switch back to the playmodemenu.
     * Regular Pick works as such:
     * Players select three spells that they want to prepare to take into battle with them.
     * No one player may select the same spell twice, but mutliple players may select the same spell.
     * After all players have selected three spells, any one player may press start to move into the battle arena selection screen.
     * */
    public RegularPickBattleMenu()
    {

    }

    public void HandleInput()
    {

    }

    public void Update()
    {

    }
}

public class StageSelectionMenu : IMenuState
{
    /*
     * This state will either revert back to whatever previous state it was in, or switch into the scene containing the stage selected.
     * 
     * */
    public StageSelectionMenu()
    {

    }

    public void HandleInput()
    {

    }

    public void Update()
    {

    }
}