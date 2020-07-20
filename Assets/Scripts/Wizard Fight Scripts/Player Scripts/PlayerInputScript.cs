using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem;
/*
 * This script accepts input from the player and transmits it through the PlayerScript to control the animations 
 * and the game logic that controls the character in the game world.
 * This script also controls the state machine that governs what behavior the character exhibits during the update function.
 */
public class PlayerInputScript : MonoBehaviour
{

    Vector2 movementInput;
    Vector2 runInput;
    float cancelSpell;
    float jumpInput;
    float[] spellInputTable;
    [SerializeField]
    internal PlayerInputActions inputAction;
    [SerializeField]
    internal PlayerScript playerScript;
    [SerializeField]
    internal int playerNumber = -1;
    Rigidbody playerRB;
    InputUser _user;
    internal IPlayerState currentState;

    private void Awake()
    {
        //Default player to player 1 (which is 0 in the gamepad order)
        if (playerNumber == -1)
        {
            playerNumber = 0;
        }
        playerScript = GetComponent<PlayerScript>();
        playerRB = GetComponent<Rigidbody>();
        inputAction = new PlayerInputActions();
        spellInputTable = new float[4];

        //Here we handle multiple users
        _user = new InputUser();
        if (playerNumber>=Gamepad.all.Count)
        {
            //This means that somehow, there are more players in the game then gamepads
            //Remember to throw an exception
            playerNumber = 0;
        }
        _user = InputUser.PerformPairingWithDevice(Gamepad.all[playerNumber]);
        _user.AssociateActionsWithUser(inputAction);

        currentState = new PlayerStateIdle(this);
        //Setup input for horizontal movement value press
        inputAction.PlayerControls.Walk.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        //Setup input for horizontal movement release
        inputAction.PlayerControls.Walk.canceled += ctx => movementInput = ctx.ReadValue<Vector2>();

        //Setup input for run value press
        inputAction.PlayerControls.Run.performed += ctx => runInput = ctx.ReadValue<Vector2>();
        //Setup input for run value release
        inputAction.PlayerControls.Run.canceled += ctx => runInput = ctx.ReadValue<Vector2>();

        //Setup input for jump values press
        inputAction.PlayerControls.Jump.performed += ctx => jumpInput = ctx.ReadValue<float>();
        //Setup input for jump value release
        inputAction.PlayerControls.Jump.canceled += ctx => jumpInput = ctx.ReadValue<float>();

        //Setup input for jump values press
        inputAction.PlayerControls.Cancel.performed += ctx => cancelSpell = ctx.ReadValue<float>();
        //Setup input for jump value release
        inputAction.PlayerControls.Cancel.canceled += ctx => cancelSpell = ctx.ReadValue<float>();

        //Setup input for jump movement values
        inputAction.PlayerControls.Spell0.performed += ctx => spellInputTable[0] = ctx.ReadValue<float>();
        inputAction.PlayerControls.Spell0.canceled += ctx => spellInputTable[0] = ctx.ReadValue<float>();

        inputAction.PlayerControls.Spell1.performed += ctx => spellInputTable[1] = ctx.ReadValue<float>();
        inputAction.PlayerControls.Spell1.canceled += ctx => spellInputTable[1] = ctx.ReadValue<float>();

        inputAction.PlayerControls.Spell2.performed += ctx => spellInputTable[2] = ctx.ReadValue<float>();
        inputAction.PlayerControls.Spell2.canceled += ctx => spellInputTable[2] = ctx.ReadValue<float>();

    }

    void Start()
    {

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //The player's actions are controlled through two functions.
        //State update is what actually happens each frame during the state.
        currentState.StateUpdate();
        //
        currentState.HandleInput();
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

    /*
     * Get_Input functions are used by the state classes to determine when to switch states.
     */
    public Vector2 GetMovementInput()
    {
        return movementInput;
    }

    public Vector2 GetRunInput()
    {
        return runInput;
    }

    public float GetJumpInput()
    {
        return jumpInput;
    }

    public float GetCancelInput()
    {
        return cancelSpell;
    }

    public float[] GetSpellInputTable()
    {
        return spellInputTable;
    }

    //Assigns the attached character gameobject a player number
    public void SetPlayerNumber(int p)
    {
        playerNumber = p;
        _user = new InputUser();
        if (playerNumber >= Gamepad.all.Count)
        {
            //This means that somehow, there are more players in the game then gamepads
            //Remember to throw an exception
            playerNumber = 0;
        }
        _user = InputUser.PerformPairingWithDevice(Gamepad.all[playerNumber]);
        _user.AssociateActionsWithUser(inputAction);
    }
    //###

    /*Used in idle and walking states to determine if the player can jump again 
     * (if the player is on the ground and has stopped ariel movement).*/
    public bool IsGrounded()
    {
        Debug.Log(playerRB.velocity.y);
        if (Mathf.Approximately(playerRB.velocity.y, 0.0f))
            return true;
        else
            return false;
    }

    public void Death()
    {
        currentState = new PlayerStateDeath(this);
    }
}