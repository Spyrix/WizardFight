using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpSpeed;
    public float climbSpeed;
    public float runSpeed;
    public float turnSpeed;
    public bool canJump;
    public bool isClimbing;
    Rigidbody rb;
    Transform tf;
    GameObject climbObject;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tf = GetComponent<Transform>();
        canJump = true;
        isClimbing = false;
        jumpSpeed = 8f;
        runSpeed = 8f;
        turnSpeed = 500f;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // What to do when the player collides with something
    private void OnCollisionEnter(Collision col)
    {
        /* ex code
        if (col.gameObject.tag == ("Ground") && isGrounded == false)
        {
            isGrounded = true;
        }
         */
        print("Normal of contact point is:" + col.contacts[0].normal);
        canJump = true;
        if (col.gameObject.tag == ("Climbable"))
        {
            print("Can climb!");
            climbObject = col.gameObject;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Jump();
        GroundMovement();
        //Climb();
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && canJump)
        {
            rb.velocity = new Vector3(0, jumpSpeed,0);
            canJump = false;
        }
    }

    void GroundMovement()
    {
        
        float forwardMovement = Input.GetAxis("Vertical") * runSpeed * Time.deltaTime;
        float sidewaysMovement = Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime;

        //move player
        transform.Translate(Vector3.forward * forwardMovement);
        transform.Rotate(Vector3.up * sidewaysMovement);
    }

    /*void Climb()
    {
        if (Input.GetButtonDown("Climb"))
        {
            isClimbing = true;
            rb.useGravity = false;
            //if ()
            //{

            //}
        }
        if (Input.GetButtonUp("Climb"))
        {
            isClimbing = false;
            climbObject = null;
            rb.useGravity = true;
        }
    }*/
}
