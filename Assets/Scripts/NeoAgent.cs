using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEditor.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class NeoAgent : Agent
{
    // public Animator anim;
    private Rigidbody2D neo;
    public bool facingRight = true;
    public GameObject sword;
    private Vector3 neoStartPos;
    // private RandomPlatform randomPlatform;
    private int deadMinionNum;
    public GameObject Neo;
    public GameObject Boss;
    public Transform coinLocations;
    public float previousHeight;
    private Collider2D previourPlatform;
    private int onSamePlatformTimes;
    private int punishTime;
    private int collectedCoins;
    private int privateCollectedCoints;
    private int jumpedCount;
    private int privateJumpedCount;
    private int coinCount;

    private List<Collider2D> jumpedPlatformsList = new List<Collider2D>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Started");
        Application.runInBackground = true;
        // anim = GetComponent<Animator>();
        neo = GetComponent<Rigidbody2D>();
        Neo = GameObject.Find("Neo");
        sword = GameObject.Find("sword");
        Boss = GameObject.Find("TheBoss");
        neoStartPos = Neo.transform.position;
        previousHeight = Neo.transform.position.y;
        // randomPlatform = GameObject.Find("Platforms").GetComponent<RandomPlatform>();

        // minions = randomPlatform.agents;
        // minionNames = randomPlatform.leaderMinions;
        privateCollectedCoints = 0;
        collectedCoins = 0;
        jumpedCount = 0;
        privateJumpedCount = 0;
        // Get coin count
        foreach (Transform coin in coinLocations)
        {
            coinCount++;
        }
    }
    
    void Update()
    {
        // if (Time.frameCount % 20== 0)
        // {
        //     NoCoinCollect();
        // }
    }

    public void Restart()
    {
        // initialize Neo state
        transform.position = neoStartPos;
        // for(int i = 0; i < randomPlatform.agents.Count; i++)
        // {
        //     Destroy(randomPlatform.agents[i]);
        // }
        // initialize boss state
        Boss.GetComponent<BossState>().healthPoint=500;
        Boss.GetComponent<BossState>().healthBar.SetHealth(500);
        
        // clear platforms
        // randomPlatform.agents.Clear();
        // randomPlatform.leaderMinions.Clear();

        // initialize coin state
        foreach(Transform coin in coinLocations){
            coin.gameObject.SetActive(true);
            coinCount++;
        }
        
        // initialize parameter
        jumpedPlatformsList.Clear();
        punishTime = 0;
        collectedCoins = 0;
        onSamePlatformTimes = 0;
        coinCount = 0;
        deadMinionNum = 0;
        jumpedCount = 0;
        privateJumpedCount = 0;
        
        SetReward(0f);
    }
    
    public override void OnEpisodeBegin(){
        // Debug.Log("episode began");

        Restart();
    }
    
    
    // Actions
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
                moveHorizontal = 1.0f;
                break;
            case 2:
                moveHorizontal = -1.0f;
                break;
        }

        switch(jump_action){
            case 1:
                if(Neo.GetComponent<NeoMovement>().isGround){
                    Neo.GetComponent<NeoMovement>().jumpRequest=true;
                } 
                break;
            case 0:
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
    }
    

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(0.0f);
    }

    
    /**
     *  Attack Related
     */
    public void handleSwordAttack(){
        // Debug.Log("hit!");
        // AddReward(3.0f);
        // deadMinionNum ++;
        // if (deadMinionNum == 10){
        //     // Debug.Log("slayed ten minions!");
        // EndEpisode();
        // }

    }

    public void handleSwordAttackBoss(){
        AddReward(10.0f);
        
        if (Boss.GetComponent<BossState>().healthPoint <= 0)
        {
            Debug.Log("Boss Dead.");
            AddReward((float)Math.Abs(GetCumulativeReward()));
            // Debug.Log(GetCumulativeReward());
            EndEpisode();
        }
        
    }


    public void handleSwordNotAttack(){
        // Debug.Log("doesnt hit anything");
        // AddReward(-1f);
    }

    
    /**
     * Jump Related
     *
     * 1. Jump on same platform
     * 2. Jump on the platform jumped before
     */
    
    // Jump to platforms or ground reward 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name =="Square"){
            
            // Jump on same platform
            if (other == previourPlatform)
            {
                HandleJumpOnSamePlatform();
            }
            else if (jumpedPlatformsList.Contains(other))
            {
                // Debug.Log("Jump this platform before");
                AddReward(-1f);

            }
            // else
            // {
            //     onSamePlatformTimes = 0;
            //     punishTime = 0;
            //     AddReward(10f);
            //     jumpedPlatformsList.Add(other);
            // }
            
            previourPlatform = other;
        }

        if (other.name == "Bound")
        {
            SetReward(-2 * (MaxStep - StepCount));
            // Debug.Log(GetCumulativeReward());
            Debug.Log("Touch Bound");
            EndEpisode();
        }
    }

    public void HandleJumpOnSamePlatform()
    {
        AddReward(-1f);
        Debug.Log("On Same Platform!");
        onSamePlatformTimes++;
        if (onSamePlatformTimes >= 5)
        {
            punishTime++;
            AddReward(-(float)Math.Pow(1.5f, punishTime));
            onSamePlatformTimes = 0;
        }

        if (punishTime >= 5)
        {
            SetReward(-2 * (MaxStep - StepCount));
            // Debug.Log(GetCumulativeReward());
            Debug.Log("Punish time over 5");
            EndEpisode();
        }
    }

    /**
     * Coin collected Related
     *
     * 1. whether collect coin in 5 per frame
     */
    public void HandleCollectCoin()
    {
        privateCollectedCoints = collectedCoins;
        collectedCoins++;
        coinCount--;
        AddReward(5f);
    }

    // No coin get in 5 per frame, then -1 reward
    public void NoCoinCollect()
    {
        if (coinCount != 0)
        {
            if (privateCollectedCoints == collectedCoins)
            {
                // Debug.Log("Didn't get coin in 5 frame.");
                AddReward(-2f);
            }
        }
    }

    public Vector2 randomPosition()
    {
        float x = Random.Range(-50f, 40f);
        float y = Random.Range(-20f, 60f);
        Vector2 pos = new Vector2(x, y);
        return pos;
    }
}
