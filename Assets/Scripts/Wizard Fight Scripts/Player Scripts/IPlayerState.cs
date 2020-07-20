using System;
using UnityEngine;
/*
 * 
 * State Design Pattern
 * Every state implenents the IPlayerState interface
 * The player input script runs the StateUpdate and HandleInput functions on every engine Update (so every frame).
 * StateUpdate will update variables within each state that control when the states moves to another state, using player input or environmental changes.
 * It also calls the PlayerScript to handle other behavior that the player should be doing in a given state.
 * HandleInput is responsible for looking at the variables that StateUpdate changes and switching state when conditions are met.
 * */
interface IPlayerState
{
    void HandleInput();
    void StateUpdate();
    Color GetColor();
}