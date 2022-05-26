using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossState : MonoBehaviour
{
    public int healthPoint;
    public int previousHealthPoint = 500;
    public List<GameObject> ant;
    public GameObject platforms;
    public RandomPlatform randomPlatform;
    public HealthBar healthBar;
    public GameObject winMenu;
    public GameObject Neo;


    // Start is called before the first frame update
    void Start()
    {   
        //Get the all Leader Minions.
        ant = new List<GameObject>();
        platforms = GameObject.Find("Platforms");
        randomPlatform = platforms.GetComponent<RandomPlatform>();
        Neo = GameObject.Find("Neo");
    }

    // Update is called once per frame
    void Update()
    {
        Neo.GetComponent<NeoAgent>().nearByBoss(gameObject.transform.position-Neo.transform.position);
        //Decide the boss die.
        // if (healthPoint <= 0)
        // {
        //     Destroy(gameObject);
        //     // winMenu.SetActive(true);
        //     Time.timeScale = 0f;
        // }
        ant = new List<GameObject>();
        foreach (string one in randomPlatform.leaderMinions)
        {
            ant.Add(GameObject.Find(one));
        }

    }

    public void TakeDamage(int damage)
    {
        //Take the damage from Neo.
        healthPoint -= damage;
        //Set the health bar.
        healthBar.SetHealth(healthPoint);
        foreach (GameObject one in ant)
        {
            //Let Minion know the Boss has been attacked
            one.GetComponent<MinionState>().BossBeenAttacked();
        }
    }

}
