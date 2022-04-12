using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Leader Minion states
public enum State
{
    Patrol, Die, Attack, Walk, Run, 
}

public class MinionState : MonoBehaviour
{
    //Leader Minion's health points
    private int HP;
    private float distance;
    private int availableTime;
    public State currentState;
    public GameObject ant;
    private NeoState neoState;
    
    // Start is called before the first frame update
    void Start()
    {
        Vector2 toTarget = GameObject.Find("Neo").transform.position - this.transform.position;
        currentState = State.Patrol;
        HP = 100;
        distance = toTarget.magnitude;
        availableTime = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ant = GameObject.Find("Neo");
        neoState = ant.GetComponent<NeoState>();
        // Debug.Log(availableTime);
        Vector2 toTarget = GameObject.Find("Neo").transform.position - this.transform.position;
        distance = toTarget.magnitude;
        switch (currentState) { 
            case State.Patrol:
                if (HP <= 0) {
                    ChangeState(State.Die);
                }
                if (neoState.currentPlayerState!=PlayerState.Invincibility && distance <= 1)
                {
                    ChangeState(State.Attack);
                }
                // Get neo's loaciton, enter Run or Walk state.
                else if (neoState.currentPlayerState!=PlayerState.Invincibility&&(availableTime > 0 || distance <= 30))
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
                if(neoState.currentPlayerState==PlayerState.Invincibility){
                    ChangeState(State.Patrol);
                }
                else if (distance > 1) {
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
                //If Neo's location not available, change to patrol state
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
                //If Neo's location not available, change to patrol state
                if (distance > 9 && availableTime <= 0)
                {
                    ChangeState(State.Patrol);
                }else if(availableTime>0){
                    availableTime--;
                }
                //If near by neo enter attack state.
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

    //If boss has been attacked, update available Time
    public void BossBeenAttacked(){
        availableTime = 10;
    }

    public void TakeDamage(int damage){
        HP -= damage;
    }
}
