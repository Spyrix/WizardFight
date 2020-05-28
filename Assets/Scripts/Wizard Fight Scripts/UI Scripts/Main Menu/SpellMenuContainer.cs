using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
