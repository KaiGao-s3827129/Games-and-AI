using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingBase : MonoBehaviour
{
    public GameObject agentPrefab;
    List<GameObject> agents = new List<GameObject>();
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

    // public float SquareAvoidanceRadius{ get { return squareAvoidanceRadius;}}

    // Start is called before the first frame update
    void Start()
    {
        // squareMaxForce = maxForce * maxForce;
        // squareNeighborRadius = neighborRadius * neighborRadius;
        // avoidance radius is smaller than neighbor radius
        // squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
    
        for (int i = 0; i < agentCount; i++)
        {
            GameObject newAgent = Instantiate(
                agentPrefab,
                Random.insideUnitCircle * agentCount * AgentDensity,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform
            );

            newAgent.name = "Agent " + i;
            agents.Add(newAgent);

            // InvokeRepeating("FlockingBehaviors.getWanderVector", 1f, 1f);
        }

        // InvokeRepeating("getRandomPos",5f, 5f);
        // InvokeRepeating("addForce",5f, 1f);

        // Destroy(GameObject.Find("Agent 7").GetComponent<Collider2D>());

    }


    void getRandomPos(){
        randomPos = new Vector2(Random.Range(-60f, 60f), Random.Range(-60f, 60f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // void addForce(){
    //     foreach(GameObject agent in agents){
    //         Debug.DrawLine(agent.transform.position, (Vector3)((Vector2)agent.transform.position + (Vector2)move), Color.white, 0.1f);
    //         agent.GetComponent<Rigidbody2D>().AddForce(move);
    //     }
    // }
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

    Vector2 getDir(GameObject agent, List<GameObject> context){
        force = Vector2.zero;
        if(context.Count == 0){
            // force += new Vector2(-agent.transform.position.x, -agent.transform.position.y) * wanderWeight;
            force = Vector2.zero;
            force += FlockingBehaviors.getCohesionVector(agent, context, cohesionWeight, randomPos, maxForceMagnitude);
        }
        else{
            Vector2 cohesion = new Vector2(0,0);
            if(agent.name.Substring(0,3)!="Min"){
                cohesion = FlockingBehaviors.getCohesionVector(agent, context, cohesionWeightPos, randomPos, maxForceMagnitude);
            }
            Vector2 alignment = FlockingBehaviors.getAlignmentVector(agent, context, alignmentWeight);
            Vector2 cohesionOld = FlockingBehaviors.getCohesionVector(agent, context, cohesionWeight);
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

