using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public BoxCollider2D bc;
    
    private Vector2 movement = new Vector2(0.0f, 0.0f);

    
    private float axis = 0.0f;
    private float sign = 0.0f;

    public float maxSpeed = 5.0f;
    public float acceleration = 40.0f;
    public float inAirAccelerationMultiplier = 2.0f;
    private Vector2 horizontalAcceleration = new Vector2(0.0f, 0.0f);
    private Vector2 horizonalVelocity = new Vector2(0.0f, 0.0f);
    
    

    private bool jumpInput = false;
    private float jumpBuffer = 0.2f;
    private float jumpTimer = 0.0f;

    public float maxFallSpeed = 20.0f;
    public float jumpHeight = 3.0f;
    public float peakTime = 0.5f;
    private Vector2 gravity = new Vector2(0.0f, 0.0f);
    private Vector2 aerialVelocity = new Vector2(0.0f, 0.0f);

    private float ground_y = 0.0f;
    private bool isColliding = false;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        bc = GetComponent<BoxCollider2D>();

        gravity.Set(0.0f, (-2 * jumpHeight) / (peakTime * peakTime));
    }
    
    void Update()
    {

        axis = Input.GetAxisRaw("Horizontal");

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
        movement.Set(0.0f, 0.0f);

        if(jumpInput)
        {
            jumpInput = false;
            aerialVelocity.Set(0.0f, 2 * (jumpHeight) / (peakTime));
        }
        // else
        // {
        //     aerialVelocity.Set(0.0f, 0.0f);
        // }

        horizontalAcceleration.Set(acceleration, 0.0f);


        // jump calculations
        movement = movement + aerialVelocity * Time.deltaTime + 0.5f * gravity * Time.deltaTime * Time.deltaTime;
        aerialVelocity = aerialVelocity + gravity * Time.deltaTime;
        
        if (aerialVelocity.y < -maxFallSpeed)
            aerialVelocity.y = -maxFallSpeed;

        

        // run calculations
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

        
        movement = movement + horizonalVelocity * Time.deltaTime + sign * 0.5f * horizontalAcceleration * Time.deltaTime * Time.deltaTime;
        horizonalVelocity = horizonalVelocity + sign * horizontalAcceleration * Time.deltaTime;
        
        if (horizonalVelocity.x > maxSpeed)
            horizonalVelocity.x = maxSpeed;
        if (horizonalVelocity.x < -maxSpeed)
            horizonalVelocity.x = -maxSpeed;

        if (axis == 0.0f && ((sign == 1.0f && horizonalVelocity.x > 0.0f) || (sign == -1.0f && horizonalVelocity.x < 0.0f)))
            horizonalVelocity.x = 0.0f;    
        


        //Debug.Log(aerialVelocity);

        if (isColliding)
        {
            movement.y = ground_y - (bc.bounds.center.y-bc.bounds.extents.y);
            isColliding=false;
        }
        
        
        Debug.Log(movement);



        
        rb.MovePosition(rb.position + movement);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        // for (int i=0; i<collision.contactCount; i++)
        //     Debug.Log(collision.GetContact(i).point);

        ground_y = collision.GetContact(0).point.y;
        isColliding = true;
        

        // Debug.Log(ground_y);

        
    
    }

    void OnCollisionStay2D(Collision2D collision)
    {

        isColliding=true;

        Debug.Log(movement);
        

        //Debug.Log(aerialVelocity.y);
    }

    

    
}
