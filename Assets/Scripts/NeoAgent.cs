using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.SceneManagement;

public class NeoAgent : Agent
{

    // public Animator anim;
    private Rigidbody2D neo;
    public bool facingRight = true;
    public GameObject sword;
    private Vector3 neoStartPos;
    private RandomPlatform randomPlatform;
    private int deadMinionNum;
    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true;
        // anim = GetComponent<Animator>();
        neo = GetComponent<Rigidbody2D>();
        sword = GameObject.Find("sword");
        neoStartPos = transform.position;
        randomPlatform = GameObject.Find("Platforms").GetComponent<RandomPlatform>();
        // minions = randomPlatform.agents;
        // minionNames = randomPlatform.leaderMinions;
    }

    public void Restart()
    {    
        deadMinionNum = 0;
        transform.position = neoStartPos;
        for(int i = 0; i < randomPlatform.agents.Count; i++)
        {
            Destroy(randomPlatform.agents[i]);
        }
        randomPlatform.agents.Clear();
        randomPlatform.leaderMinions.Clear();
        
    }
    public override void OnEpisodeBegin(){
        Debug.Log("episode began");
        Restart();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(0.0f);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        //Debug.Log("Action taken was: " + actionBuffers.DiscreteActions[0]);
        int move_action = actionBuffers.DiscreteActions[0];
        int jump_action = actionBuffers.DiscreteActions[1];
        float moveHorizontal = 0.0f;
        switch(move_action){
            case 0:
                sword.GetComponent<sword>().Attack();
                break;
            case 1:
                moveHorizontal = -1.0f;
                break;
            case 2:
                moveHorizontal = 1.0f;
                break;
        }

        switch(jump_action){
            case 0:
                sword.GetComponent<sword>().Attack();
                break;
            case 1:
                moveHorizontal = -1.0f;
                break;
            
        }



        neo.velocity = new Vector2(moveHorizontal * 20f, neo.velocity.y);
        
        if (moveHorizontal != 0)
        {
            if(moveHorizontal>0){
                facingRight = true;
            }else{
                facingRight = false;
            }
            transform.localScale = new Vector3(moveHorizontal, 1, 1);
        }
        // // Actions, size = 2
        // Vector3 controlSignal = Vector3.zero;
        // controlSignal.x = actionBuffers.ContinuousActions[0];
        // controlSignal.z = actionBuffers.ContinuousActions[1];
        // rBody.AddForce(controlSignal * forceMultiplier);

        // // Rewards
        // float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        // // Reached target
        // if (distanceToTarget < 1.42f)
        // {
        //     SetReward(1.0f);
        //     EndEpisode();
        // }

        // // Fell off platform
        // else if (this.transform.localPosition.y < 0)
        // {
        //     EndEpisode();
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void handleSwordAttack(){
        Debug.Log("hit!");
        AddReward(3.0f);
        deadMinionNum ++;
        if (deadMinionNum == 10){
            Debug.Log("slayed ten minions!");
            EndEpisode();
        }
    }

    public void handleSwordNotAttack(){
        Debug.Log("doesnt hit anything");
        AddReward(-1.0f);
    }

}
