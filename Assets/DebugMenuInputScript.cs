using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugMenuInputScript : MonoBehaviour
{
    [SerializeField]
    internal PlayerInputActions inputAction;
    Vector2 navigationInput;
    // Start is called before the first frame update
    void Awake()
    {
        inputAction = new PlayerInputActions();
        inputAction.PlayerControls.Walk.performed += ctx => navigationInput = ctx.ReadValue<Vector2>();
        //Setup input for horizontal movement release
        inputAction.PlayerControls.Walk.canceled += ctx => navigationInput = ctx.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = ""+navigationInput;
    }

    private void OnEnable()
    {
        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.Disable();
    }

}
