using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : MonoBehaviour
{
    public int healthPoint;
    public int previousHealthPoint = 200;
    public List<GameObject> ant;
    public MinionState minionState;
    private float speed = 20.0f;
    public GameObject platforms;
    public RandomPlatform randomPlatform;

    // Start is called before the first frame update
    void Start()
    {
        ant = new List<GameObject>();
        platforms = GameObject.Find("Platforms");
        randomPlatform  = platforms.GetComponent<RandomPlatform>();

        // healthPoint = 200;
    }

    // Update is called once per frame
    void Update()
    {
        
        
        //Decide the boss die.
        if(healthPoint<=0){
            Destroy(gameObject);
        }
        ant = new List<GameObject>();
        foreach(string one in randomPlatform.leaderMinions){
            ant.Add(GameObject.Find(one));
        }
        
    }

    public void TakeDamage(int damage){
        healthPoint -= damage;
        foreach(GameObject one in ant){
            one.GetComponent<MinionState>().BossBeenAttacked();
        }
    }

}
