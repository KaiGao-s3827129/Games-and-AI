using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MinionAgent : Agent
{
    public float speed;
    private Rigidbody2D rb2d;
    private SpriteRenderer sr;
    private Vector3 minionStartPos; 
    public GameObject platforms;
    public RandomPlatform randomPlatform;
    [SerializeField] private Transform targetTransform;
    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true;
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        minionStartPos = transform.position;
        platforms = GameObject.Find("Platforms");
        randomPlatform  = platforms.GetComponent<RandomPlatform>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("Episode");
        transform.position = minionStartPos;
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(targetTransform.position);
        foreach(var one in randomPlatform.platformList){
            sensor.AddObservation(one);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveHorizontal = 0.0f;
        float moveVertical = 0.0f;
        int action = actions.DiscreteActions[0];
        switch(action){
            case 0:
                break;
            case 1:
                moveHorizontal = -1.0f;
                break;
            case 2:
                moveVertical = 1.0f;
                break;
            case 3:
                moveHorizontal = 1.0f;
                break;
            case 4:
                moveVertical = -1.0f;
                break;
        }
        
        if(moveHorizontal>0){
            sr.flipX = true;
        }
        else if(moveHorizontal<0){
            sr.flipX=false;
        }

        Vector2 forceDirection = new Vector2(moveHorizontal,moveVertical);
        rb2d.AddForce(forceDirection*speed);
    }

    public void HandleGetNeo(){
        Debug.Log(11111111111);
        AddReward(10.0f);
        OnEpisodeBegin();
    }

    private void OnTriggerEnter2D(Collider2D other){
        Debug.Log(other.name);
        Debug.Log(2222222222);
        if(other.TryGetComponent<PlatForm>(out PlatForm platformPrefab)){
            AddReward(-0.01f);
            // EndEpisode();
        }  
    }

}
