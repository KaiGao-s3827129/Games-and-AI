using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdStatus : MonoBehaviour
{
    private int coinsCollected;
    private UnityEngine.UI.Text scoreDisplay;

    // Use this for initialization
    void Start()
    {
        coinsCollected = 0;
        scoreDisplay = GameObject.Find("Canvas").transform.Find("Text").GetComponent<UnityEngine.UI.Text>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreDisplay.text = "Coins: " + coinsCollected;
    }

    public void CollectCoin(GameObject coin)
    {
        coinsCollected++;
        Destroy(coin);
    }
}
