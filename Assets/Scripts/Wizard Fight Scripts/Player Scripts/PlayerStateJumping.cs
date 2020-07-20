using System;
using UnityEngine;

//This state has the player move up in a jump motion for exactly one second, then puts them back into the idle state to fall.
public class PlayerStateJumping : MonoBehaviour, IPlayerState
{
    Vector2 mi;
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
        jumpTimer += Time.deltaTime;
        mi = pi.GetMovementInput();
        pi.playerScript.Jump(walking, jumpTimer, mi);
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
