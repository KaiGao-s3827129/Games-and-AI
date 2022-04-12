using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Flocking Minion Movement
public class FlockingMinionMovement : MonoBehaviour
{
    public GameObject ant;
    private FlockingMinionState flockingMinionState;
    private Rigidbody2D rb2d;
    public GameObject platforms;
    public RandomPlatform randomPlatform;
    // Start is called before the first frame update
    void Start()
    {
        
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        platforms = GameObject.Find("Platforms");
        randomPlatform  = platforms.GetComponent<RandomPlatform>();
        ant = GameObject.Find(gameObject.name);
        flockingMinionState = ant.GetComponent<FlockingMinionState>();
        if(flockingMinionState.currentState == FlockingState.Die){
            foreach(string one in randomPlatform.leaderMinions){
                if(one==ant.name){
                    randomPlatform.leaderMinions.Remove(one);
                }
            }
            foreach(GameObject one in randomPlatform.agents){
                if(one.name==ant.name){
                    randomPlatform.agents.Remove(one);
                }
            }
            Destroy(gameObject);
        }else if(flockingMinionState.currentState == FlockingState.Patrol){
            //Flocking here.
        }
    }
}
