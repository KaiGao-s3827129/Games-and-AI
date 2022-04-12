using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlockingBehaviors : MonoBehaviour
{
    
    public static Vector2 getAlignmentVector(GameObject agent, List<GameObject> context, float weight)
    {
        
        Vector2 direction = Vector2.zero;
        foreach (GameObject neighbor in context)
        {
            // direction += neighbor.GetComponent<Rigidbody2D>().velocity;
            direction += (Vector2)neighbor.transform.right;
        }

        direction /= context.Count;
        // direction.Normalize();
        return direction * weight;
    }

    public static Vector2 getCohesionVector(GameObject agent, List<GameObject> context, float weight)
    {
        Vector2 direction = Vector2.zero;
        foreach (GameObject neighbor in context)
        {
            direction += (Vector2)neighbor.transform.position;
        }
        direction /= context.Count;
        direction -= (Vector2) agent.transform.position;
        // Vector3 leaderPos = GameObject.Find("Leader").transform.position;
        // direction = (Vector2)(leaderPos - agent.transform.position);
        direction *= weight;
        // direction = Seek(agent, direction);
        // Debug.Log(direction);
        // direction.Normalize();
        return direction;
    }

    public static Vector2 getCohesionVector(GameObject agent, List<GameObject> context, float weight, Vector2 randomPos, float maxForceMagnitude)
    {
        Vector2 direction = Vector2.zero;
        // foreach (GameObject neighbor in context)
        // {
        //     direction += (Vector2)neighbor.transform.position;
        // }
        // direction -= (Vector2) agent.transform.position;
        direction = randomPos - (Vector2)agent.transform.position;
        if(direction.magnitude > maxForceMagnitude){
            direction.Normalize();
        }
        direction *= weight;
        // direction = Seek(agent, direction);
        // Debug.Log(direction);
        return direction;
    }

    public static Vector2 getAvoidanceVector(GameObject agent, List<GameObject> context, float weight)
    {
        Vector2 direction = Vector2.zero;
        Vector2 currPos = (Vector2) agent.transform.position;
        foreach (GameObject neighbor in context)
        {
            // if (Vector2.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius){}
            direction += currPos - (Vector2) neighbor.transform.position;
        }
        direction /= context.Count;
        direction *= weight;
        direction = Seek(agent, direction);
        // direction.Normalize();
        return direction;
    }

    public static Vector2 getStayVector(GameObject agent, float weight, Vector2 center, float radius)
    {
        Vector2 centerOffset = center - (Vector2)agent.transform.position;
        float t = centerOffset.magnitude / radius;
        // if (t < 0.9f){
        //     return Vector2.zero;
        // }

        return centerOffset * t * t * weight;
    }

    public static Vector2 getWanderVector(GameObject agent, List<GameObject> context, float weight)
    {
        Vector2 randomVector = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        // randomVector.Normalize();
        // Debug.DrawLine(agent.transform.position, (agent.transform.position + (Vector3)(randomVector * weight)), Color.white, 0.1f);
        // Debug.Log(randomVector);
        return randomVector * weight;
    }

    static Vector2 Seek(GameObject agent, Vector2 desiredDir){
        Vector2 currDir = agent.GetComponent<Rigidbody2D>().velocity;
        return (desiredDir - currDir);
    }

    // public override Vector2 getAlignmentVector(FlockAgent agent, List<Transform> context, Flock flock)
    // {}

}