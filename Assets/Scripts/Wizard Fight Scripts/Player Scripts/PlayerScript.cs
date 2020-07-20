using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * This is the manager script for controlling everything that the player can do. 
 * If the input script wants to talk to the movement script, it must go through this script, for example.
 * This is to segregate code and make sure that we know exactly what is responsible for doing what.
 * Essentially, this is the bridge between input, the game logic, and the animations.
 * 
 * In theory, attaching this script to any player character gameobject should make it controllable by the player
 * due to the fact that it requires so many components.
 */
[RequireComponent(typeof(PlayerInputScript))]
[RequireComponent(typeof(PlayerMovementScript))]
[RequireComponent(typeof(PlayerCollisionScript))]
[RequireComponent(typeof(PlayerSpellcasting))]
[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerHealthController))]
[RequireComponent(typeof(MeshCollider))]
public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    internal PlayerInputScript inputScript;
    [SerializeField]
    internal PlayerMovementScript movementScript;
    [SerializeField]
    internal PlayerCollisionScript collisionScript;
    [SerializeField]
    internal PlayerSpellcasting spellcastingScript;
    [SerializeField]
    internal Transform playerTransform;
    [SerializeField]
    internal PlayerAnimation animationScript;
    [SerializeField]
    internal PlayerHealthController healthScript;
    // Start is called before the first frame update
    void Start()
    {
        //Initialize things, set necessary variables.
        inputScript = GetComponent<PlayerInputScript>();
        movementScript = GetComponent<PlayerMovementScript>();
        collisionScript = GetComponent<PlayerCollisionScript>();
        spellcastingScript = GetComponent<PlayerSpellcasting>();
        animationScript = GetComponent<PlayerAnimation>();
        healthScript = GetComponent<PlayerHealthController>();
        playerTransform = GetComponent<Transform>();
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        GetComponent<MeshCollider>().convex = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public void adjustHealth(float value)
    {
        healthScript.addHealth(value);
    }

    internal void GroundMovement(float x, float y, float movementInput)
    {
        movementScript.GroundMovement(x, y);
        movementScript.RotatePlayer(x, y);
        //Debug.Log("movementinput float:"+ movementInput);
        animationScript.EnterWalkingState(movementInput);
    }

    internal void Run(float runInput)
    {
        movementScript.Run(runInput);
    }

    internal void Idle()
    {
        animationScript.LeaveJumpState();
        animationScript.LeaveWalkingState();
        animationScript.LeaveSpellAimingState();
    }

    internal void SpellAim(float playerLook_x, float playerLook_y)
    {
        //Playerlook variables tell us what direction to aim in

        animationScript.LeaveWalkingState();
        animationScript.EnterSpellAimingState();
        movementScript.RotatePlayer(playerLook_x, playerLook_y);
    }

    internal void StartAiming(int sn)
    {
        spellcastingScript.StartAiming(sn);
    }

    internal void EndAiming()
    {
        spellcastingScript.EndAiming();
    }

    internal void CastSpell(int spellNumber)
    {
        spellcastingScript.Cast(spellNumber, playerTransform);
    }

    internal void Jump(bool walking, float jumpTimer, Vector2 movementInput)
    {
        movementScript.Jump(walking, jumpTimer, movementInput);
        animationScript.PlayJump();
    }

    //Starts the death process
    internal void Death()
    {
        inputScript.Death();
        animationScript.PlayDeath();
        StartCoroutine(WaitToDestroy(2f));
    }

    //Returns player health
    public float GetPlayerHealth()
    {
        return healthScript.GetPlayerHealth();
    }

    //Gets the player's attatched transform.
    public Transform GetPlayerTransform()
    {
        return playerTransform;
    }

    IEnumerator WaitToDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    /*This is a debug function for the player state machine
     * The gizmo will change a different color depending on the state
     * internal void OnDrawGizmos()
    {
        float radius = 0.5f;
        Gizmos.color = inputScript.currentState.GetColor();
        Vector3 center = transform.position + new Vector3(0,3,0);
        Gizmos.DrawSphere(center, radius);
    }*/
}

