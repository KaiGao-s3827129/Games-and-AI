using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FlockingState
{
    Patrol, Die
}
public class FlockingMinionState : MonoBehaviour
{
    //Flocking Minion's health Points
    private int HP;
    public FlockingState currentState;
    // Start is called before the first frame update
    void Start()
    {
        HP=50;
        currentState = FlockingState.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        //Switch different state for Following Minion.
        switch (currentState) { 
            case FlockingState.Patrol:
                if(HP<=0){
                    ChangeState(FlockingState.Die);
                }
                break;
            case FlockingState.Die:
                break;
        }
    }

    public void ChangeState(FlockingState state) {
        currentState = state;
    }

    //Following Minion take damage
    public void TakeDamage(int damage){
        HP -= damage;
    }
}
