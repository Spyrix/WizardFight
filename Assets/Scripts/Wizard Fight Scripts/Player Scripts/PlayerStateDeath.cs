using System;
using UnityEngine;
//This is essentiall a nullstate, where the player is sent to so that they can't take any more actions.
public class PlayerStateDeath : MonoBehaviour, IPlayerState
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