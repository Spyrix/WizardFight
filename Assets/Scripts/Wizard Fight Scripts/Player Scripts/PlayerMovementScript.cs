using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script controls game logic for moving, rotating, and jumping the player.
 */

[RequireComponent(typeof(Transform))]
public class PlayerMovementScript : MonoBehaviour
{
    float walkSpeed = 6f;
    float runSpeed = 4f;
    float jumpSpeed = .05f;
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

    internal void Jump(bool walking, float jumpTimer, Vector2 movementInput)
    {
        float yMovement = Mathf.Pow(jumpTimer, -1) * jumpSpeed;
        playerTransform.Translate(new Vector3(0, yMovement, 0), Space.World);
        //Allow the player to move in the air because it's fun
        RotatePlayer(movementInput.x, movementInput.y);
        GroundMovement(movementInput.x, movementInput.y);
    }
}
