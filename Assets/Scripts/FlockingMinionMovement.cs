using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingMinionMovement : MonoBehaviour
{
    public GameObject ant;
    private FlockingMinionState flockingMinionState;
    private Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ant = GameObject.Find("FlockingMinion");
        flockingMinionState = ant.GetComponent<FlockingMinionState>();
        if(flockingMinionState.currentState == FlockingState.Die){
            Destroy(gameObject);
        }else if(flockingMinionState.currentState == FlockingState.Patrol){
            //Flocking
        }
    }
}
