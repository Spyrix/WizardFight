using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem;

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
     //###
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

    public bool IsGrounded()
    {
        /*Used in idle and walking states to determine if the player can jump again 
         * (if the player is on the ground and has stopped ariel movement).*/
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
/*
 * 
 * State Design Pattern
 * Every state implenents the IPlayerState interface, which 
 * */
public interface IPlayerState
{
    void HandleInput();
    void StateUpdate();
    Color GetColor();
}

public class PlayerStateIdle : IPlayerState
{
    /*
     * The player starts in idle, and does not move.
     * */
    PlayerInputScript pi;
    Vector2 mi;
    Vector2 ri;
    float ji;
    float[] spellInputTable;
    /*The constructor takes in the player input script because it needs 
    * to be able to communicate with the player and accept data about the
    * player, like input etc*/
    public PlayerStateIdle(PlayerInputScript player)
    {
        pi = player;
    }
    //
    /*If the player is in this state, they will always do whatever is in state update.
     * Which includes accepting new input and */
    public void StateUpdate()
    {
        pi.playerScript.Idle();
        mi = pi.GetMovementInput();
        ri = pi.GetRunInput();
        ji = pi.GetJumpInput();
        spellInputTable = pi.GetSpellInputTable();
    }
    public void HandleInput()
    {
        if (mi.magnitude > 0f)
            pi.currentState = new PlayerStateWalking(pi);
        if (ji == 1f && pi.IsGrounded())
            pi.currentState = new PlayerStateJumping(pi,false);
        //check to see if a spell input was pressed
        for (int i = 0; i<spellInputTable.Length;i++)
        {
            if (spellInputTable[i] == 1f)
            {
                //Decide if the spell should be cast or is being aimed
                pi.currentState = new PlayerStateAiming(pi, i);
            }
        }
    }
    //Debug gizmo
    public Color GetColor()
    {
        return Color.red;
    }
}

public class PlayerStateWalking : IPlayerState
{
    /*
     * Purpose: To direct the player character in the direction that the player moves the left analog stick.
     * Step by step:
     * 
     */
    PlayerInputScript pi;
    Vector2 mi;
    Vector2 ri;
    float[] spellInputTable;
    float ji;
    public PlayerStateWalking(PlayerInputScript player)
    {
        pi = player;
    }
    public void StateUpdate()
    {
        mi = pi.GetMovementInput();
        ri = pi.GetRunInput();
        spellInputTable = pi.GetSpellInputTable();
        ji = pi.GetJumpInput();
        pi.playerScript.GroundMovement(mi.x, mi.y, mi.magnitude);
        pi.playerScript.Run(ri.magnitude);
    }
    public void HandleInput()
    {
        if (mi.magnitude == 0f)
            pi.currentState = new PlayerStateIdle(pi);
        //check to see if a spell input was pressed
        for (int i = 0; i < spellInputTable.Length; i++)
        {
            if (spellInputTable[i] == 1f)
            {
                //Decide if the spell should be cast, or is being aimed
                pi.currentState = new PlayerStateAiming(pi, i);
            }
        }
        if (ji == 1f && pi.IsGrounded())
            pi.currentState = new PlayerStateJumping(pi,true);
    }
    public Color GetColor()
    {
        return Color.blue;
    }
}

public class PlayerStateAiming : IPlayerState
{
    PlayerInputScript pi;
    Vector2 mi;
    float ci;
    int spellNum;
    float[] spellInputTable;
    public PlayerStateAiming(PlayerInputScript player, int sn)
    {
        pi = player;
        spellNum = sn;
        player.playerScript.StartAiming(sn);
    }
    public void StateUpdate()
    {
        mi = pi.GetMovementInput();
        ci = pi.GetCancelInput();
        //pass in the movement input vector so we know what direction to aim in
        pi.playerScript.SpellAim(mi.x,mi.y);
        spellInputTable = pi.GetSpellInputTable();
    }
    public void HandleInput()
    {
        if (spellInputTable[spellNum] == 0f)
        {
            pi.currentState = new PlayerStateCasting(pi, spellNum);
        }
        if (ci > 0)
        {
            pi.currentState = new PlayerStateIdle(pi);
            pi.playerScript.spellcastingScript.EndAiming();
        }
    }
    public Color GetColor()
    {
        return Color.yellow;
    }
}

public class PlayerStateCasting : IPlayerState
{
    PlayerInputScript pi;
    int spellNumber;
    public PlayerStateCasting(PlayerInputScript player, int sn)
    {
        pi = player;
        spellNumber = sn;
    }
    public void StateUpdate()
    {
        pi.playerScript.CastSpell(spellNumber);
    }
    public void HandleInput()
    {
        pi.currentState = new PlayerStateIdle(pi);
    }
    public Color GetColor()
    {
        return Color.green;
    }
}

public class PlayerStateJumping : IPlayerState
{
    float jumpTimer = 0.0f;
    float jumpTimeMax = 1.0f;
    bool walking = false;
    PlayerInputScript pi;
    public PlayerStateJumping(PlayerInputScript player, bool w)
    {
        pi = player;
        walking = w;
    }
    public void StateUpdate()
    {
        jumpTimer += 0.1f;
        pi.playerScript.Jump(walking);
    }
    public void HandleInput()
    {
        if (jumpTimer >= jumpTimeMax)
        {
            pi.currentState = new PlayerStateIdle(pi);
        }
    }
    public Color GetColor()
    {
        return Color.black;
    }
}


public class PlayerStateDeath : IPlayerState
{
    PlayerInputScript pi;
    public PlayerStateDeath(PlayerInputScript player)
    {
        pi = player;
    }
    public void StateUpdate()
    {

    }
    public void HandleInput()
    {
    }
    public Color GetColor()
    {
        return Color.magenta;
    }
}