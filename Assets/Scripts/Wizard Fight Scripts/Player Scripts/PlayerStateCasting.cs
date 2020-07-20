using System;
using UnityEngine;
//In this state the player casts the chosen spell, spawning a spellobject.
public class PlayerStateCasting : MonoBehaviour, IPlayerState
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
