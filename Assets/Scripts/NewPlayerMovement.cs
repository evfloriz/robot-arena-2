using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer sr;

    public LayerMask groundLayer;


    public float maxSpeed = 5.0f;
    public float acceleration = 40.0f;
    public float inAirAccelerationMultiplier = 1.0f;
    public float maxFallSpeed = 20.0f;
    public float jumpHeight = 3.0f;
    public float peakTime = 0.5f;
        
    

    private float axis = 0.0f;
    private float sign = 0.0f;

    private bool jumpInput = false;
    private float jumpBuffer = 0.2f;
    private float jumpTimer = 0.0f;

    
    private Vector2 movement = new Vector2(0.0f, 0.0f);
    private Vector2 horizontalAcceleration = new Vector2(0.0f, 0.0f);
    private Vector2 horizonalVelocity = new Vector2(0.0f, 0.0f);
    private Vector2 gravity = new Vector2(0.0f, 0.0f);
    private Vector2 aerialVelocity = new Vector2(0.0f, 0.0f);
    
    private bool isGroundedNextFrame = false;
    private bool collisionHorizontal = false;
    private bool collisionUp = false;

    private Vector2 originalExtents;

    
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        animator = GetComponent<Animator>();

        sr = GetComponent<SpriteRenderer>();

        gravity.Set(0.0f, (-2 * jumpHeight) / (peakTime * peakTime));

        originalExtents = sr.bounds.extents;
    }
    
    void Update()
    {

        axis = Input.GetAxisRaw("Horizontal");

        // flip sprite
        if (axis == 1)
            sr.flipX = false;
        else if (axis == -1)
            sr.flipX = true;

        animator.SetFloat("Speed", Mathf.Abs(axis));

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

        if (isGroundedNextFrame)
        {
            if(jumpInput)
            {
                jumpInput = false;
                aerialVelocity.Set(0.0f, 2 * (jumpHeight) / (peakTime));
                animator.SetBool("isJumping", true);
            }
            else
            {
                aerialVelocity.Set(0.0f, 0.0f);
            }

            horizontalAcceleration.Set(acceleration, 0.0f);
            animator.SetBool("isJumping", false);
        }
        else
        {
            horizontalAcceleration.Set(acceleration*inAirAccelerationMultiplier, 0.0f);
        }

        // jump animation
        if (aerialVelocity.y == 0.0f)
            animator.SetBool("isInAir", false);
        else
            animator.SetBool("isInAir", true);

        if (aerialVelocity.y < 0.0f)
            animator.SetBool("isJumping", false);

        // jump calculations
        movement += aerialVelocity * Time.deltaTime + 0.5f * gravity * Time.deltaTime * Time.deltaTime;
        aerialVelocity += gravity * Time.deltaTime;
        
        if (aerialVelocity.y < -maxFallSpeed)
            aerialVelocity.y = -maxFallSpeed;

        

        // run calculations
        // determine correct sign for calculations
        if (axis==0.0f)
        {
            if (horizonalVelocity.x > 0.0f)
                sign = -1.0f;
            else if (horizonalVelocity.x < 0.0f)
                sign = 1.0f;
            else
                sign = 0.0f;
        }
        else
            sign = axis;

        
        movement += horizonalVelocity * Time.deltaTime + sign * 0.5f * horizontalAcceleration * Time.deltaTime * Time.deltaTime;
        horizonalVelocity += sign * horizontalAcceleration * Time.deltaTime;
        
        if (horizonalVelocity.x > maxSpeed)
            horizonalVelocity.x = maxSpeed;
        if (horizonalVelocity.x < -maxSpeed)
            horizonalVelocity.x = -maxSpeed;

        if (axis == 0.0f && ((sign == 1.0f && horizonalVelocity.x > 0.0f) || (sign == -1.0f && horizonalVelocity.x < 0.0f)))
            horizonalVelocity.x = 0.0f;

        
            
        
        // ground check for next frame
        isGroundedNextFrame = RaycastCollision(Vector2.down);
        
        // upward collision
        if (aerialVelocity.y > 0.0f)
        {
            collisionUp = RaycastCollision(Vector2.up);
            if (collisionUp)
            {
                aerialVelocity.y = aerialVelocity.y/3.0f;
                animator.SetBool("isJumping", false);
            }

        }

        // horizontal collision
        if (horizonalVelocity.x >= 0.0f)
            collisionHorizontal = RaycastCollision(Vector2.right);
        
        if (horizonalVelocity.x <= 0.0f)
            collisionHorizontal = RaycastCollision(Vector2.left);

        if (collisionHorizontal)
            horizonalVelocity.x = 0.0f;

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
