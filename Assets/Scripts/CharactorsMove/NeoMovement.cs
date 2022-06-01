using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeoMovement : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    private Rigidbody2D neo;
    private Animator anim;
    public GameObject loseMenu;
    
    public AudioSource getAudio;
    
    public LayerMask mask;
    public float boxHeight = 0.05f;
    public float jumpValue = 150f;
    public float speed = 20f;
    public float fallMulti = 20f;
    public float lowJumpMulti = 25f;
    public int jumpCount = 1;
    public bool isJump = false;
    public static bool isGetWeapon = false;
    public static bool isGetSkill = false;
    private Vector2 playerSize;
    private Vector2 boxSize;
    private float horizontalMove;
    public bool jumpRequest = false;
    public bool isGround = false;
    public GameObject ant;
    private NeoState neoState;
    public bool facingRight = true;
    
    // Start is called before the first frame update
    void Start()
    {
        ant = GameObject.Find("Neo");
        neo = GetComponent<Rigidbody2D>();
        playerSize = GetComponent<SpriteRenderer>().bounds.size;
        boxSize = new Vector2(playerSize.x * 0.0f, boxHeight);
        anim = GetComponent<Animator>();
        getAudio = GetComponent<AudioSource>();
        // bulletPrefab = GameObject.Find("bulletPrefab");
        

    }

    // Update is called once per frame
    void Update()
    {
        neoState = ant.GetComponent<NeoState>();
        //movement
        Run();
        if (Input.GetButtonDown("Jump") && (isGround || jumpCount >= 1) )
        {
            jumpRequest = true;
        }

        if (jumpCount <= 0)
        {
            isGetSkill = false;
        }
        if (isGround && jumpCount <= 0)
        {
            jumpCount = 1;
        }

        if (isGround && isGetSkill)
        {
            jumpCount = 2;
        }
        SwitchAnim();
        
        // die
        if (neoState.currentPlayerState == PlayerState.Die)
        {
            gameObject.SetActive(false);
            Destroy(GameObject.Find("sword"));
            loseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void FixedUpdate()
    {
        // Jump function
        if (jumpRequest)
        {
            neo.AddForce(Vector2.up * jumpValue, ForceMode2D.Impulse);
            SoundManage.instance.JumpAudioPlay();
            jumpRequest = false;
            isGround = false;
            jumpCount -= 1;
        }
        else
        {
            Vector2 boxCenter = (Vector2) transform.position + (Vector2.down * playerSize.y * 0.5f);

            if (Physics2D.OverlapBox(boxCenter, boxSize, 0, mask) != null)
            {
                isGround = true;
                isJump = false;
            }
            else
            {
                isGround = false;
                isJump = true;
            }
        }
        
        if (neo.velocity.y < 0)
        {
            neo.gravityScale = fallMulti;
            if (Input.GetButtonDown("Jump") && isGetSkill)
            {
                neo.AddForce(Vector2.up * jumpValue, ForceMode2D.Impulse);
            }
        } else if (neo.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            neo.gravityScale = lowJumpMulti;
        }
        else
        {
            neo.gravityScale = 10f;
        }
        //Click J to shoot.
        // if(neoState.currentAttackState==AttackState.Remote){
        //     if(Input.GetKey(KeyCode.J)){
        //         Shoot();
        //     }
        // }
    }
    
    private void Run()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        neo.velocity = new Vector2(horizontalMove * speed, neo.velocity.y);
        
        if (horizontalMove != 0)
        {
            if(horizontalMove>0){
                facingRight = true;
            }else{
                facingRight = false;
            }
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }
    }
    
    void SwitchAnim()
    {
        anim.SetFloat("running", Mathf.Abs(neo.velocity.x));

        if (isGround)
        {
            anim.SetBool("falling", false);
            anim.SetBool("jumping", false);
        }
        else if (!isGround && neo.velocity.y > 0)
        {
            anim.SetBool("jumping", true);
            anim.SetBool("falling", false);
        }
        else if (neo.velocity.y < 0)
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //Get WeaponBox can shoot
        if (col.tag == "WeaponBox" && !isGetWeapon)
        {
            isGetWeapon = true;
            SoundManage.instance.GetAudioPlay();
            col.GetComponent<Animator>().SetTrigger("get");
            
            Destroy(col.gameObject, 1.5f);
        }
        
        if (col.tag == "SkillBox" && !isGetSkill)
        {
            jumpCount++;
            isGetSkill = true;
            SoundManage.instance.GetAudioPlay();
            col.GetComponent<Animator>().SetTrigger("get");
            Destroy(col.gameObject, 1.5f);
        }
        
    }


    private void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.name.Substring(0,3)=="Min")
        {
            //Neo has been damaged by Leader Minion
            if(neoState.currentPlayerState!=PlayerState.Invincibility){
                neoState.TakeDamage(1);
            }
            if (neoState.currentPlayerState==PlayerState.Die)
            {
                gameObject.SetActive(false);
                Destroy(GameObject.Find("sword"));
            }
            SoundManage.instance.HurtAudioPlay();
           
            
        }
        //Neo damaged by Boss.
        if(col.gameObject.name=="TheBoss"){
            if(neoState.currentPlayerState!=PlayerState.Invincibility){
                neoState.TakeDamage(1);
            }
            
            if (neoState.currentPlayerState==PlayerState.Die)
            {
                gameObject.SetActive(false);
                Destroy(GameObject.Find("sword"));
            }
        }
        //Neo damaged by following minion.
        if(col.gameObject.name.Substring(0,3)=="Flo"){
            if(neoState.currentPlayerState!=PlayerState.Invincibility){
                neoState.TakeDamage(1);
            }
            if (neoState.currentPlayerState==PlayerState.Die)
            {
                gameObject.SetActive(false);
                Destroy(GameObject.Find("sword"));
            }
        }
    }
    //Shoot function
    void Shoot(){
        GameObject bullet = Instantiate(bulletPrefab,firePoint.position,firePoint.rotation);
        Destroy(bullet,3f);
    }

}
