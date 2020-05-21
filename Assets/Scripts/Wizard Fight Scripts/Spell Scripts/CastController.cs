using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpellObject))]
public class CastController : MonoBehaviour
{
    /*
     * This class basically holds the spellobject class for the prefab, so that you can just invoke a cast 
     * function and not have to worry about getting the specific spellobject component to cast
     * */

    /*When attaching this script to a prefab (so that the playerspellcasting script can cast spells), 
     * make sure to assign the prefab's spellobject component to this variable.*/
    [SerializeField]
    internal SpellObject so;
    void Start()
    {
        so = GetComponent<SpellObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Cast(Transform t)
    {
        so.Cast(t);
    }

    public void Cast()
    {
        so.Cast();
    }

    public float GetCooldownTime()
    {
        return so.GetCooldownTime();
    }
    public GameObject GetAimPrefab()
    {
        return so.GetAimPrefab();
    }
    public void SetAimGameObject(GameObject go)
    {
        so.SetAimGameObject(go);
    }
}
