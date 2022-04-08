using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : MonoBehaviour
{
    public int healthPoint;
    public int previousHealthPoint;
    public GameObject ant;
    public MinionState minionState;
    private float speed = 20.0f;
    // Start is called before the first frame update
    void Start()
    {
        ant = GameObject.Find("Minion");
        minionState = ant.GetComponent<MinionState>();
        healthPoint = 200;
        previousHealthPoint = 200;
    }

    // Update is called once per frame
    void Update()
    {
        //Decide the boss die.
        if(healthPoint<=0){
            Destroy(gameObject);
        }
        if(previousHealthPoint>healthPoint){
            previousHealthPoint = healthPoint;
            minionState.BossBeenAttacked();
        }
    }

    public void TakeDamage(int damage){
        healthPoint -= damage;
        Debug.Log(healthPoint);
    }
}
