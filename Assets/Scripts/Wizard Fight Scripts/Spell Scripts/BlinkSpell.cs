using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Start is called before the first frame update


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
