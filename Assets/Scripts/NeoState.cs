using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState
{
    Die, Invincibility, Alive
}

public enum JumpState{
    SingleJump, DoubleJump
}

public enum AttackState{
    Melee, Remote
}

public class NeoState : MonoBehaviour
{
    public int healthPoint = 3;
    public int previousHealthPoint=3;
    public PlayerState currentPlayerState;
    public JumpState currentJumpState;
    public AttackState currentAttackState;
    private int InvincibilityTime;
    // Start is called before the first frame update
    void Start()
    {
        currentPlayerState = PlayerState.Alive;
        currentJumpState = JumpState.SingleJump;
        currentAttackState = AttackState.Melee;
        InvincibilityTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(healthPoint < previousHealthPoint){
            previousHealthPoint = healthPoint;
        }
        switch (currentPlayerState) { 
            case PlayerState.Alive:
                if(healthPoint<=0){
                    ChangePlayerState(PlayerState.Die);
                }
                if(healthPoint<previousHealthPoint){
                    previousHealthPoint = healthPoint;
                    InvincibilityTime = 3;
                     ChangePlayerState(PlayerState.Invincibility);
                }
                break;
            case PlayerState.Die:
                break;
            case PlayerState.Invincibility:
                if(healthPoint<=0){
                    ChangePlayerState(PlayerState.Die);
                }
                if(InvincibilityTime>0){
                    InvincibilityTime--;
                }else if(InvincibilityTime <= 0 && (healthPoint==previousHealthPoint)){
                    ChangePlayerState(PlayerState.Alive);
                }
                break;
        }


        switch(currentJumpState){
            case JumpState.SingleJump:
                if (NeoMovement.isGetSkill)
                {
                    ChangeJumpState(JumpState.DoubleJump);
                }
                break;
            case JumpState.DoubleJump:
                if (!NeoMovement.isGetSkill)
                {
                    ChangeJumpState(JumpState.SingleJump);
                }
                break;
        }


        switch(currentAttackState){
            case AttackState.Melee:
                if (NeoMovement.isGetWeapon)
                {
                    ChangeAttackState(AttackState.Remote);
                }
            
                break;
            case AttackState.Remote:
                if (!NeoMovement.isGetWeapon)
                {
                    ChangeAttackState(AttackState.Melee);
                }

                break;
        }

    }


    public void ChangePlayerState(PlayerState state) {
        currentPlayerState = state;
    }

    public void ChangeJumpState(JumpState state) {
        currentJumpState = state;
    }

    public void ChangeAttackState(AttackState state) {
        currentAttackState = state;
    }
}
