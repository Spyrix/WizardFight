using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class AimReticleController : MonoBehaviour,IAimPrefab
{
    // Start is called before the first frame update
    Mesh mesh;

    Vector3[] vertices;
    [SerializeField]
    internal PlayerInputScript inputScript;
    [SerializeField]
    GameObject player;
    int[] triangles;
    float maxDistanceFromPlayer = 15f;

    Vector2 movementInput = new Vector2();
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        UpdateMesh();
        transform.position = player.transform.position; 
        inputScript = player.GetComponent<PlayerInputScript>();
    }

    // Update is called once per frame
    void Update()
    {
        movement();
    }

    void CreateShape()
    {
        vertices = new Vector3[]
        {
            new Vector3(0,0,0),

            new Vector3(-.5f,0,1),
            new Vector3(.5f,0,1),

            new Vector3(-.5f,0,-1),
            new Vector3(.5f,0,-1),

            new Vector3(-1,0,.5f),
            new Vector3(-1,0,-.5f),

            new Vector3(1,0,.5f),
            new Vector3(1,0,-.5f),

        };

        triangles = new int[]
        {
            0,1,2,
            0,3,4,
            0,5,6,
            0,7,8
        };
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    internal void movement()
    {

        if (Mathf.Abs(transform.position.x - player.transform.position.x) > maxDistanceFromPlayer)
        {
            float offset = 1f;
            if (transform.position.x < player.transform.position.x)
                offset *= -1;
            transform.position = new Vector3(transform.transform.position.x - offset, transform.position.y, transform.position.z);
        }
        else if (Mathf.Abs(transform.position.z - player.transform.position.z) > maxDistanceFromPlayer)
        {
            float offset = 1f;
            if (transform.position.z < player.transform.position.z)
                offset *= -1;
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.transform.position.z - offset);
        }

        Vector2 m = inputScript.GetMovementInput();
        float forwardMovement = 20f * Time.deltaTime;
        Vector3 movementVector = new Vector3(m.x, 0f, m.y);
        transform.Translate(movementVector * forwardMovement, Space.World);
    }
    public void SetPlayer(GameObject p)
    {
        player = p;
    }
    public GameObject GetPlayer()
    {
        return player;
    }
}
