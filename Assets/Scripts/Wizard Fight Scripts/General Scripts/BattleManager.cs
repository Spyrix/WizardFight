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
    internal Vector3[] playerSpawnPoints;


    //list of all active players in scene
    GameObject[] players;
    bool gameOver = false;
    // Start is called before the first frame update
    void Awake()
    {
        playerSpawnPoints = new Vector3[] 
        {
            new Vector3(7,.2f,-15),
            new Vector3(48,-.2f,-15),
            new Vector3(7,-.2f,-30),
            new Vector3(48,-.2f,-30),
        };
        players = new GameObject[4];
        GameObject[] playerInfo = GameObject.FindGameObjectsWithTag("SpellSelectCursor");
        Debug.Log(playerInfo.Length);
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
            //Get rid of extraneous cursor object
            //Destroy(playerInfo[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        VictoryCondition();
    }

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
