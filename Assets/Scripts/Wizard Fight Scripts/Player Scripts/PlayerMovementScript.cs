using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class PlayerMovementScript : MonoBehaviour
{
    float walkSpeed = 6f;
    float runSpeed = 4f;
    float jumpSpeed = 0.3f;
    [SerializeField]
    float movementSpeed;
    float turnSpeed = 100f;


    [SerializeField]
    internal Transform playerTransform;
    [SerializeField]
    internal PlayerScript playerScript;
    internal Rigidbody playerRB;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GetComponent<Transform>();
        playerRB = GetComponent<Rigidbody>();
        playerScript = GetComponent<PlayerScript>();
        movementSpeed = walkSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    internal void GroundMovement(float x, float y)
    {
        float forwardMovement = movementSpeed * Time.deltaTime;

        Vector3 movementVector = new Vector3(x, 0f, y);

        //move player
        playerTransform.Translate(movementVector * forwardMovement, Space.World);
        movementSpeed = walkSpeed;
    }
    internal void RotatePlayer(float x, float y)
    {
        //Rotate player, so long as the vector isn't 0. If it's zero, it just resets to facing in the default.
        if (x != 0f || y != 0f)
           playerTransform.rotation = Quaternion.LookRotation(new Vector3(x, 0f, y));
            
        //Debug.Log("debug transform forward"+playerTransform.forward);
    }

    internal void Run(float runInput)
    {
        //Debug.Log(runInput);
        if(runInput > 0)
            movementSpeed = walkSpeed + runSpeed;
    }

    internal void Jump(bool walking)
    {
        float xMovement = 0.0f;
        float zMovement = 0.0f;
        if (walking)
        {
            float forwardMovement = movementSpeed * Time.deltaTime;
            xMovement = playerTransform.forward.x * forwardMovement;
            zMovement = playerTransform.forward.z * forwardMovement;
        }
        playerTransform.Translate(new Vector3(xMovement, jumpSpeed, zMovement), Space.World);
    }
}
