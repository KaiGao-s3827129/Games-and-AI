using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class NeoAgent : Agent
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void OnEpisodeBegin(){
        Debug.Log("An episode began");
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
