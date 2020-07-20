using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * This class controls the camera during gameplay.
 */

[RequireComponent(typeof(Camera))]
public class BattleCameraController : MonoBehaviour
{
    // Start is called before the first frame update
    List<GameObject> players;
    Camera c;
    [SerializeField]
    Vector3 cameraOffset = new Vector3(0,20f,-20f);
    private Vector3 velocity;
    public float smoothTime = .5f;
    [SerializeField]
    public float minZoom = 200f;
    [SerializeField]
    public float maxZoom = 40f;
    [SerializeField]
    public float zoomLimit = 100f;
    [SerializeField]
    public float zoomOffSet = 12f;
    void Awake()
    {
        //Get all the active players in the scene and put them in the list
        players = GameObject.FindGameObjectsWithTag("WizardCharacter").ToList<GameObject>();
        c = GetComponent<Camera>();
    }

    // LateUpdate is called once per frame after update, because Update is where we move characters. So we don't want it to stutter
    void LateUpdate()
    {
        //Ensure that you're always getting the latest list of updated players
        players = GameObject.FindGameObjectsWithTag("WizardCharacter").ToList<GameObject>();
        //Update the two furthest players
        Vector3[] furthestPlayers = GetPlayersWithGreatestDistance();
        moveCamera(furthestPlayers);
        zoomCamera(furthestPlayers);
    }

    //Returns the position of the furthest players
    Vector3[] GetPlayersWithGreatestDistance()
    {
        Vector3[] playerPositions = new Vector3[2];
        float greatestDistance = 0;
        //Compare each player to each other to get the greatest distance between two players
        for (int i = 0; i < players.Count; i++)
        {
            //Using Linq to compare each player with every other player, ordering by distance from closest to furthest away and returning the furthest player
            GameObject furthestPlayer = players.OrderBy(t => Vector3.Distance(players[i].transform.position, t.transform.position)).Last<GameObject>();//Because we assume the last element is the greatest in asc order
            float distance = Vector3.Distance(players[i].transform.position, furthestPlayer.transform.position);
            //Update the greatest distance and the furthest distance away
            if (distance > greatestDistance)
            {
                greatestDistance = distance;
                playerPositions[0] = players[i].transform.position;
                playerPositions[1] = furthestPlayer.transform.position;
            }
        }
        return playerPositions;
    }

    //Moves the camera
    void moveCamera(Vector3[] furthestPlayers)
    {
        if (players.Count == 1)
        {
            //Look at the victor
            transform.RotateAround(players[0].GetComponent<Transform>().position, Vector3.up, 30 * Time.deltaTime);
            c.transform.LookAt(players[0].GetComponent<Transform>().position);
            //c.transform.LookAt(players[0].GetComponent<Transform>().position);
        }
        else
        {
            //Adjust offset
            Vector3 midPoint = (furthestPlayers[0] + furthestPlayers[1]) * 0.5f;
            Vector3 newPosition = midPoint + cameraOffset;
            c.transform.position = Vector3.SmoothDamp(c.transform.position, newPosition, ref velocity, smoothTime);
        }
    }

    //Adjusts the camera zoom
    void zoomCamera(Vector3[] furthestPlayers)
    {
        float distanceBetweenPlayers = Vector3.Distance(furthestPlayers[0], furthestPlayers[1]);
        Debug.Log(distanceBetweenPlayers);
        //Add 12f to new zoom so that it doesn't totally ofuscate the menu bars
        float newZoom = Mathf.Lerp(maxZoom,minZoom,distanceBetweenPlayers/zoomLimit)+zoomOffSet;
        //float newZoom = Mathf.Lerp(maxZoom,minZoom,300f);
        c.fieldOfView = Mathf.Lerp(c.fieldOfView,newZoom,Time.deltaTime);
    }
}
