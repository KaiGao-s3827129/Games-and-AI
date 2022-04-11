using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockAgent : MonoBehaviour
{
    Collider2D agentCollider;
    private Rigidbody2D rb2d;
    public float slowDownRadius = 5f;
    public Collider2D AgentCollider { get { return agentCollider; } }
    // Start is called before the first frame update
    void Start()
    {
        // rb2d = GetComponent<Rigidbody2D>();
        agentCollider = GetComponent<Collider2D>();
    }


    public void Move(Vector2 velocity)
    {
        // float speed= 0.1f;
        // Vector2 steeringForce = new Vector2(0, 0);
        // Vector2 toTarget = velocity - (Vector2)transform.position;
        // float distance = toTarget.magnitude;
        // if (distance < slowDownRadius)
        // {
        //     Vector2 desiredVelocity = (toTarget).normalized *  speed* (distance / slowDownRadius);
        //     steeringForce = desiredVelocity;
        //     rb2d.AddForce(steeringForce);
        // }
        // else
        // {
        //     Vector2 desiredVelocity = toTarget * speed;
        //     steeringForce = desiredVelocity;
        //     rb2d.AddForce(steeringForce);
        // }
        transform.up = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
