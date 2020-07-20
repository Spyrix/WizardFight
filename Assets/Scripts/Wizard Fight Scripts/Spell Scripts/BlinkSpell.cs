using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This spell is a little different from the other spell objects, because it merely defines where the player is teleporting to when the spell is cast

public class BlinkSpell : SpellObject
{
    internal float damage = 0f;
 
    bool move = false;
    [SerializeField]
    internal float spellCooldownTime = 8f;
    [SerializeField]
    internal GameObject aimPrefab;
    [SerializeField]
    internal Sprite spellIcon;


    public override void Cast(Transform t)
    {
        GameObject p = t.gameObject.GetComponent<IAimPrefab>().GetPlayer();
        p.transform.position = t.position;
    }

    public override float GetCooldownTime()
    {
        return spellCooldownTime;
    }

    public override GameObject GetAimPrefab()
    {
        return aimPrefab;
    }

    public float GetDamage()
    {
        return damage;
    }

    public override Sprite GetSpellIcon()
    {
        return spellIcon;
    }
}
