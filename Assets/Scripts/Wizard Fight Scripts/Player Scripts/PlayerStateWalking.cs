using System;
using UnityEngine;
/*
 * This state keeps the player moving in the direction dictated by the left analog stick.
 * Can transition into the idle state by letting go of the left analog stick
 * Can transition into aiming by casting a spell
 * Can transition into jumping if the player is grounded and the jump input is pressed
 */
public class PlayerStateWalking : MonoBehaviour, IPlayerState
{

    PlayerInputScript pi;
    Vector2 mi;
    Vector2 ri;
    float[] spellInputTable;
    float ji;
    float cancelSpell;
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
        cancelSpell = pi.GetCancelInput();
    }
    public void HandleInput()
    {
        if (mi.magnitude == 0f)
            pi.currentState = new PlayerStateIdle(pi);
        //check to see if a spell input was pressed
        for (int i = 0; i < spellInputTable.Length; i++)
        {
            if (spellInputTable[i] == 1f && cancelSpell == 0f)
            {
                //Decide if the spell should be cast, or is being aimed
                pi.currentState = new PlayerStateAiming(pi, i);
            }
        }
        if (ji == 1f && pi.IsGrounded())
            pi.currentState = new PlayerStateJumping(pi, true);
    }
    public Color GetColor()
    {
        return Color.blue;
    }
}