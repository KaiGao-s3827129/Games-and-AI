using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Player state
public enum PlayerState
{
    Die, Invincibility, Alive
}
//Player Jump state
public enum JumpState{
    SingleJump, DoubleJump
}
//Player attack state(melee or shoot)
public enum AttackState{
    Melee, Remote
}

public class NeoState : MonoBehaviour
{
    //Neo's health points
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
    void FixedUpdate()
    {
        //change state according to different condition
        switch (currentPlayerState) { 
            case PlayerState.Alive:
                if(healthPoint<=0){
                    ChangePlayerState(PlayerState.Die);
                }
                if(healthPoint<previousHealthPoint){
                    previousHealthPoint = healthPoint;
                    InvincibilityTime = 100;
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
        //switch jump state
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

        //switch attack state
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

    public void TakeDamage(int damage){
        healthPoint -= damage;
    }
}
