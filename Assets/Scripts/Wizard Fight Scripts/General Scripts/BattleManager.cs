using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    /*This script/object belongs on every stage, and it's responsibility is to set up the players for gameplay.
     *This means instiating the players at the correct spawn points, giving them the correct spells that they selected,
     * ensuring that when the player 
     * */
    [SerializeField]
    internal List<Vector3> playerSpawnPoints;
    PlayerInputActions inputAction;
    [SerializeField]
    internal GameObject pauseCanvas;

    //list of all active players in scene
    [SerializeField]
    internal GameObject[] players;
    bool gameOver = false;
    [SerializeField]
    internal bool togglePause = true;
    [SerializeField]
    internal float pauseInput;
    // Start is called before the first frame update
    void Awake()
    {
        if(players.Length == 0)//For debug, sometimes there may already be players in the array for debug purposes
            players = new GameObject[4];
        GameObject[] playerInfo = GameObject.FindGameObjectsWithTag("SpellSelectCursor");
        for (int i = 0; i< playerInfo.Length; i++)
        {
            //create player in world at spawn point
            GameObject player = Instantiate(Resources.Load<GameObject>("Prefabs/Wizard"),playerSpawnPoints[i],Quaternion.identity);
            //Add the player's selected spells to the created player object
            GameObject[] playerSpells = playerInfo[i].GetComponent<SpellSelectScript>().GetSpellSelection();
            for (int s = 0; s < playerSpells.Length; s++)
            {
                player.GetComponent<PlayerSpellcasting>().AddSpellToSlot(playerSpells[s]);
            }
            //Track player
            players[i] = player;
            players[i].GetComponent<PlayerInputScript>().SetPlayerNumber(i);
            //Get rid of extraneous cursor object
            Destroy(playerInfo[i]);
        }
        //Get Pause input
        inputAction = new PlayerInputActions();
        inputAction.PlayerControls.Pause.performed += ctx => pauseInput = ctx.ReadValue<float>();
        inputAction.PlayerControls.Pause.canceled += ctx => pauseInput = ctx.ReadValue<float>();
    }

    //OnEnable and OnDisable are required for the inputAction class to work
    private void OnEnable()
    {
        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.Disable();
    }


    // Update is called once per frame
    void Update()
    {
        VictoryCondition();
        //Pausing mid battle
        if (pauseInput > 0 && togglePause && pauseCanvas.activeSelf == false)
        {
            Pause();
            togglePause = false;
        }
        if (pauseInput > 0 && togglePause && pauseCanvas.activeSelf == true)
        {
            UnPause();
            togglePause = false;
        }
        if (pauseInput == 0)
        {
            togglePause = true;
        }
    }

    //Pauses the game
    internal void Pause()
    {
        Time.timeScale = 0;
        pauseCanvas.SetActive(true);
    }

    //Unpauses the game
    internal void UnPause()
    {
        Time.timeScale = 1;
        pauseCanvas.SetActive(false);
    }

    //Checks if the victory condition is met
    //If so, return to main menu
    void VictoryCondition()
    {
        int activePlayers = 0;
        //Last Man standing
        foreach (GameObject g in players)
        {
            if (g != null)
                activePlayers++;
        }
        if (activePlayers <= 1 && gameOver == false)
        {
            StartCoroutine(ReturnToMainMenu());
            gameOver = true;
        }
    }

    IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("MainMenu");
    }
}
