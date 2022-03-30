using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeoCollect : MonoBehaviour
{
    private Rigidbody2D neo;

    public int jumpAccount;

    public bool isGetWeapon;
    // Start is called before the first frame update
    void Start()
    {
        neo = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "WeaponBox" && !isGetWeapon)
        {
            isGetWeapon = true;
        }

        if (col.tag == "SkillBox" && jumpAccount == 1)
        {
            jumpAccount++;
        }
    }
}
