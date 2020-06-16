using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Camera cam;

    private float beamDrawOffset = 0.15f;
    private float hitDrawOffset = 0.25f;

    private bool isShooting = false;
    private float shotTimer = 0.0f;
    private float shotTime = 0.1f;
    private float shotDelay = 0.2f;

    private bool shootInput = false;
    private float shootInputTimer = 0.0f;
    private float shootInputBuffer = 0.1f;

    
    
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // shoot input buffer
        if (Input.GetButtonDown("Fire1"))
        {
            shootInput = true;
            shootInputTimer = 0.0f;
        }
        else
        {
            shootInputTimer += Time.deltaTime;
            if (shootInputTimer > shootInputBuffer)
                shootInput = false;
        }
        
        // set shooting to true if input and not currently shooting
        if (shootInput && isShooting == false)
        {
            // Debug.Log("shooting");
            isShooting = true;
        }

        // if shooting, shoot for specified time and wait for specified time before stopping
        if (isShooting)
        {
            if (shotTimer < shotTime)
                Shoot();
            else
                lineRenderer.enabled = false;
            
            if (shotTimer > shotDelay)
            {
                shotTimer = 0.0f;
                isShooting = false;
                // Debug.Log("not shooting");
            }

            shotTimer += Time.deltaTime;

        }   
    }
        

    void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right);

        Vector3 beamOffsetVector = new Vector3(transform.right.x * beamDrawOffset, 0.0f, 0.0f);
        Vector2 hitOffsetVector = new Vector2(0.0f, 0.0f);

        if (hit.collider != null)
        {
            // if ray hits enemy, have beam penetrate a little bit into the bounding box
            if (hit.collider.tag == "Enemy")
            {
                hitOffsetVector.x = transform.right.x * hitDrawOffset;
                Debug.Log("enemy hit");
            }
            
            Debug.DrawLine(transform.position + beamOffsetVector, hit.point, Color.red);
            lineRenderer.SetPosition(0, transform.position + beamOffsetVector);
            lineRenderer.SetPosition(1, hit.point + hitOffsetVector);

            // Debug.Log(hit.collider.name);
            
        }
        else
        {
            float viewportEdge;
            if (transform.right.x == 1.0f)
                viewportEdge = 1.0f;
            else
                viewportEdge = 0.0f;
            
            
            Vector2 screenEdge = cam.ViewportToWorldPoint(new Vector2(viewportEdge, 0.0f));
            screenEdge.y = transform.position.y;

            // Debug.Log(screenEdge);

            // fire laser
            Debug.DrawLine(transform.position + beamOffsetVector, screenEdge, Color.blue);
            lineRenderer.SetPosition(0, transform.position + beamOffsetVector);
            lineRenderer.SetPosition(1, screenEdge);

            // Debug.Log("Miss");

        }

        lineRenderer.enabled = true;
        
    }
}
