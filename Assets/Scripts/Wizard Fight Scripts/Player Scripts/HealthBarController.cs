using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    internal Transform mainCameraTransform;
    Transform t;
    GameObject slider;
    void Start()
    {
        if (mainCameraTransform == null) {
            mainCameraTransform = GameObject.Find("PlayerCamera").GetComponent<Transform>();
        }
        t = GetComponent<Transform>();
        slider = t.Find("Slider").gameObject;
        slider.GetComponent<Slider>().value = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        //Always face the camera
        t.rotation = Quaternion.Euler(new Vector3(45, 0, 0));
    }

    public void UpdateHealth(float h, float mh)
    {
        slider.GetComponent<Slider>().value = (h/mh)*(slider.GetComponent<Slider>().maxValue);
    }
}
