using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public int coins;
    // Start is called before the first frame update
    void Start()
    {
        coins = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "coin") {
            Destroy(col.gameObject);
            coins++;
            GameObject.Find("CoinAmount").GetComponent<UnityEngine.UI.Text>().text = "Coins:\n"+coins;
        }
    }
}
