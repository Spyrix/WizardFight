using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This class dictates what happens when a ice spell collides with a player
public class IceSpellCollision : MonoBehaviour
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
            //Disable spell collider and renderer
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            //Freeze the wizard character
            //disable animator
            go.GetComponent<Animator>().enabled = false;
            //disable inputscript
            go.GetComponent<PlayerInputScript>().enabled = false;

            Transform t = go.GetComponent<PlayerScript>().GetPlayerTransform();
            SkinnedMeshRenderer smr = t.Find("Body").GetComponent<SkinnedMeshRenderer>();
            //get the materials
            Material[] ms = smr.materials;
            Material[] ms_copy = new Material[ms.Length];
            //Change all materials from the meshrenderer to snow
            for (int i = 0; i < ms.Length; i++)
            {
                ms_copy[i] = Resources.Load<Material>("Materials/SpellEffects/Snow");
            }
            //switch the array on the meshrenderer to a copy
            smr.materials = ms_copy;
            //##

            //after a second, revert changes
            StartCoroutine(WaitToRevertWizardCharacter(1f,go,smr));
        }
        else
        {
            //we've hit an object we can't interact with, so destroy the spell
            Destroy(gameObject);
        }
    }

    IEnumerator WaitToRevertWizardCharacter(float time, GameObject go, SkinnedMeshRenderer smr)
    {
        //Wait for time seconds to revert frozen status
        yield return new WaitForSeconds(time);
        //Make it so that the player can move again
        go.GetComponent<Animator>().enabled = true;
        go.GetComponent<PlayerInputScript>().enabled = true;
        //put all the player's materials back to normal
        Material[] m = new Material[7];
        m[0] = Resources.Load<Material>("Materials/WizardCharacter/Clothes");
        m[1] = Resources.Load<Material>("Materials/WizardCharacter/Skin");
        m[2] = Resources.Load<Material>("Materials/WizardCharacter/Belt");
        m[3] = Resources.Load<Material>("Materials/WizardCharacter/Gold");
        m[4] = Resources.Load<Material>("Materials/WizardCharacter/Hat");
        m[5] = Resources.Load<Material>("Materials/WizardCharacter/Hair");
        m[6] = Resources.Load<Material>("Materials/WizardCharacter/Face");
        smr.materials = m;
        Destroy(gameObject);
    }
}
