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
    public float last = 10000.0f;
    public float lastDistanceToBoss = 10000.0f;
    public string lastLoc;
    private int onSamePlatformTimes;
    private int punishTime;

    // Start is called before the first frame update
    void Start()
    {   //Switch boss location Vector
        bossLocation1 = new Vector2(55.6f,22.9f);
        bossLocation2 = new Vector2(-55.8f, 30.8f);
        Application.runInBackground = true;
        neo = GetComponent<Rigidbody2D>();
        Neo = GameObject.Find("Neo");
        sword = GameObject.Find("sword");
        Boss = GameObject.Find("TheBoss");
        previousHeight = Neo.transform.position.y;
        neoStartPos = transform.position;
        randomPlatform = GameObject.Find("Platforms").GetComponent<RandomPlatform>();
        Boss.transform.position = bossLocation1;
        // repeat check the distance between boss and agent
        InvokeRepeating("nearByBoss", 0, 0.5f);
    }

    public void Restart()
    {    lastLoc = "Plat";
        last = 10000.0f;
        //Switch boss loaction when start
        if(Boss.transform.position.x==bossLocation1.x && Boss.transform.position.y==bossLocation1.y){
            Boss.transform.position = bossLocation2;
        }else{
            Boss.transform.position = bossLocation1;
        }
        deadMinionNum = 0;
        //rendom generate Neo start location
        var random = new System.Random();
        var list = randomPlatform.platformList;
        int index = random.Next(list.Count);
        Vector2 newLocation =  new Vector2(list[index].x,list[index].y+4);
        transform.position = newLocation;
        for(int i = 0; i < randomPlatform.agents.Count; i++)
        {
            Destroy(randomPlatform.agents[i]);
        }
        //Update the Boss health point when start
        Boss.GetComponent<BossState>().healthPoint=500;
        Boss.GetComponent<BossState>().healthBar.SetHealth(500);
        randomPlatform.agents.Clear();
        randomPlatform.leaderMinions.Clear();
        foreach(Transform coin in coinLocations){
            coin.gameObject.SetActive(true);
        }
        punishTime = 0;
        onSamePlatformTimes = 0;
        neo.GetComponent<NeoState>().healthPoint = 10;
        neo.GetComponent<NeoState>().previousHealthPoint = 10;
        
    }
    public override void OnEpisodeBegin(){
        Debug.Log("episode began");
        Restart();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //Add boss position and Neo position
        sensor.AddObservation(transform.position);
        sensor.AddObservation(Boss.transform.position);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        //Debug.Log("Action taken was: " + actionBuffers.DiscreteActions[0]);
        int move_action = actionBuffers.DiscreteActions[0];
        int jump_action = actionBuffers.DiscreteActions[1];
        int attack_action = actionBuffers.DiscreteActions[2];
        float moveHorizontal = 0.0f;
        //move action
        switch(move_action){
            case 0:
                moveHorizontal = -1.0f;
                break;
            case 1:
                moveHorizontal = 1.0f;
                break;
            // case 3:
            //     // Neo.GetComponent<NeoMovement>().Shoot();
            //     break;
        }
        switch(attack_action){
            case 0:
            
                sword.GetComponent<sword>().Attack();
                break;
            case 1:
                
                break;
        }
        switch(jump_action){
            case 0:
                if(Neo.GetComponent<NeoMovement>().isGround || Neo.GetComponent<NeoMovement>().jumpCount >= 1){
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
    }

    //attack minion get 4 rewards
    public void handleSwordAttack(){
        // Debug.Log("hit!");
        AddReward(4.0f);
        deadMinionNum ++;
        if (deadMinionNum == 10){
            Debug.Log("slayed ten minions!");
            EndEpisode();
        }
    }

    //Attack boss 10 rewards
    public void handleSwordAttackBoss(){
        if(Boss.GetComponent<BossState>().healthPoint<=0){
            EndEpisode();
        }
        AddReward(10.0f);
    }

    //Attack and not damage enemy minus 0.1 reward.
    public void handleSwordNotAttack(){
        // Debug.Log("doesnt hit anything");
        AddReward(-0.1f);
    }

    //Agent take damage minus 0.1 reward
    public void takenDamage(){
        Debug.Log("taken dmg");
        AddReward(-1.0f);
    }

    private void OnTriggerEnter2D(Collider2D other){
        // if(other.gameObject.name=="SkillBox(Clone)"){
        //     Debug.Log("Skill");
        //     AddReward(3.0f);
        // }
        // if(other.gameObject.name=="WeaponBox(Clone)"){
        //     Debug.Log("Weapon");
        //     AddReward(3.0f);
        // }
        // Debug.Log(other.gameObject.name);

        //Jump on the ground
        if(other.gameObject.name=="Ground"){
            if(lastLoc =="Plat"){
                AddReward(-0.7f);
            }else{
            AddReward(-1.5f);
            
            last = 10000.0f;
            lastLoc = "Grou";
        }
        }
        //Jump on the platform
        else if(other.gameObject.name=="Square"){
            lastLoc = "Plat";
            Vector2 distance = Boss.transform.position - other.transform.position;
            float dis = distance.magnitude;
            if(dis<last){
                last = dis;
                AddReward(2.0f);
            }else if(dis>last){
                last = dis;
                AddReward(-2.0f);
            }
            else if(dis ==last){
                last = dis;
                // AddReward(-0.6f);
                HandleJumpOnSamePlatform();
            }
        }
    }

    //Jump on the platform minus reward
    public void HandleJumpOnSamePlatform()
    {
        Debug.Log(onSamePlatformTimes);
        AddReward(-0.2f);
        Debug.Log("On Same Platform!");
        onSamePlatformTimes++;
        if (onSamePlatformTimes >= 5)
        {
            punishTime++;
            AddReward(-1f);
            onSamePlatformTimes = 0;
        }

        if (punishTime >= 5)
        {
            // Debug.Log(GetCumulativeReward());
            AddReward(-5f);
            Debug.Log("Punish time over 5");
            EndEpisode();
        }
    }

    //Touch bound minus reward
    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.name=="Bound"){
            AddReward(-1.0f);
        }
    }

    //Near by boss get 0.2 reward.
    public void nearByBoss(){
        Vector2 distance = Boss.transform.position-Neo.transform.position;
        float distanceToTarget = distance.magnitude;
        if(distanceToTarget < lastDistanceToBoss){
            lastDistanceToBoss = distanceToTarget;
            AddReward(0.2f);
        }else{
            lastDistanceToBoss = distanceToTarget;
            AddReward(-0.2f);
        }
    }

    // public void HandleCollectCoin(){
    //     AddReward(0.8f);
    // }
}