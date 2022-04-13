using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class RandomPlatform : MonoBehaviour
{
    //Random create platfrom
    public GameObject platform;
    public float yMin, yMax, xMin, xMax;
    public float spawnTime;
    public float spawnRepeatingTime;
    public float destroyTime;
    public LayerMask layerMask;
    private Transform platformParent;
    public List<Vector2> platformList = new List<Vector2>();
    public GameObject skillBox;
    public GameObject weaponBox;
    public int boxCount;
    public int platformCount;
    //Create Minion
    private FlockingMinionState flockingMinionState;
    public int startingCount = 5;
    public FlockingMinionMovement AgentPrefabs;
    public GameObject minionAgentPrefab;


    public int leaderMinionNumber;
    public int followMinionNumber;
    public List<string> leaderMinions;





    //flocking
    public GameObject agentPrefab;
    public List<GameObject> agents = new List<GameObject>();
    // public FlockBehavior behavior;

    [Range(2, 500)]
    public int agentCount = 250;

    [Range(0.1f, 10f)]
    public float AgentDensity = 1f;

    // [Range(1f, 100f)]
    // public float forceMultiplier = 10f;
    [Range(1f, 60f)]
    public float maxForceMagnitude = 10f;
    [Range(1f, 20f)]
    public float neighborRadius = 1.5f;

    public Vector2 center;
    public float radius;
    // [Range(0f, 1f)]
    // public float avoidanceRadiusMultiplier = 0.8f;
    // public float WanderDistance;
	// public float WanderJitter;

	// private Vector2 m_vWanderTarget;

    public float avoidanceWeight;

    public float cohesionWeight;

    public float cohesionWeightPos;

    public float alignmentWeight;

    public float stayWeight;

    public float wanderWeight;

    private Vector2 force;

    public Vector2 randomPos = new Vector2(0f, 0f);

    private Vector2 move;

    void Start()
    {
        leaderMinions = new List<string>();
        platformParent = GameObject.Find("Platforms").transform;
        leaderMinionNumber = 0;
        platformSpawn();
        // InvokeRepeating("createMinion", 0, 10f);
        InvokeRepeating("boxSpawn", spawnTime, spawnRepeatingTime);





        // for (int i = 0; i < agentCount; i++)
        // {
        //     GameObject newAgent = Instantiate(
        //         agentPrefab,
        //         Random.insideUnitCircle * agentCount * AgentDensity,
        //         Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
        //         transform
        //     );

        //     newAgent.name = "Agent " + i;
        //     agents.Add(newAgent);

        //     // InvokeRepeating("FlockingBehaviors.getWanderVector", 1f, 1f);
        // }
    }

    void platformSpawn()
    {
        float y;
        float x;
        Vector2 pos;
        GameObject clone = new GameObject("dummy");
        Vector2 platformSize;

        for (int i = 0; i < platformCount; i++)
        {
            do
            {
                Destroy(clone);
                y = Random.Range(yMin, yMax);
                x = Random.Range(xMin, xMax);
                pos = new Vector2(x, y);
                clone = Instantiate(platform, pos, transform.rotation);
                platformSize = clone.GetComponent<Collider2D>().bounds.size;
                clone.transform.SetParent(platformParent);
            }
            while (hasObstacleAtPosition(pos, platformSize, layerMask));
            platformList.Add(pos);
            clone = new GameObject("dummy");

        }
    }

    bool hasObstacleAtPosition(Vector2 position, Vector2 size, LayerMask layerMask)
    {
        Vector2 checkSize = new Vector2(size.x * 2f, size.y * 3.5f);
        Collider2D[] intersection = Physics2D.OverlapBoxAll(position, checkSize, 0f, layerMask);
        return intersection.Length > 1;
    }

    void boxSpawn()
    {
        Vector2 boxPos;
        int platformNo;
        List<int> selectedPlatforms = new List<int>();

        for (int i = 0; i < boxCount; i++)
        {
            do
            {
                platformNo = Random.Range(1, platformCount);
            } while (selectedPlatforms.Contains(platformNo));
            selectedPlatforms.Add(platformNo);
            Vector2 platformPos = platformList[platformNo];
            float x = Random.Range(platformPos.x - 10f + 4f, platformPos.x + 10f - 4f);
            float y = platformPos.y + 4f;
            boxPos = new Vector2(x, y);
            // Debug.Log("create a skill box on (" + x + "," + y + ")");
            GameObject gameObjectSkillBox = Instantiate(skillBox, boxPos, transform.rotation);
            Destroy(gameObjectSkillBox, destroyTime);
        }
        
        for (int i = 0; i < boxCount; i++)
        {
            do
            {
                platformNo = Random.Range(1, platformCount);
            } while (selectedPlatforms.Contains(platformNo));
            selectedPlatforms.Add(platformNo);
            Vector2 platformPos = platformList[platformNo];
            float x = Random.Range(platformPos.x - 10f + 4f, platformPos.x + 10f - 4f);
            float y = platformPos.y + 4f;
            boxPos = new Vector2(x, y);
            // Debug.Log("create a weapon box on (" + x + "," + y + ")");
            GameObject gameObjectSkillBox = Instantiate(weaponBox, boxPos, transform.rotation);
            Destroy(gameObjectSkillBox, destroyTime);
        }

    }

    void createMinion()
    {
        if(leaderMinions.Count<=1){
            float x, y;
            Vector2 boxPos;
            int platformNo;
            List<int> selectedPlatforms = new List<int>();
            do
            {
                platformNo = Random.Range(1, platformCount);
            } while (selectedPlatforms.Contains(platformNo));
            selectedPlatforms.Add(platformNo);
            Vector2 platformPos = platformList[platformNo];
            x = Random.Range(platformPos.x - 10f + 4f, platformPos.x + 10f - 4f);
            y = platformPos.y + 5f;
            boxPos = new Vector2(x, y);
            leaderMinionNumber++;
            leaderMinions.Add("Minion" + leaderMinionNumber);
            GameObject minAgent = Instantiate(
            minionAgentPrefab,
            boxPos,
                    Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                    transform
            );
            minAgent.name = "Minion" + leaderMinionNumber;
            agents.Add(minAgent);
            boxPos.y +=1;
            for (int i = followMinionNumber; i < followMinionNumber+2; i++)
            {
                boxPos.x+=5f;
                GameObject newAgent = Instantiate(
                    agentPrefab,
                    boxPos,
                    Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                    transform
                    );
                newAgent.name = "Flocking " + i;
                agents.Add(newAgent);
            }
            followMinionNumber +=2;
            boxPos.x -= 10f;
            for (int i = followMinionNumber; i < followMinionNumber+2; i++)
            {
                boxPos.x-=5f;
                GameObject newAgent = Instantiate(
                    agentPrefab,
                    boxPos,
                    Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                    transform
                    );
                newAgent.name = "Flocking " + i;
                agents.Add(newAgent);
                
            }
            followMinionNumber +=2;
            boxPos.x+=10f;
            boxPos.y+=5f;
            GameObject newUpAgent = Instantiate(
                    agentPrefab,
                    boxPos,
                    Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                    transform
                    );
                newUpAgent.name = "Flocking " + followMinionNumber;
                agents.Add(newUpAgent);
            followMinionNumber++;
        }
    }




    void FixedUpdate() {
        foreach(GameObject agent in agents){
            List<GameObject> context = GetNearbyObjects(agent);
            // agent.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 3f);
            move = getDir(agent, context);

            
            float hasForce = Random.value;
            float threshold = 0.1f;

            // if(hasForce < threshold){
            //     Debug.DrawLine(agent.transform.position, (Vector3)((Vector2)agent.transform.position + (Vector2)move), Color.white, 0.1f);
            //     agent.GetComponent<Rigidbody2D>().AddForce(move);
            // }

            Vector2 steeringForce = new Vector2(0, 0);
            if(hasForce < threshold){
                float slowDownRadius = 1.1f;
                
                Vector2 toTarget = (Vector3)((Vector2)agent.transform.position + (Vector2)move)-agent.transform.position;
                float distance = toTarget.magnitude;
                Vector2 desiredVelocity = (toTarget).normalized * (distance / slowDownRadius);
                steeringForce = desiredVelocity - agent.GetComponent<Rigidbody2D>().velocity;
                Debug.DrawLine(agent.transform.position, (Vector3)((Vector2)agent.transform.position + (Vector2)move), Color.white, 0.1f);

                agent.GetComponent<Rigidbody2D>().AddForce(steeringForce);
            }
            
            
            // if(agent.name == "Agent 7"){
            //     Debug.Log(context.Count);
            //     force = Vector2.zero;
            //     force += FlockingBehaviors.getWanderVector(agent, context, wanderWeight);
            //     Debug.Log(force);
            //     agent.GetComponent<Rigidbody2D>().AddForce(force);
            //     // continue;
            // }else{
            // agent.GetComponent<Rigidbody2D>().AddForce(move);
            // }
            // normalize force if too big
            // if (move.sqrMagnitude > squareMaxForce){
            //     move = move.normalized * maxForce;
            // }  
        }
    }

    void getRandomPos(){
        randomPos = new Vector2(Random.Range(-60f, 60f), Random.Range(-60f, 60f));
    }





    Vector2 getDir(GameObject agent, List<GameObject> context){
        force = Vector2.zero;
        if(context.Count == 0){
            // force += new Vector2(-agent.transform.position.x, -agent.transform.position.y) * wanderWeight;
            force = Vector2.zero;
            force += FlockingBehaviors.getCohesionVector(agent, context, cohesionWeight, randomPos, maxForceMagnitude);
        }
        else{
            Vector2 alignment = FlockingBehaviors.getAlignmentVector(agent, context, alignmentWeight);
            Vector2 cohesionOld = FlockingBehaviors.getCohesionVector(agent, context, cohesionWeight);
            Vector2 cohesion = FlockingBehaviors.getCohesionVector(agent, context, cohesionWeightPos, randomPos, maxForceMagnitude);
            Vector2 avoidance = FlockingBehaviors.getAvoidanceVector(agent, context, avoidanceWeight);
            force += alignment + cohesion + avoidance + cohesionOld;

            // force += AccForce(0f, avoidance);
            // force += AccForce(force.magnitude, cohesion);
            // force += AccForce(force.magnitude, alignment);

        }

        // force += FlockingBehaviors.getWanderVector(agent, context, wanderWeight);

        // force += FlockingBehaviors.getStayVector(agent, stayWeight, center, radius);

        // Debug.DrawLine(agent.transform.position, (Vector3)((Vector2)agent.transform.position + (Vector2)force), Color.white, 0.1f);
        return force;

        // Vector2 seekForce = Seek(agent, force);
        // Debug.DrawLine(agent.transform.position, agent.transform.position + (Vector3)seekForce, Color.white, 0.1f);
        // return seekForce;
    }

    private Vector2 AccForce(float SteeringMag,Vector2 ForceToAdd){

		Vector2 returnForce = new Vector2(0,0);
		float ForceToAddMag = ForceToAdd.magnitude;
		float RemainingForceMag = maxForceMagnitude - SteeringMag;

		if(RemainingForceMag <=  0)
			return returnForce;

		if (ForceToAddMag < RemainingForceMag){
            return ForceToAdd;
        }
		else{
            return (ForceToAdd.normalized * RemainingForceMag);
        }
						

	}

    List<GameObject> GetNearbyObjects(GameObject agent){
        List<GameObject> context = new List<GameObject>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);
        foreach (Collider2D collider in contextColliders)
        {
            if ( collider != agent.GetComponent<Collider2D>()){
                context.Add(collider.gameObject);
            }
        }
        return context;
    }

}