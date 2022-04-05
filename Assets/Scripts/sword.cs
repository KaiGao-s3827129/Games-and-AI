using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword : MonoBehaviour
{
    public Animator anim;
    public GameObject ant;
    private NeoMovement neoMovement;
    private float speed = 100.0f;
    private float horizontalMove;
    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange;
    public int damage;
    // Start is called before the first frame update
    void Start()
    {

        ant = GameObject.Find("Neo");
        neoMovement = ant.GetComponent<NeoMovement>();
        damage = 20;
        // anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (timeBtwAttack <= 0)
        // {

        //         if (Input.GetKeyDown(KeyCode.K))
        //         {
        //             Collider2D[] Minions = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
        //             for (int i = 0; i < Minions.Length; i++)
        //             {
        //                 Minions[i].GetComponent<MinionState>().TakeDamage(damage);
        //             }
        //         }





        if (neoMovement.facingRight)
        {
            Vector2 newLocation = new Vector2(ant.transform.position.x + 2, ant.transform.position.y + 1);
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, newLocation, step);
        }
        else
        {
            Vector2 newLocation = new Vector2(ant.transform.position.x - 2, ant.transform.position.y + 1);
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, newLocation, step);
        }
        Attack();
    }

    void Attack()
    {
        if (neoMovement.facingRight)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                anim.SetTrigger("Attacked");
            }

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                anim.SetTrigger("AttackLeft");
            }
        }

        Collider2D[] Minions = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
        for (int i = 0; i < Minions.Length; i++)
        {
            Minions[i].GetComponent<MinionState>().TakeDamage(damage);
        }

    }

    void OnDrawGizmosSelected()
    {
        if (attackPos == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

}
