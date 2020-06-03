using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    internal Animator animator;
    [SerializeField]
    internal PlayerScript playerScript;

    List<string> spellAnimationBools;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerScript = GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void EnterWalkingState(float magnitude)
    {
        animator.SetBool("WalkingState", true);
        animator.speed = magnitude;
    }

    internal void LeaveWalkingState()
    {
        animator.speed = 1f;
        animator.SetBool("WalkingState", false);
    }

    internal void EnterSpellAimingState()
    {
        animator.speed = 4f;
        animator.SetBool("PointAimState", true);
      
    }

    internal void LeaveSpellAimingState()
    {
        animator.speed = 1f;
        animator.SetBool("PointAimState", false);
    }

    internal void PlayJump()
    {
        //to be implemented
        animator.SetBool("JumpState", true);
    }

    internal void LeaveJumpState()
    {
        //to be implemented
        animator.SetBool("JumpState", false);
    }

    internal void PlayDeath()
    {
        animator.SetBool("DeathState", true);
    }
}
