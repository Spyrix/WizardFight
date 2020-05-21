using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "weapon")
        {

            Destroy(col.gameObject);
            GameObject.Find("PlayerCharacter").GetComponent<PlayerWeaponController>().Equip("sword");

        }
    }
}
