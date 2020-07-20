using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This is basically a base class for the other spell game objects that the player creates when they cast a spell. 

public class SpellObject : MonoBehaviour
{


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Cast()
    {
    }

    public virtual void Cast(Transform T)
    {
    }

    public virtual float GetCooldownTime()
    {
        return 0f;
    }

    public virtual GameObject GetAimPrefab()
    {
        return null;
    }

    public virtual void SetAimGameObject(GameObject go)
    {

    }

    public virtual void SetSpellOwner(GameObject go)
    {

    }

    public virtual GameObject GetSpellOwner()
    {
        return null;
    }

    public virtual Sprite GetSpellIcon()
    {
        return null;
    }
}
