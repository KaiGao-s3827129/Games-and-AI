using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Flock/Behavior/Avoidance")]
public class AvoidanceBehavior : FlockBehavior
{
    // Start is called before the first frame update
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        int count = context.Count;
        if(count==0){
            return Vector2.zero;
        }
        Vector2 avoidanceMove = Vector2.zero;
        int avoid = 0;
        foreach(Transform one in context){
            if(Vector2.SqrMagnitude(one.position-agent.transform.position)<flock.SquareAvoidanceRadius){
                avoid++;
                avoidanceMove+=(Vector2)(agent.transform.position-one.position);
            }
            
        }
        if(avoid>0){
            avoidanceMove = avoidanceMove/avoid;
        }
        return avoidanceMove;
    }
}
