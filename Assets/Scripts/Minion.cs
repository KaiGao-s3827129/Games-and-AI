using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum State
//{
//    Patrol, Die, Attack, Walk, Run,
//}

public class Minion : MonoBehaviour
{
    public float max_velocity;
    public GameObject Neo;
    public Vector2 velocity;
    private Rigidbody2D rb2d;
    public float slowDownRadius;
    public GameObject ant;
    private MinionState minionState;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        max_velocity = 1;
        
    }

    // Update is called once per frame
    void Update()
    {
        ant = GameObject.Find(gameObject.name);
        minionState = ant.GetComponent<MinionState>();
        Vector2 steeringForce = new Vector2(0, 0);
        Vector2 toTarget = GameObject.Find("Neo").transform.position - this.transform.position;
        float distance = toTarget.magnitude;
        // Debug.Log(minionState.currentState);
        if (minionState.currentState == State.Walk)
        {
            // 替换A* and flocking
            if (distance < slowDownRadius)
            {
                Vector2 desiredVelocity = (toTarget).normalized * max_velocity * (distance / slowDownRadius);
                steeringForce = desiredVelocity - velocity;
                rb2d.AddForce(steeringForce);
            }
            else
            {
                Vector2 desiredVelocity = toTarget * max_velocity;
                steeringForce = desiredVelocity - velocity;
                rb2d.AddForce(steeringForce);
            }
        }
        else if (minionState.currentState == State.Run)
        {
            // 替换A* and flocking
            max_velocity = 1.5f;
            if (distance < slowDownRadius)
            {
                Vector2 desiredVelocity = (toTarget).normalized * max_velocity * (distance / slowDownRadius);
                steeringForce = desiredVelocity - velocity;
                rb2d.AddForce(steeringForce);
            }
            else
            {
                Vector2 desiredVelocity = toTarget * max_velocity;
                steeringForce = desiredVelocity - velocity;
                rb2d.AddForce(steeringForce);
            }
        }
        else if (minionState.currentState == State.Patrol)
        {
            // flocking
        }
        else if (minionState.currentState == State.Die) {
            Destroy(ant);
        }else if(minionState.currentState==State.Attack){
            //可以不写
        }
        
           
    }
}
