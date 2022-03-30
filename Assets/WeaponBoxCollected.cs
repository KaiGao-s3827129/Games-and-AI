using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBoxCollected : MonoBehaviour
{
    private Rigidbody2D WeaponBox;

    private Animator anim;

    public bool isOpen = false;
    
    // Start is called before the first frame update
    void Start()
    {
        WeaponBox = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.tag == "Player")
        {
            anim.SetTrigger("get");
            Destroy(WeaponBox);
        }
    }
}
