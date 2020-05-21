using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class AimArrowController : MonoBehaviour,IAimPrefab
{
    // Start is called before the first frame update
    Mesh mesh;

    Vector3[] vertices;
    [SerializeField]
    internal GameObject player;
    int[] triangles;
    float scaleMaximumZ = 5f;
    float scaleChange = 0f;
    float scaleChangeSpeed = 5f;
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        UpdateMesh();
        transform.SetParent(player.transform);
    }

    // Update is called once per frame
    void Update()
    {
        //Change scaling
        scaleChange = Time.deltaTime*scaleChangeSpeed;
        if (transform.localScale.z > scaleMaximumZ)
            scaleChange = 0f;
        transform.localScale = new Vector3(transform.localScale.x,transform.localScale.y,transform.localScale.z + scaleChange);
        transform.position = player.transform.position;

        transform.forward = player.transform.forward;
    }

    void CreateShape()
    {
        vertices = new Vector3[]
        {
            new Vector3(-.5f,0,0),
            new Vector3(.5f,0,0),
            new Vector3(-1,0,1),//new Vector3(-1,0,1),
            new Vector3(-.5f,0,1),
            new Vector3(1,0,1),//new Vector3(1,0,1),
            new Vector3(.5f,0,1),
            new Vector3(0,0,3),//new Vector3(0,0,2)
        };

        triangles = new int[]
        {
            6,2,4,
            0,1,3,
            1,3,5
        };
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    public void SetPlayer(GameObject go)
    {
        player = go;
    }

    public GameObject GetPlayer()
    {
        return player;
    }
}
