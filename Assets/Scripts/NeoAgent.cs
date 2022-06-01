using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
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
    public GameObject Neo;
    public GameObject Boss;
    public Transform coinLocations;
    public float previousHeight;
    private Collider2D previourPlatform;
    private Vector2 playerSize;
    private int onSamePlatformTimes;
    private int punishTime;
    private int collectedCoins;

    private float currentTime;

    private int coinCount;
    private int privateCollectedCoints;
    private int currentStepCount;
    private int jumpedCount;
    private int privateJumpedCount;

    private List<Collider2D> jumpedPlatformsList = new List<Collider2D>();

    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true;
        currentStepCount = StepCount;
        // anim = GetComponent<Animator>();
        neo = GetComponent<Rigidbody2D>();
        Neo = GameObject.Find("Neo");
        sword = GameObject.Find("sword");
        Boss = GameObject.Find("TheBoss");
        previousHeight = Neo.transform.position.y;
        neoStartPos = transform.position;
        randomPlatform = GameObject.Find("Platforms").GetComponent<RandomPlatform>();
        playerSize = GetComponent<SpriteRenderer>().bounds.size;

        // minions = randomPlatform.agents;
        // minionNames = randomPlatform.leaderMinions;
        privateCollectedCoints = 0;
        collectedCoins = 0;
        jumpedCount = 0;
        privateJumpedCount = 0;
        currentTime = Time.frameCount;
        // Get coin count
        foreach (Transform coin in coinLocations)
        {
            coinCount++;
        }

    }

    public void Restart()
    {    
        // initialize Neo state
        transform.position = neoStartPos;
        for(int i = 0; i < randomPlatform.agents.Count; i++)
        {
            Destroy(randomPlatform.agents[i]);
        }
        // initialize boss state
        Boss.GetComponent<BossState>().healthPoint=500;
        Boss.GetComponent<BossState>().healthBar.SetHealth(500);
        
        // clear platforms
        randomPlatform.agents.Clear();
        randomPlatform.leaderMinions.Clear();

        // initialize coin state
        foreach(Transform coin in coinLocations){
            coin.gameObject.SetActive(true);
            coinCount++;
        }
        
        // initialize parameter
        jumpedPlatformsList = new List<Collider2D>();
        punishTime = 0;
        collectedCoins = 0;
        onSamePlatformTimes = 0;
        coinCount = 0;
        deadMinionNum = 0;
        jumpedCount = 0;
        privateJumpedCount = 0;
    }

    
    public override void OnEpisodeBegin(){
        // Debug.Log("episode began");
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
                if(Neo.GetComponent<NeoMovement>().isGround){
                    Neo.GetComponent<NeoMovement>().jumpRequest=true;
                } 
                break;
            case 1:
                privateJumpedCount = jumpedCount;
                jumpedCount++;
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
        NoCoinCollect();
        if (Time.frameCount % 5 == 0)
        {
            if (jumpedCount == privateJumpedCount)
            {
                AddReward(-2f);
            }
        }

        if (Neo.GetComponent<NeoMovement>().isGround)
        {
            AddReward(-1f);
        }
    }

    public void handleSwordAttack(){
        // Debug.Log("hit!");
        AddReward(3.0f);
        deadMinionNum ++;
        if (deadMinionNum == 10){
            // Debug.Log("slayed ten minions!");
            EndEpisode();
        }

    }

    public void handleSwordAttackBoss(){
        AddReward(10.0f);
        if (Boss.GetComponent<BossState>().healthPoint == 0)
        {
            AddReward((float)Math.Abs(GetCumulativeReward()));
            EndEpisode();
        }
    }


    public void handleSwordNotAttack(){
        // Debug.Log("doesnt hit anything");
        AddReward(-1f);
    }

    public void handleOnPlatfrom(){
        AddReward(1f);
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="RewardSquare"){
            if (other == previourPlatform)
            {
                HandleJumpOnSamePlatform();
            }
            else
            {
                if (!jumpedPlatformsList.Contains(other))
                {
                    onSamePlatformTimes = 0;
                    punishTime = 0;
                    AddReward(5f);
                    jumpedPlatformsList.Add(other);
                }
                else
                {
                    Debug.Log("Jump this platform before");
                    AddReward(-1f);
                }
                
            }
            previourPlatform = other;
        }
    }

    public void HandleJumpOnSamePlatform()
    {
        AddReward(-1f);
        // Debug.Log("On Same Platform!");
        onSamePlatformTimes++;
        // Debug.Log(onSamePlatformTimes);
        if (onSamePlatformTimes >= 5)
        {
            punishTime++;
            AddReward(-(float)Math.Pow(1.1, punishTime));
            onSamePlatformTimes = 0;
        }
    }

    public void nearByBoss(Vector3 distance){
        // float distanceToTarget = 1/(distance.magnitude * 5);
        // Debug.Log(distanceToTarget);
        // SetReward(distanceToTarget/10);
    }

    public void HandleCollectCoin()
    {
        privateCollectedCoints = collectedCoins;
        collectedCoins++;
        coinCount--;
        AddReward(5f * collectedCoins);
    }

    // No coin get in 5sec, then -1 reward
    public void NoCoinCollect()
    {
        if (coinLocations.childCount != 0)
        {
            if (privateCollectedCoints == collectedCoins)
            {
                AddReward(-1f);
            }
        }
        else
        {
            AddReward(100f);
            EndEpisode();
        }
    }
}
