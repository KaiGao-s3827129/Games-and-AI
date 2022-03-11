using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    public enum InputType { Keyboard, Mouse };

    public InputType inputType;
    public float speedMultiplier;

    public GameObject bomb;
    private Transform bombsParent;

    // Store a reference to the Rigidbody2D component required to use 2D Physics.
    private Rigidbody2D rb2d;
    private SpriteRenderer sr;

    // To store the last position that the player right-clicked.
    private Vector2 lastClickPosition;

    private const float MINIMUM_MOUSE_OFFSET = 0.1f;

    // Use this for initialization
    void Start()
    {
        // Get and store a references to the Rigidbody2D and SpriteRenderer components.
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        bombsParent = GameObject.Find("Bombs").transform;

        // Default the last click position to the position of the bird so that it doesn't move until the player clicks.
        lastClickPosition = gameObject.transform.position;
    }

    // Update is called once per frame.
    void Update()
    {
        // Check whether the player right-clicked
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 clickPosition3d = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lastClickPosition = (Vector2)clickPosition3d;
        }

        // Check if we're launching a bomb
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 spawnPointOffset = new Vector3(0.75f, 0.0f, 0.0f);
            Vector2 initialVelocity = new Vector2(4.0f, 0.0f);

            if (sr.flipX)
            {
                spawnPointOffset *= -1.0f;
                initialVelocity *= -1.0f;
            }

            GameObject newBomb = Instantiate(bomb, transform.position + spawnPointOffset, Quaternion.identity, bombsParent);

            newBomb.GetComponent<Rigidbody2D>().velocity = initialVelocity;
        }
    }

    // FixedUpdate is called at a fixed interval and is independent of frame rate.
    // Put physics code here.
    void FixedUpdate()
    {
        float moveHorizontal = 0.0f;
        float moveVertical = 0.0f;

        switch (inputType)
        {
            case InputType.Keyboard:

                moveHorizontal = Input.GetAxis("Horizontal");
                moveVertical = Input.GetAxis("Vertical");
                break;

            case InputType.Mouse:

                Vector2 currentBirdPosition = gameObject.transform.position;
                Vector2 clickOffset = lastClickPosition - currentBirdPosition;

                // TO DO: Rather than using a minimum mouse offset (which is a bit hacky), a better solution would be
                // to use the "arrive" steering behaviour. Implementing this is left as an exercise :)
                if (clickOffset.magnitude < MINIMUM_MOUSE_OFFSET)
                {
                    moveHorizontal = 0.0f;
                    moveVertical = 0.0f;
                }
                else
                {
                    // Normalise the click offset so that the bird's movement speed is not dependent on how far away the player clicked.
                    clickOffset.Normalize();

                    moveHorizontal = clickOffset.x;
                    moveVertical = clickOffset.y;
                }

                break;

            default:

                Debug.LogError("Error: Unknown movement style!");
                break;
        }

        // Flip the bird to face the horizontal direction chosen.
        if (moveHorizontal > 0)
        {
            sr.flipX = true;
        }
        else if (moveHorizontal < 0)
        {
            sr.flipX = false;
        }

        // Use the two floats to create a vector.
        Vector2 forceDirection = new Vector2(moveHorizontal, moveVertical);

        // Accelerate the player in the chosen direction.
        rb2d.AddForce(forceDirection * speedMultiplier);
    }
}

