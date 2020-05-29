using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpellcasting : MonoBehaviour
{
    //Spell objects are game objects that do the spells. 
    [SerializeField]
    internal List<GameObject> spellSlots;
    List<GameObject> abilityCooldownControllers;
    GameObject[] abilityAimGameObjects;
    int[] spellNumMapping;
    float[] spellSlotCooldownTimers;//One for each mapped spell

    //maybe how this should be is... I have a list of all of the game object spells possible
    //and only 4 can be flipped on to a character at the same time
    [SerializeField]
    internal PlayerScript playerScript;

    // Start is called before the first frame update
    void Awake()
    {
        spellSlotCooldownTimers = new float[3];
        if(spellSlots.Count == 0)//For debug purposes, sometimes it may have spells attached already
            spellSlots = new List<GameObject>();
        abilityCooldownControllers = new List<GameObject>();
        abilityAimGameObjects = new GameObject[3];
        playerScript = GetComponent<PlayerScript>();

        //For debug
        //spellSlots.Add(Resources.Load<GameObject>("Prefabs/IceSpellPrefab"));
        //create the UI element for each spellslot
        for (int i = 0; i < spellSlots.Count; i++)
        {
            //Instantiate graphic and attach it to the player status bar, above each player's head
            GameObject bar = transform.Find("PlayerStatusBar").gameObject;
            GameObject ac = Instantiate(Resources.Load<GameObject>("Prefabs/AbilityCooldownPrefab"),bar.transform);
            GameObject cooldownImage = ac.transform.Find("Center").gameObject;
            cooldownImage.GetComponent<Image>().sprite = spellSlots[i].GetComponent<CastController>().GetSpellObject().GetSpellIcon();
            ac.transform.SetParent(bar.transform);

            //Giving the ability cooldown graphic different text//features depending on their spell slot/button
            switch (i)
            {
                case 0:
                    ac.transform.Find("Text").gameObject.GetComponent<Text>().text = "X";
                    break;
                case 1:
                    ac.transform.Find("Text").gameObject.GetComponent<Text>().text = "Y";
                    break;
                case 2:
                    ac.transform.Find("Text").gameObject.GetComponent<Text>().text = "B";
                    break;
                default:
                    ac.transform.Find("Text").gameObject.GetComponent<Text>().text = "e";
                    break;
            }

            //Set scale and position of graphic
            RectTransform rt = ac.GetComponent<RectTransform>();
            rt.localScale = new Vector3(.40f, .40f, .40f);
            //moove the cooldown graphic in positive y axis direction by the graphic height (scaled)
            //move it in the negative x axis direction by half the width of the canvas (plus the graphic width scaled)
            rt.localPosition = new Vector3((rt.localScale.x*rt.rect.width*i)-(bar.GetComponent<RectTransform>().rect.width/2), rt.localScale.y * rt.rect.height,0);
            abilityCooldownControllers.Add(ac);
        }
        //default timer to 0
        for (int i = 0; i < spellSlotCooldownTimers.Length; i++)
        {
            spellSlotCooldownTimers[i] = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleSpellCooldowns();
    }

    //Three different ways to cast a spell
    //instant cast
    //Aim and charge as projectile

    internal void Cast(int spellNum, Transform playerTransform)
    {

        if (spellSlotCooldownTimers[spellNum] == 0.0f) {
            spellSlots[spellNum].GetComponent<CastController>().Cast(abilityAimGameObjects[spellNum].transform);
            spellSlotCooldownTimers[spellNum] += 0.1f;
        }
        //we may want the location of the aiming prefab for it's position, so only end aiming after casting has ended
        EndAiming();
    }

    internal void HandleSpellCooldowns()
    {
        for (int i = 0; i < spellSlotCooldownTimers.Length; i++)
        {
            if (spellSlotCooldownTimers[i] > 0.0f)
            {
                spellSlotCooldownTimers[i] += Time.deltaTime;
                abilityCooldownControllers[i].transform.Find("ProgressBar").GetComponent<Image>().fillAmount = spellSlotCooldownTimers[i] / spellSlots[i].GetComponent<CastController>().GetCooldownTime();
                if (spellSlotCooldownTimers[i] >= spellSlots[i].GetComponent<CastController>().GetCooldownTime())
                {
                    spellSlotCooldownTimers[i] = 0.0f;
                }
            }
        }
    }

    public void StartAiming(int spellNum)
    {
        //Debug.Log(spellSlots[spellNum]);
        GameObject aimPrefab = spellSlots[spellNum].GetComponent<CastController>().GetAimPrefab();
        //if the aim prefab is not null, instantiate it
        if (aimPrefab) {
            GameObject go = Instantiate(aimPrefab);
            go.GetComponent<IAimPrefab>().SetPlayer(gameObject);
            abilityAimGameObjects[spellNum] = go;
        }
        else if(aimPrefab == null)
        {
            Debug.Log("no prefab");
        }
    }

    public void EndAiming()
    {
        //Look for all possible aimprefabs and destroy them
        foreach (GameObject a in abilityAimGameObjects)
        {
            Destroy(a);
        }
    }

    public float[] GetSpellCooldowns()
    {
        return spellSlotCooldownTimers;
    }

    public void AddSpellToSlot(GameObject g)
    {
        //add the prefab to the spellslot
        spellSlots.Add(g);
        int i = spellSlots.Count - 1;
        //handle the UI elements
        //Instantiate graphic and attach it to the player status bar, above each player's head
        GameObject bar = transform.Find("PlayerStatusBar").gameObject;
        GameObject ac = Instantiate(Resources.Load<GameObject>("Prefabs/AbilityCooldownPrefab"), bar.transform);
        GameObject cooldownImage = ac.transform.Find("Center").gameObject;
        cooldownImage.GetComponent<Image>().sprite = g.GetComponent<CastController>().GetSpellObject().GetSpellIcon();
        ac.transform.SetParent(bar.transform);

        //Giving the ability cooldown graphic different text//features depending on their spell slot/button
        switch (i)
        {
            case 0:
                ac.transform.Find("Text").gameObject.GetComponent<Text>().text = "X";
                break;
            case 1:
                ac.transform.Find("Text").gameObject.GetComponent<Text>().text = "Y";
                break;
            case 2:
                ac.transform.Find("Text").gameObject.GetComponent<Text>().text = "B";
                break;
            default:
                ac.transform.Find("Text").gameObject.GetComponent<Text>().text = "e";
                break;
        }

        //Set scale and position of graphic
        RectTransform rt = ac.GetComponent<RectTransform>();
        Debug.Log(rt.anchoredPosition);
        rt.localScale = new Vector3(.40f, .40f, .40f);
        //moove the cooldown graphic in positive y axis direction by the graphic height (scaled)
        //move it in the negative x axis direction by half the width of the canvas (plus the graphic width scaled)
        //rt.anchoredPosition = new Vector2((rt.localScale.x * rt.rect.width * i) - (bar.GetComponent<RectTransform>().rect.width / 2), rt.localScale.y * rt.rect.height);
        rt.localPosition = new Vector3((rt.localScale.x * rt.rect.width * i) - (bar.GetComponent<RectTransform>().rect.width / 2), rt.localScale.y * rt.rect.height,0);
        abilityCooldownControllers.Add(ac);
        //handle cooldown stuff
        spellSlotCooldownTimers[i] = 0f;
    }

    public List<GameObject> GetSpellSlots()
    {
        return spellSlots;
    }
}