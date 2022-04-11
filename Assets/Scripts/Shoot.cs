using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot: MonoBehaviour
{
    public float speed = 70f;
    public Rigidbody2D rb2d;
    private float horizontalMove;
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        damage = 10;
        rb2d = GetComponent<Rigidbody2D>();
        if(Input.GetKey(KeyCode.W)){
            rb2d.velocity = transform.up*speed*5;
        }else if(Input.GetKey(KeyCode.S)){
            rb2d.velocity = -transform.up *speed*5;
        }
        else if(neoMovement.facingRight){
            rb2d.velocity = transform.right*speed*5;
        }else{
            rb2d.velocity = -transform.right*speed*5;
        }
    }

    void OnTriggerEnter2D(Collider2D hit){
        // Debug.Log(hit.name);
        //Hit Minion through Collider
        string enemy = hit.name;
        if(enemy == "TheBoss"){
            GameObject TheBoss = GameObject.Find(hit.name);
            if(TheBoss !=null){
            BossState bossState = TheBoss.GetComponent<BossState>();
            bossState.TakeDamage(damage);
        }
        }
        else if(enemy.Substring(0,3)=="Min"){
            GameObject minion = GameObject.Find(enemy);
            if(minion !=null){
                MinionState minionState = minion.GetComponent<MinionState>();
                minionState.TakeDamage(damage);
            }
        }else if(enemy.Substring(0,3)=="Flo"){
            
            GameObject flockingMinion = GameObject.Find(enemy);
            if(flockingMinion !=null){
                FlockingMinionState flockingMinionState = flockingMinion.GetComponent<FlockingMinionState>();
                flockingMinionState.TakeDamage(damage);
            }
        }
        
    }

}
