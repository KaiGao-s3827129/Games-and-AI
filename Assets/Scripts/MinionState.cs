using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Patrol, Die, Attack, Walk, Run, 
}

public class MinionState : MonoBehaviour
{
    private int HP;
    private float distance;
    private int availableTime;
    public State currentState;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 toTarget = GameObject.Find("Neo").transform.position - this.transform.position;
        currentState = State.Patrol;
        HP = 100;
        //distance = Vector2.Distance(transform.position, Neo.transform.position);
        distance = toTarget.magnitude;
        availableTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // if (availableTime > 0)
        // {
        //     availableTime--;
        // }
        Vector2 toTarget = GameObject.Find("Neo").transform.position - this.transform.position;
        distance = toTarget.magnitude;
        switch (currentState) { 
            case State.Patrol:
                if (HP <= 0) {
                    ChangeState(State.Die);
                }
                if (distance <= 1)
                {
                    ChangeState(State.Attack);
                }
                else if (availableTime > 0 || distance <= 30)
                {
                    if (HP <= 10)
                    {
                        ChangeState(State.Run);
                    }
                    else
                    {
                        ChangeState(State.Walk);
                    }
                }
                break;
            case State.Die:
                break;
            case State.Attack:
                if (HP <= 0) {
                    ChangeState(State.Die);
                }
                // if Neo enter invincible state, change to patrol.
                if (distance > 1) {
                    if (HP <= 10)
                    {
                        ChangeState(State.Run);
                    }
                    else
                    {
                        ChangeState(State.Walk);
                    }
                }
                break;
            case State.Walk:
                if (HP <= 0)
                {
                    ChangeState(State.Die);
                }
                if (HP <= 10) {
                    ChangeState(State.Run);
                }
                if (distance > 9 && availableTime <= 0) { 
                    ChangeState (State.Patrol);
                }else if(availableTime>0){
                    availableTime--;
                }
                if (distance <= 1) {
                    ChangeState(State.Attack);
                }
                break;
            case State.Run:
                if (HP <= 0)
                {
                    ChangeState(State.Die);
                }
                if (distance > 9 && availableTime <= 0)
                {
                    ChangeState(State.Patrol);
                }else if(availableTime>0){
                    availableTime--;
                }
                if (distance <= 1)
                {
                    ChangeState(State.Attack);
                }
                break;
        }
    }

    public void ChangeState(State state) {
        currentState = state;
    }

    public void BossBeenAttacked(){
        availableTime = 5;
    }

    public void TakeDamage(int damage){
        HP -= damage;
        Debug.Log(HP);
    }
}
