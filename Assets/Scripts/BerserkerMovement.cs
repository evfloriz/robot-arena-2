using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer sr;

    private Transform playerTransform;
    private Vector3 positionToFollow;

    public LayerMask groundLayer;

    public float maxSpeed = 3.0f;
    public float maxFallSpeed = 20.0f;
    public float jumpHeight = 3.0f;
    public float peakTime = 0.5f;

    private Vector2 movement = new Vector2(0.0f, 0.0f);
    private Vector2 velocity = new Vector2(0.0f, 0.0f);
    private Vector2 gravity = new Vector2(0.0f, 0.0f);

    private bool isGroundedNextFrame = false;
    private bool collisionHorizontal = false;
    private bool collisionUp = false;

    private Vector2 originalExtents;

    private bool shouldJump = false;
    private float moveDirection = 0.0f;

    private GameObject laserSpawn;
    private Vector2 originalSpawnTransform;

    private float deadzone = 2.0f;


    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        playerTransform = GameObject.FindWithTag("Player").transform;

        rb.isKinematic = true;
        gravity.y = (-2 * jumpHeight) / (peakTime * peakTime);
        originalExtents = sr.bounds.extents;

        laserSpawn = transform.Find("LaserSpawn").gameObject;
        originalSpawnTransform = laserSpawn.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
            positionToFollow = playerTransform.position;

        float positionDifference = transform.position.x - positionToFollow.x;

        // change direction and flip sprite based on position relative to player
        moveDirection = 0.0f;
        if (positionDifference > 0.0f)
        {
            sr.flipX = true;
            laserSpawn.transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            laserSpawn.transform.localPosition = new Vector2(-originalSpawnTransform.x, originalSpawnTransform.y);
            if (positionDifference > deadzone)
                moveDirection = -1.0f;
            
        }
        else
        {
            sr.flipX = false;
            laserSpawn.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            laserSpawn.transform.localPosition = new Vector2(originalSpawnTransform.x, originalSpawnTransform.y);
            if (positionDifference < -deadzone)
                moveDirection = 1.0f;
        }
            

        animator.SetFloat("Speed", Mathf.Abs(moveDirection));

        
    }

    void FixedUpdate()
    {
        UpdateMovement();

        rb.MovePosition(rb.position + movement);
    }

    void UpdateMovement()
    {
        // constant velocity, no acceleration
        movement.Set(0.0f, 0.0f);

        // set properties that depend on if enemy is grounded
        if (isGroundedNextFrame)
        {
            if (shouldJump)
            {
                shouldJump = false;
                velocity.y = 2 * jumpHeight / peakTime;
            }
            else
                velocity.y = 0.0f;
        }

        // apply movement
        movement += velocity * Time.deltaTime + 0.5f * gravity * Time.deltaTime * Time.deltaTime;
        velocity.x = moveDirection * maxSpeed;
        velocity.y += gravity.y * Time.deltaTime;

        // cap fall speed
        if (velocity.y < -maxFallSpeed)
            velocity.y = -maxFallSpeed;

        
        
        // ground check for next frame
        isGroundedNextFrame = RaycastCollision(Vector2.down);
        
        // upward collision
        if (velocity.y > 0.0f)
        {
            collisionUp = RaycastCollision(Vector2.up);
            if (collisionUp)
            {
                velocity.y = velocity.y/3.0f;
            }
        }

        // horizontal collision
        if (velocity.x >= 0.0f)
            collisionHorizontal = RaycastCollision(Vector2.right);
        
        if (velocity.x <= 0.0f)
            collisionHorizontal = RaycastCollision(Vector2.left);

        if (collisionHorizontal)
        {
            velocity.x = 0.0f;
            shouldJump = true;
        }
        else
            shouldJump = false;


    }

    // raycasts in a specified direction
    bool RaycastCollision(Vector2 direction)
    {
        // sprite extents
        Vector2 extents = originalExtents;

        // offset is the location of start of the first raycast
        // change is to iterate and space out the three raycasts
        float offsetFactor = -0.3f;
        float changeFactor = 0.66f;

        Vector2 offset = new Vector2(0.0f,0.0f);
        Vector2 change = new Vector2(0.0f,0.0f);

        float distance = 0.0f;
        
        // vertical raycasts
        if (direction.x==0.0f)
        {
            offset.y = extents.y * offsetFactor * direction.y;
            offset.x = -extents.x * changeFactor;
            
            change.x = extents.x * changeFactor;
            
            distance = extents.y + Mathf.Abs(offset.y);
        }
        // horizontal raycasts
        else // direction.y==0.0f
        {
            offset.x = extents.x * offsetFactor * direction.x;
            offset.y = -extents.y * changeFactor;

            change.y = extents.y * changeFactor;

            distance = extents.x + Mathf.Abs(offset.x);
        }        

        Vector2 drawDir = distance * direction;
        Vector2 position = rb.position + movement;               // next frame
        
        for (int i=0; i<3; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(position + offset, direction, distance, groundLayer);
            Debug.DrawRay(rb.position+offset, drawDir, Color.blue);
            
            if (hit.collider != null)
            {
                Debug.DrawRay(position+offset, drawDir, Color.red);
                movement = movement - direction * (distance-hit.distance);
                // Debug.Log(distance-hit.distance);
                return true;
            }

            offset = offset + change;
        }

        return false;
    }
}
