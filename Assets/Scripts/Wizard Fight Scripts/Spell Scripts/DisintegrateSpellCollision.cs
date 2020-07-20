using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Disintegrate spell object behavior when it collides with anything
public class DisintegrateSpellCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;
        //handle behavior for interacting with a disintegrate spell
        if (go.tag == "WizardCharacter")
        {
            //Deal damage to the character
            float damage = GetComponent<DisintegrateSpell>().GetDamage();
            go.GetComponent<PlayerScript>().adjustHealth(damage);
            //if the player is dead, switch all materials to plasma2
            if (go.GetComponent<PlayerScript>().GetPlayerHealth()<=0)
            {
                Transform t = go.GetComponent<PlayerScript>().GetPlayerTransform();
                SkinnedMeshRenderer smr = t.Find("Body").GetComponent<SkinnedMeshRenderer>();
                //get the materials
                Material[] ms = smr.materials;
                Material[] ms_copy = new Material[ms.Length];
                //Change all materials from the meshrenderer to plasma
                for (int i = 0; i < ms.Length; i++) {
                    ms_copy[i] = Resources.Load<Material>("Materials/SpellEffects/Plasma(2)");
                }
                //switch the array on the meshrenderer to a copy
                smr.materials = ms_copy;
            }
            Destroy(gameObject);
        }

        //If the collided game object is interactable, destroy it like it's melting
        else if (go.tag == "Interactable")
        {
            //Disable collider and renderer
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<SphereCollider>().enabled = false;
            Transform goTransform = go.GetComponent<Transform>();
            GameObject mesh = goTransform.Find("default").gameObject;
            MeshRenderer mr = mesh.GetComponent<MeshRenderer>();
            Material[] m = mr.materials;
            m[0] = Resources.Load<Material>("Materials/SpellEffects/Plasma(2)");
            mr.materials = m;
            StartCoroutine(WaitToDestroy(1f, go));
        }

        else
        {
            Destroy(gameObject);
        }

    }

    IEnumerator WaitToDestroy(float time, GameObject go)
    {
        yield return new WaitForSeconds(time);
        Destroy(go);
        Destroy(gameObject);
    }
}
