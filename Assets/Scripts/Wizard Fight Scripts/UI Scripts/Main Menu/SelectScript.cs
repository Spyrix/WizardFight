using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectScript : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        CreateShape();
        UpdateMesh();

        //Input for controlling the arrow

    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateShape()
    {
        vertices = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(1,0,0),
            new Vector3(-1,1,0),
            new Vector3(-1,-1,0),
        };

        triangles = new int[]
        {
            0,1,2,
            0,1,3
        };
    }

    void UpdateMesh()
    {
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}
