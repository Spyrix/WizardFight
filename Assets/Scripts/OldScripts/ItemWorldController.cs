using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorldController : MonoBehaviour
{
    public float hoverSpeedMax = 1f;
    public Vector3 pointOne;
    public Vector3 pointTwo;
    public int direction = 1;
    private float startTime;
    private float journeyLength;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        pointOne = transform.position;
        pointTwo = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
        journeyLength = Vector3.Distance(pointOne, pointTwo);
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
    }

    // Update is called once per frame
    void Update()
    {
        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - startTime) * hoverSpeedMax;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / journeyLength;

        if (direction > 0) {
            transform.position = Vector3.Lerp(pointOne, pointTwo, fractionOfJourney);
        }
        if (direction < 0)
        {
            transform.position = Vector3.Lerp(pointTwo, pointOne, fractionOfJourney);
        }

        if (transform.position == pointTwo)
        {
            direction = -1;
            startTime = Time.time;
        }
        if (transform.position == pointOne)
        {
           direction = 1;
           startTime = Time.time;
        }
        //transform.LookAt(GameObject.Find("PlayerCharacter").transform);
    }
}
