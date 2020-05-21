using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricSpell : SpellObject
{
    internal float damage = -50f;
    internal float duration = 4f;
    internal float timer = 0f;
    bool move = false;
    [SerializeField]
    internal float spellCooldownTime = 6f;
    [SerializeField]
    internal GameObject aimPrefab;
    [SerializeField]
    internal GameObject spellOwner;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }


    public override void Cast(Transform t)
    {
        GameObject spell = Instantiate(gameObject, t.position, Quaternion.identity);
        spell.GetComponent<ElectricSpell>().SetSpellOwner(t.gameObject.GetComponent<IAimPrefab>().GetPlayer());
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
    public override void SetSpellOwner(GameObject go)
    {
        spellOwner = go;
    }
    public override GameObject GetSpellOwner()
    {
        return spellOwner;
    }
}
