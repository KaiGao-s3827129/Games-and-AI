using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot: MonoBehaviour
{
    public float speed = 100f;
    public Rigidbody2D rb2d;
    private float horizontalMove;
    public GameObject ant;
    private NeoMovement neoMovement;
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        damage = 1;
        // horizontalMove = Input.GetAxisRaw("Horizontal");
        rb2d = GetComponent<Rigidbody2D>();
        // rb2d.velocity = transform.right*speed*5;
        ant = GameObject.Find("Neo");
        neoMovement = ant.GetComponent<NeoMovement>();
        if(neoMovement.facingRight){
            rb2d.velocity = transform.right*speed*5;
        }else{
            rb2d.velocity = -transform.right*speed*5;
        }
        // Destroy (gameObject, 10);
    }

    void OnTriggerEnter2D(Collider2D hit){
        Debug.Log(hit.name);
        string enemy = hit.name;
        if(enemy == "TheBoss"){
            GameObject TheBoss = GameObject.Find("TheBoss");
            if(TheBoss !=null){
            BossState bossState = TheBoss.GetComponent<BossState>();
            bossState.TakeDamage(damage);
        }
        }
        else if(enemy=="Minion"){
            GameObject minion = GameObject.Find("Minion");
            if(minion !=null){
                MinionState minionState = minion.GetComponent<MinionState>();
                minionState.TakeDamage(damage);
            }
        }else if(enemy=="FlockingMinion"){
            GameObject flockingMinion = GameObject.Find("FlockingMinion");
            if(flockingMinion !=null){
                FlockingMinionState flockingMinionState = flockingMinion.GetComponent<FlockingMinionState>();
                flockingMinionState.TakeDamage(damage);
            }
        }


    }

}
