using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public float speed = 40f;
    public Rigidbody2D rb2d;
    private float horizontalMove;
    // Start is called before the first frame update
    void Start()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        rb2d = GetComponent<Rigidbody2D>();
        if(horizontalMove>0){
            rb2d.velocity = transform.right*speed;
        }else{
            rb2d.velocity = -transform.right*speed;
        }
    }

    // Update is called once per frame
}
