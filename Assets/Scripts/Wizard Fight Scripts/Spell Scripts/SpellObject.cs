using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellObject : MonoBehaviour
{
    //I'm going to level, this should like... probably 100% be an interface.
    //I'll just like... fix it later hahahaha.

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
}
