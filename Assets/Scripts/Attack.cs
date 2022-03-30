using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private Rigidbody2D neo;
    private Animator anim;

    private bool getWeapon;
    // Start is called before the first frame update
    void Start()
    {
        neo = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ToAttack();
    }

    private void ToAttack()
    {
        if (getWeapon)
        {
            anim.SetTrigger("Attack");
        }
    }
}
