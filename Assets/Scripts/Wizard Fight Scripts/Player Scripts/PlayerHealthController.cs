using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField]
    internal float playerHealth;
    [SerializeField]
    internal float playerMaxHealth = 100f;
    [SerializeField]
    internal PlayerScript playerScript;
    [SerializeField]
    internal bool invulnerableOn = false;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = GetComponent<PlayerScript>();
        //Players start with 100 health
        playerHealth = playerMaxHealth;
        //players start with no status affects
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void addHealth(float healthAmount)
    {
        //Only modify health if invulnerability is off or if the health is a positive value
        if (invulnerableOn != true || healthAmount >= 0) {
            playerHealth += healthAmount;
            //Don't go over 100 health
            if (playerHealth > playerMaxHealth)
            {
                playerHealth = playerMaxHealth;
            }
            //update health in healthbar
            GameObject healthBar = transform.Find("PlayerStatusBar").gameObject;
            healthBar.GetComponent<HealthBarController>().UpdateHealth(playerHealth, playerMaxHealth);
        }
        if (playerHealth <= 0)
        {
            //death!!!!
            playerScript.Death();
        }
        //If the player takes damage, give them invulnerability frames
        if (healthAmount < 0)
        {
            SetPlayerInvulnerable(1f);
        }
    }

    public float GetPlayerHealth()
    {
        return playerHealth;
    }

    public void SetPlayerInvulnerable(float seconds)
    {
        //Gives player some invulnerability frames after being hit by certain objects
        invulnerableOn = true;
        StartCoroutine(WaitToRevertInvulnerable(seconds));
    }

    IEnumerator WaitToRevertInvulnerable(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        invulnerableOn = false;
    }
}
