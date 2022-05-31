using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.SceneManagement;
using System;
public class NeoAgent : Agent
{

    // public Animator anim;
    private Rigidbody2D neo;
    public bool facingRight = true;
    public GameObject sword;
    private Vector3 neoStartPos;
    private RandomPlatform randomPlatform;
    private int deadMinionNum;
    public GameObject Neo;
    public GameObject Boss;
    public Transform coinLocations;
    public float previousHeight;
    private Vector2 bossLocation1;
    private Vector2 bossLocation2;

    // Start is called before the first frame update
    void Start()
    {
        bossLocation1 = new Vector2(49.6f,28.2f);
        bossLocation2 = new Vector2(-56.6f, 36.2f);
        Application.runInBackground = true;
        // anim = GetComponent<Animator>();
        neo = GetComponent<Rigidbody2D>();
        Neo = GameObject.Find("Neo");
        sword = GameObject.Find("sword");
        Boss = GameObject.Find("TheBoss");
        previousHeight = Neo.transform.position.y;
        neoStartPos = transform.position;
        randomPlatform = GameObject.Find("Platforms").GetComponent<RandomPlatform>();
        Boss.transform.position = bossLocation1;
        // minions = randomPlatform.agents;
        // minionNames = randomPlatform.leaderMinions;
    }

    public void Restart()
    {    
        if(Boss.transform.position.x==bossLocation1.x && Boss.transform.position.y==bossLocation1.y){
            Boss.transform.position = bossLocation2;
        }else{
            Boss.transform.position = bossLocation1;
        }
        deadMinionNum = 0;
        var random = new System.Random();
        var list = randomPlatform.platformList;
        int index = random.Next(list.Count);
        Vector2 newLocation =  new Vector2(list[index].x,list[index].y+4);
        transform.position = newLocation;
        for(int i = 0; i < randomPlatform.agents.Count; i++)
        {
            Destroy(randomPlatform.agents[i]);
        }
        Boss.GetComponent<BossState>().healthPoint=500;
        Boss.GetComponent<BossState>().healthBar.SetHealth(500);
        randomPlatform.agents.Clear();
        randomPlatform.leaderMinions.Clear();
        foreach(Transform coin in coinLocations){
            coin.gameObject.SetActive(true);
        }
        
    }
    public override void OnEpisodeBegin(){
        Debug.Log("episode began");
        Restart();
    }

    public override void CollectObservations(VectorSensor sensor)
    {


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
            // case 3:
            //     Neo.GetComponent<NeoMovement>().Shoot();
            //     break;
        }

        switch(jump_action){
            case 0:
                if(Neo.GetComponent<NeoMovement>().isGround){
                    Neo.GetComponent<NeoMovement>().jumpRequest=true;
                } 
                break;
            case 1:
                
                break;
        }



        neo.velocity = new Vector2(moveHorizontal * 20f, neo.velocity.y);
        
        if (moveHorizontal != 0)
        {
            if(moveHorizontal>0){
                Neo.GetComponent<NeoMovement>().facingRight=true;
            }else{
                Neo.GetComponent<NeoMovement>().facingRight=false;
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
        // SetReward(Neo.transform.position.y);
    }

    public void handleSwordAttack(){
        Debug.Log("hit!");
        AddReward(4.0f);
        deadMinionNum ++;
        if (deadMinionNum == 10){
            Debug.Log("slayed ten minions!");
            EndEpisode();
        }
    }

    public void handleSwordAttackBoss(){
        if(Boss.GetComponent<BossState>().healthPoint<=0){
            EndEpisode();
        }
        AddReward(10.0f);
    }


    public void handleSwordNotAttack(){
        Debug.Log("doesnt hit anything");
        AddReward(-0.1f);
    }

    public void handleOnPlatfrom(){
        AddReward(0.1f);
        
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.name=="Square"){
            handleOnPlatfrom();
            // Debug.Log(Neo.transform.position.y/10);
            // AddReward(Neo.transform.position.y/10);
        }
        if(other.gameObject.name=="LeftBound"|| other.gameObject.name=="RightBound"){
            
            AddReward(-0.5f);
        }
    }

    public void nearByBoss(Vector3 distance){
        float distanceToTarget = 1/(distance.magnitude);
        Debug.Log(distanceToTarget*20);
        SetReward(distanceToTarget*20);
    }

    public void HandleCollectCoin(){
        AddReward(2.0f);
    }

}
