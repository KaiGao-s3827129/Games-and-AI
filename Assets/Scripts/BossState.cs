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
        ant = GameObject.Find("Neo");
        minionState = ant.GetComponent<MinionState>();
        healthPoint = 5;
        previousHealthPoint = 5;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newLocation = new Vector2(ant.transform.position.x-30,ant.transform.position.y+30);
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, newLocation, step);
        if(previousHealthPoint>healthPoint){
            previousHealthPoint = healthPoint;
            minionState.BossBeenAttacked();
        }
    }
}
