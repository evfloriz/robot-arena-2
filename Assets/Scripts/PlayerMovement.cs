using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer sr;

    public LayerMask groundLayer;

    private GameObject laserSpawn;

    public float maxSpeed = 5.0f;
    public float horizontalAcceleration = 80.0f;
    public float horizontalFriction = 80.0f;
    public float inAirAccelerationMultiplier = 1.0f;
    public float maxFallSpeed = 20.0f;
    public float jumpHeight = 3.0f;
    public float peakTime = 0.5f;

    private float inputAxis = 0.0f;

    private bool jumpInput = false;
    private float jumpBuffer = 0.2f;
    private float jumpTimer = 0.0f;

    
    private Vector2 movement = new Vector2(0.0f, 0.0f);
    private Vector2 velocity = new Vector2(0.0f, 0.0f);
    private Vector2 acceleration = new Vector2(0.0f, 0.0f);
    
    private bool isGroundedNextFrame = false;
    private bool collisionHorizontal = false;
    private bool collisionUp = false;

    private Vector2 originalExtents;
    private Vector2 originalSpawnTransform;

    
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        rb.isKinematic = true;
        acceleration.y = (-2 * jumpHeight) / (peakTime * peakTime);
        originalExtents = sr.bounds.extents;

        laserSpawn = transform.Find("LaserSpawn").gameObject;
        originalSpawnTransform = laserSpawn.transform.localPosition;
    }
    
    void Update()
    {

        inputAxis = Input.GetAxisRaw("Horizontal");

        // flip sprite
        if (inputAxis == 1)
        {
            sr.flipX = false;
            laserSpawn.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            laserSpawn.transform.localPosition = new Vector2(originalSpawnTransform.x, originalSpawnTransform.y);

        }
        else if (inputAxis == -1)
        {
            sr.flipX = true;
            laserSpawn.transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            laserSpawn.transform.localPosition = new Vector2(-originalSpawnTransform.x, originalSpawnTransform.y);
        }

        animator.SetFloat("Speed", Mathf.Abs(inputAxis));

        if(Input.GetButtonDown("Jump"))
        {
            jumpInput = true;
            jumpTimer = 0;
        }

        if (jumpInput)
        {
            jumpTimer = jumpTimer + Time.deltaTime;
            if (jumpTimer>jumpBuffer)
                jumpInput=false;
        
        }

        

    }
    
    void FixedUpdate()
    {
        UpdateMovement();

        rb.MovePosition(rb.position + movement);        
    }

    void UpdateMovement()
    {
        movement.Set(0.0f, 0.0f);

        // set properties that depend on if player is grounded
        if (isGroundedNextFrame)
        {
            if(jumpInput)
            {
                jumpInput = false;
                velocity.y = 2 * jumpHeight / peakTime;
                
                animator.SetBool("isJumping", true);
            }
            else
            {
                velocity.y = 0.0f;
            }

            acceleration.x = horizontalAcceleration;
            
            animator.SetBool("isInAir", false);
        }
        else
        {
            acceleration.x = horizontalAcceleration*inAirAccelerationMultiplier;
            
            animator.SetBool("isInAir", true);
            if (velocity.y < 0.0f)
                animator.SetBool("isJumping", false);
        }
            

        // determine the correct acceleration.x value
        if (inputAxis==0.0f)
        {
            if (velocity.x > 0.0f)
                acceleration.x = -horizontalFriction;
            else if (velocity.x < 0.0f)
                acceleration.x = horizontalFriction;
            else
                acceleration.x = 0.0f;
        }
        else
            acceleration.x = inputAxis * horizontalAcceleration;
        
        
        // apply movement
        movement += velocity * Time.deltaTime + 0.5f * acceleration * Time.deltaTime * Time.deltaTime;
        // Debug.Log(movement.ToString("F8"));
        
        // calculate velocity for next frame
        velocity += acceleration * Time.deltaTime;
        
        // cap move speed
        if (velocity.x > maxSpeed)
            velocity.x = maxSpeed;
        if (velocity.x < -maxSpeed)
            velocity.x = -maxSpeed;

        // set move speed and acceleration to zero once the
        // player has come to a stop (prevents going backward)
        if (inputAxis == 0.0f && ((acceleration.x > 0.0f && velocity.x > 0.0f) || (acceleration.x < 0.0f && velocity.x < 0.0f)))
        {
            velocity.x = 0.0f;
            acceleration.x = 0.0f;
        }
            
        // cap fall speed
        if (velocity.y < -maxFallSpeed)
            velocity.y = -maxFallSpeed;
                
        // ground check for next frame
        isGroundedNextFrame = RaycastCollision(Vector2.down);

        // stop jumping animation if grounded next frame
        if (isGroundedNextFrame)
            animator.SetBool("isJumping", false);
        
        // upward collision
        if (velocity.y > 0.0f)
        {
            collisionUp = RaycastCollision(Vector2.up);
            if (collisionUp)
            {
                velocity.y = velocity.y/3.0f;
                animator.SetBool("isJumping", false);
            }
        }

        // horizontal collision
        if (velocity.x >= 0.0f)
            collisionHorizontal = RaycastCollision(Vector2.right);
        
        if (velocity.x <= 0.0f)
            collisionHorizontal = RaycastCollision(Vector2.left);

        if (collisionHorizontal)
            velocity.x = 0.0f;

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
