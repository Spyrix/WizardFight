﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionScript : MonoBehaviour
{
    [SerializeField]
    internal PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GetComponent<PlayerScript>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void OnCollisionEnter(Collision collision)
    {

    }

}
