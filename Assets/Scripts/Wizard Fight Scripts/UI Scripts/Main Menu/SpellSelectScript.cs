using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/*
 * This script is attached to a cursor that a player uses to select spells on the spell selection menu.
 * When the player hovers the spell select cursor gameobject over the spell menu option, they are able to select the spell.
 */


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class SpellSelectScript : MonoBehaviour
{

    Mesh mesh;
    Vector3[] vertices;
    PlayerInputActions inputAction;
    int[] triangles;
    [SerializeField]
    internal int playerNumber;
    [SerializeField]
    internal GameObject[] selections;
    [SerializeField]
    internal GameObject[] selectionIcons;
    Vector2 movementInput;
    [SerializeField]
    internal float[] selectInputArray;
    [SerializeField]
    internal GameObject playerText;

    float cursorSpeed;
    InputUser _user;

    // Start is called before the first frame update
    void Awake()
    {
        cursorSpeed = 150f;
        //Should not hardcode this, temporary
        transform.position = new Vector3(15,40,82);
        mesh = GetComponent<MeshFilter>().mesh;
        CreateShape();
        UpdateMesh();
        selections = new GameObject[3];
        selectInputArray = new float[3];
        //Input for controlling the arrow
        inputAction = new PlayerInputActions();

        //Here we handle multiple users
        _user = new InputUser();
        if (playerNumber >= Gamepad.all.Count)
        {
            //This means that somehow, there are more players in the game then gamepads
            //Remember to throw an exception
            playerNumber = 0;
        }
        _user = InputUser.PerformPairingWithDevice(Gamepad.all[playerNumber]);
        _user.AssociateActionsWithUser(inputAction);
        //Setup input for Navigate movement value press AND release
        //inputAction.MenuControls.Navigate.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        //inputAction.MenuControls.Navigate.canceled += ctx => movementInput = ctx.ReadValue<Vector2>();
        inputAction.PlayerControls.Walk.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        inputAction.PlayerControls.Walk.canceled += ctx => movementInput = ctx.ReadValue<Vector2>();
        //Setup input for select input values.
        inputAction.PlayerControls.Spell0.performed += ctx => selectInputArray[0] = ctx.ReadValue<float>();
        inputAction.PlayerControls.Spell1.performed += ctx => selectInputArray[1] = ctx.ReadValue<float>();
        inputAction.PlayerControls.Spell2.performed += ctx => selectInputArray[2] = ctx.ReadValue<float>();
        inputAction.PlayerControls.Spell0.canceled += ctx => selectInputArray[0] = ctx.ReadValue<float>();
        inputAction.PlayerControls.Spell1.canceled += ctx => selectInputArray[1] = ctx.ReadValue<float>();
        inputAction.PlayerControls.Spell2.canceled += ctx => selectInputArray[2] = ctx.ReadValue<float>();

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    //OnEnable and OnDisable are required for the inputAction class to work
    private void OnEnable()
    {
        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.Disable();
    }

    void CreateShape()
    {
        vertices = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(1,0,0),
            new Vector3(-1,1,0),
            new Vector3(-1,-1,0),
        };

        triangles = new int[]
        {
            0,1,3,
            2,1,0
        };
    }

    void UpdateMesh()
    {
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    void Movement()
    {
        //if movement input is pressed, move the attached game object in the appropriate direction.
        //Ensure that the game object does not go off screen.
        float x = 0;
        float y = 0;
        if (movementInput.y != 0)
        {
            y = movementInput.y * Time.deltaTime * cursorSpeed;
        }
        if (movementInput.x != 0)
        {
            x = movementInput.x * Time.deltaTime * cursorSpeed;
        }
        transform.position += new Vector3(x,y,0);
    }

    private void OnTriggerStay(Collider collision)
    {
        //If the cursor is hovering over the menu spell item, and the player is pressing select
        //add the spell prefab to the list of prefabs
        if (collision.gameObject.tag == "IconSpell")
        {
            for (int i = 0; i < selectInputArray.Length; i++)
            {
                //If a player is pressing a spell button to make a selection, add it to their selections
                if (selectInputArray[i] > 0){
                    selections[i] = collision.gameObject.GetComponent<SpellMenuContainer>().GetSpellPrefab();
                    //enable the spell icon on the cursor and change the graphic
                    selectionIcons[i].SetActive(true);
                    selectionIcons[i].GetComponent<SpriteRenderer>().sprite = collision.gameObject.GetComponent<SpellMenuContainer>().GetSprite();
                }
            }
        }
    }

    public void SetPlayerNumber(int i)
    {
        playerNumber = i;
        _user = new InputUser();
        if (playerNumber >= Gamepad.all.Count)
        {
            //This means that somehow, there are more players in the game then gamepads
            //Remember to throw an exception
            playerNumber = 0;
        }
        _user = InputUser.PerformPairingWithDevice(Gamepad.all[playerNumber]);
        _user.AssociateActionsWithUser(inputAction);
        playerText.GetComponent<Text>().text = "Player " + i;
    }

    public GameObject[] GetSpellSelection()
    {
        return selections;
    }
}
