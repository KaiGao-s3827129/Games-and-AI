using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeoCollect : MonoBehaviour
{
    private Rigidbody2D neo;

    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        neo = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Platform")
        {
            _animator.SetTrigger("get");
            Destroy(col.gameObject);
        }
    }
}
