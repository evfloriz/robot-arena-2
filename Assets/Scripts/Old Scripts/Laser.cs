using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Camera cam;
    
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
        
    }

    public void Shoot(float beamDrawOffset, float hitDrawOffset)
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
                // Debug.Log("enemy hit");
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

    public void TurnOff()
    {
        lineRenderer.enabled = false;
    }
}
