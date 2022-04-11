using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Flock/Behavior/Combine")]
public class Combine : FlockBehavior
{
    public FlockBehavior[] behaviors;
    public float[] weights;
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if(weights.Length!=behaviors.Length){
            return Vector2.zero;
        }
        Vector2 combineMove = Vector2.zero;
        for(int i = 0; i< behaviors.Length;i++){
            Vector2 newMove = behaviors[i].CalculateMove(agent,context,flock)*weights[i];
            if(newMove!=Vector2.zero){
                if(newMove.sqrMagnitude > weights[i]*weights[i]){
                    newMove.Normalize();
                    newMove = newMove*weights[i];
                }
                combineMove += newMove;
            }
        }
        return combineMove;
    }
}
