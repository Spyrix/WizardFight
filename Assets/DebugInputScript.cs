using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugInputScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    internal GameObject player;
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = ""+player.GetComponent<PlayerInputScript>().GetMovementInput();
    }
}
