using System;
using UnityEngine;

/*
 * The state that the player enters when they're aiming a spell.
 * They can choose to cancel the spell and enter idle.
 * Or, if they release the button that they pressed that took them into this state, then cast the spell.
 * This class contacts the PlayerScript for the game logic behavior.
 */
public class PlayerStateAiming : MonoBehaviour, IPlayerState
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
        //pass in the movement input vector so we know what direction to aim in
        pi.playerScript.SpellAim(mi.x, mi.y);
        spellInputTable = pi.GetSpellInputTable();
        ci = pi.GetCancelInput();
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