using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFlockingMinion : MonoBehaviour
{
    public GameObject ant;
    private FlockingMinionState flockingMinionState;
    private Rigidbody2D rb2d;
    public int startingCount = 5;
    public FlockingMinionMovement agentPrefab;
    public Minion minionAgentPrefab;
    public float AgentDensity = 0.8f;
    // Start is called before the first frame update
    void Start()
    {
        Minion minAgent = Instantiate(
                minionAgentPrefab,
                Random.insideUnitCircle * startingCount * AgentDensity,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform
                );
            minAgent.name = "Minion";
        for (int i = 0; i < startingCount; i++)
        {
            FlockingMinionMovement newAgent = Instantiate(
                agentPrefab,
                Random.insideUnitCircle * startingCount * AgentDensity,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform
                );
            newAgent.name = "Flocking " + i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
