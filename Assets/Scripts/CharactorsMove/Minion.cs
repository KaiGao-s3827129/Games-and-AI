using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Leader Minion Movement
public class Minion : MonoBehaviour
{
    public float max_velocity;
    public GameObject Neo;
    public Vector2 velocity;
    private Rigidbody2D rb2d;
    public float slowDownRadius;
    public GameObject ant;
    private MinionState minionState;
    public GameObject platforms;
    public RandomPlatform randomPlatform;
    // Start is called before the first frame update
    void Start()
    {
        // rb2d = GetComponent<Rigidbody2D>();
        max_velocity = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameObject.Find("Neo") == null)
        {
            return;
        }
        //According to state script to move
        platforms = GameObject.Find("Platforms");
        randomPlatform  = platforms.GetComponent<RandomPlatform>();
        ant = GameObject.Find(gameObject.name);
        minionState = ant.GetComponent<MinionState>();
        Vector2 steeringForce = new Vector2(0, 0);
        Vector2 toTarget = GameObject.Find("Neo").transform.position - this.transform.position;
        float distance = toTarget.magnitude;
        //Chase state
        // Debug.Log(minionState.currentState);
        if (minionState.currentState == State.Walk)
        {
            ant.GetComponent<Astar>().enabled = true;
            // ant.GetComponent<FlockingBehaviors>().enabled = false;
        }
        else if (minionState.currentState == State.Run)
        {
            ant.GetComponent<Astar>().enabled = true;
            // ant.GetComponent<FlockingBehaviors>().enabled = false;
        }
        else if (minionState.currentState == State.Patrol)
        {
            ant.GetComponent<Astar>().enabled = false;
            // ant.GetComponent<FlockingBehaviors>().enabled = true;



            
        }
        else if (minionState.currentState == State.Die) {
            for (int i=0; i<randomPlatform.leaderMinions.Count; i++){
                if(randomPlatform.leaderMinions[i]==ant.name){
                    randomPlatform.leaderMinions.RemoveAt(i);
                    i--;
                }
            }
            for (int i=0; i<randomPlatform.agents.Count; i++ ){
                if(randomPlatform.agents[i].name==ant.name){
                    randomPlatform.agents.RemoveAt(i);
                    i--;
                }
            }

            Destroy(ant);   
        }
           
    }
}
