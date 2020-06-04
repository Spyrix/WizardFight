using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class BattleCameraController : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject[] players;
    Camera c;
    Vector3 cameraOffset = new Vector3(0,20f,-20f);
    private Vector3 velocity;
    public float smoothTime = .5f;
    public float minZoom = 200f;
    public float maxZoom = 40f;
    public float zoomLimit = 100f;
    void Awake()
    {
        //Get all the active players in the scene and put them in the list
        players = GameObject.FindGameObjectsWithTag("WizardCharacter");
        c = GetComponent<Camera>();
    }

    // LateUpdate is called once per frame after update, because Update is where we move characters
    void LateUpdate()
    {
        players = GameObject.FindGameObjectsWithTag("WizardCharacter");
        //AdjustCamera(GetPlayersWithGreatestDistance());
        moveCamera(GetPlayersWithGreatestDistance());
        zoomCamera(GetPlayersWithGreatestDistance());
    }

    Vector3[] GetPlayersWithGreatestDistance()
    {
        Vector3[] playerPositions = new Vector3[2];
        float greatestDistance = 0;
        //Compare each player to each other to get the greatest distance between two players
        for (int i = 0; i < players.Length; i++)
        {
            for (int j = 0; j < players.Length; j++)
            {
                //Same gameobject
                if (i == j)
                    continue;
                float distance = Vector3.Distance(players[i].transform.position, players[j].transform.position);
                if (distance > greatestDistance)
                {
                    greatestDistance = distance;
                    playerPositions[0] = players[i].transform.position;
                    playerPositions[1] = players[j].transform.position;
                }
            }
        }
        return playerPositions;
    }

    void moveCamera(Vector3[] furthestPlayers)
    {
        if (players.Length == 1)
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

    void zoomCamera(Vector3[] furthestPlayers)
    {
        float distanceBetweenPlayers = Vector3.Distance(furthestPlayers[0], furthestPlayers[1]);
        Debug.Log(distanceBetweenPlayers);
        //Add 12f to new zoom so that it doesn't totally ofuscate the menu bars
        float newZoom = Mathf.Lerp(maxZoom,minZoom,distanceBetweenPlayers/zoomLimit)+12f;
        //float newZoom = Mathf.Lerp(maxZoom,minZoom,300f);
        c.fieldOfView = Mathf.Lerp(c.fieldOfView,newZoom,Time.deltaTime);
    }

    /*
    void AdjustCamera(Vector3[] furthestPlayers)
    {
        if (players.Length == 1)
        {
            transform.RotateAround(players[0].GetComponent<Transform>().position, Vector3.up, 30 * Time.deltaTime);
            //c.transform.LookAt(players[0].GetComponent<Transform>().position);
        }
        else {
            float distanceBetweenPlayers = Vector3.Distance(furthestPlayers[0], furthestPlayers[1]);
            //position the camera at the middle    
            Vector3 midPoint = (furthestPlayers[0] + furthestPlayers[1]) * 0.5f;
            Vector3 newCameraPos = c.transform.position;
            newCameraPos.x = midPoint.x;
            //Camera should maintain a min distance of 10 from the midpoint between players, max of 30
            //This may be subject to change
            if (midPoint.z-c.transform.position.z<10f)
            {
                newCameraPos.z -= 1f;
            }
            if (midPoint.z-c.transform.position.z > 30f)
            {
                newCameraPos.z += 1f;
            }
            //The distance between players should determine how high the camera is
            c.transform.position = newCameraPos;

            //find the distance between 2 players
  

            const float DISTANCE_MARGIN = 10.0f;
            float aspectRatio = Screen.width / Screen.height;
            float tanFov = Mathf.Tan(Mathf.Deg2Rad * c.fieldOfView / 2.0f);

            //calculate new camera distance
            float cameraDistance = (distanceBetweenPlayers / 2.0f / aspectRatio) / tanFov;
            // Set camera to new position.
            Vector3 dir = (c.transform.position - midPoint).normalized;
            c.transform.LookAt(midPoint);
            c.transform.position = midPoint + dir * (cameraDistance + DISTANCE_MARGIN);
        }
    }*/
}
