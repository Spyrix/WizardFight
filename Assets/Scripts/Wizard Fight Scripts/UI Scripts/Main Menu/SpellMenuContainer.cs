using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
* This is the class that is attached to each option on the spell selection menu.
* When the spell select cursor hovers over the attached gameobject and a selection is made, 
* it grabs the spellPrefab to carry into the game.
* 
*/
[RequireComponent(typeof(BoxCollider))]
public class SpellMenuContainer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    internal GameObject spellPrefab;

    [SerializeField]
    internal Sprite spellIcon;

    void Awake()
    {
        BoxCollider col = GetComponent<BoxCollider>();
        col.size = new Vector3(160f,30f,400f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetSpellPrefab()
    {
        return spellPrefab;
    }

    public Sprite GetSprite()
    {
        return spellIcon;
    }

}
