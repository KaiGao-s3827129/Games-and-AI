using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public float speed = 100f;
    public Rigidbody2D rb2d;
    private float horizontalMove;
    public GameObject ant;
    private NeoMovement neoMovement;
    // Start is called before the first frame update
    void Start()
    {
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
    }

}
