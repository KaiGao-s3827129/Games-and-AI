using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Flock/Behavior/StreeBehivor")]
public class StreerBehivor : FlockBehavior
{
    Vector2 currentVelocity;
    public float agentSmoothTime = 0.5f;
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        int count = context.Count;
        if(count==0){
            return Vector2.zero;
        }
        Vector2 cohesionMove = Vector2.zero;
        foreach(Transform one in context){
            cohesionMove += (Vector2)one.position;
        }
        cohesionMove=cohesionMove/count;
        cohesionMove -= (Vector2)agent.transform.position;
        cohesionMove = Vector2.SmoothDamp(agent.transform.up,cohesionMove,ref currentVelocity,agentSmoothTime);
        return cohesionMove;
    }
}
