using System;
using UnityEngine;
/*
 * The player starts in idle, and does not move.
 * They can move into walking, jumping, and aiming a spell.
 */
public class PlayerStateIdle : MonoBehaviour, IPlayerState
{
    PlayerInputScript pi;
    Vector2 mi;
    Vector2 ri;
    float ji;
    float cancelSpell;
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
        cancelSpell = pi.GetCancelInput();
    }
    public void HandleInput()
    {
        if (mi.magnitude > 0f)
            pi.currentState = new PlayerStateWalking(pi);
        if (ji == 1f && pi.IsGrounded())
            pi.currentState = new PlayerStateJumping(pi, false);
        //check to see if a spell input was pressed
        for (int i = 0; i < spellInputTable.Length; i++)
        {
            if (spellInputTable[i] == 1f && cancelSpell == 0f)
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