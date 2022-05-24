using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Player's melee
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
        damage = 30;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //switch different side through facingRight.
        if (neoMovement.facingRight)
        {
            Vector2 newLocation = new Vector2(ant.transform.position.x, ant.transform.position.y + 1);
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, newLocation, step);
        }
        else
        {
            Vector2 newLocation = new Vector2(ant.transform.position.x, ant.transform.position.y + 1);
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, newLocation, step);
        }
        // Attack();
    }

    public void Attack()
    {
        if (neoMovement.facingRight)
        {
            //Attack right enemy
            anim.SetTrigger("Attacked");
        }
        else
        {
            // if (Input.GetKeyDown(KeyCode.K))
            // {
            //Attack left enemy
            anim.SetTrigger("AttackLeft");
        }
        Collider2D[] Minions = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
        if (Minions.Length == 0){
            ant.GetComponent<NeoAgent>().handleSwordNotAttack();
            return;
        }
        for (int i = 0; i < Minions.Length; i++)
        {
            //Attack different enemy
            if (Minions[i].name.Substring(0, 3) == "Min")
            {
                Minions[i].GetComponent<MinionState>().TakeDamage(damage);
                ant.GetComponent<NeoAgent>().handleSwordAttack();
            }
            if (Minions[i].name == "TheBoss")
            {
                Minions[i].GetComponent<BossState>().TakeDamage(damage);
                ant.GetComponent<NeoAgent>().handleSwordAttackBoss();
            }
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
