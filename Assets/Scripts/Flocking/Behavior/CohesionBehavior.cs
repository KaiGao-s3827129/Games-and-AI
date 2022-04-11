using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Flock/Behavior/Cohesion")]
public class CohesionBehavior : FlockBehavior
{
    // Start is called before the first frame update
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
        return cohesionMove;
    }
}
