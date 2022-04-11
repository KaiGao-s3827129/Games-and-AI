using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Flock/Behavior/Alignment")]
public class AlignmentBehavior : FlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        int count = context.Count;
        if(count==0){
           return agent.transform.up;
        }
        Vector2 alignmentMove = Vector2.zero;
        foreach(Transform one in context){
            alignmentMove += (Vector2)one.transform.up;
        }
        alignmentMove=alignmentMove/count;
        return alignmentMove;
    }
}
