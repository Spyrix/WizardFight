using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricSpellCollision : MonoBehaviour
{
    internal void OnTriggerEnter(Collider collider)
    {
        GameObject go = collider.gameObject;

        //handle behavior for interacting with electric spell
        if (go.tag == "WizardCharacter")
        {
            //Deal damage to the character
            DamagePlayer(go);
        }
    }

    internal void OnTriggerStay(Collider collider)
    {
        GameObject go = collider.gameObject;

        //handle behavior for interacting with electric spell
        if (go.tag == "WizardCharacter")
        {
            DamagePlayer(go);
        }
    }

    void DamagePlayer(GameObject go)
    {
        //Deal damage to the character
        float damage = GetComponent<ElectricSpell>().GetDamage();
        if (go != GetComponent<SpellObject>().GetSpellOwner()) {
            Debug.Log("go1:"+go);
            Debug.Log("go2:"+GetComponent<SpellObject>().GetSpellOwner());
            go.GetComponent<PlayerScript>().adjustHealth(damage);
        }
        //if the player is dead, switch all materials to plasma2
        if (go.GetComponent<PlayerScript>().GetPlayerHealth() <= 0)
        {
            Transform t = go.GetComponent<PlayerScript>().GetPlayerTransform();
            SkinnedMeshRenderer smr = t.Find("Body").GetComponent<SkinnedMeshRenderer>();
            //get the materials
            Material[] ms = smr.materials;
            Material[] ms_copy = new Material[ms.Length];
            //Change all materials from the meshrenderer to plasma
            for (int i = 0; i < ms.Length; i++)
            {
                ms_copy[i] = Resources.Load<Material>("Materials/SpellEffects/Plasma(2)");
            }
            //switch the array on the meshrenderer to a copy
            smr.materials = ms_copy;
        }
    }
}
