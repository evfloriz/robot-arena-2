using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserObject : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Camera mainCamera;

    private float length = 1.0f;
    private float speed = 1.0f;
    private int damage = 30;
    private string type = "temp";

    private Vector3 backPosition;
    private Vector3 frontPosition;

    private bool hitHasOccurred = false;

    private Color color = Color.white;
    
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;

        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // raycast from back position
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, length);
        Debug.DrawRay(transform.position, length * transform.right, Color.blue);

        backPosition = transform.position;
        frontPosition = transform.position + length * transform.right;
        
        if (hit.collider != null)
        {
            hitHasOccurred = true;

            // enemy collision
            if (hit.collider.tag == "Enemy")
            {
                // make sure enemy didn't fire laser
                if (type != "Enemy")
                {
                    // Debug.Log("hit enemy");
                    EnemyHit enemy = hit.collider.GetComponent<EnemyHit>();
                    enemy.SetHit(true, damage);
                }
                else
                    hitHasOccurred = false;
            }
            // player collision
            else if (hit.collider.tag == "Player")
            {
                // make sure player didn't fire laser
                if (type != "Player")
                {
                    //Debug.Log("hit player");
                    PlayerHitBehavior player = hit.collider.GetComponent<PlayerHitBehavior>();
                    player.TakeDamage(damage);
                }
                else
                    hitHasOccurred = false;

            }

            if (hitHasOccurred)
                Destroy(gameObject);
        }
        else
        {
            // destroy if too far out of camera bounds
            Vector3 viewportPosition = mainCamera.WorldToViewportPoint(backPosition);
            if (viewportPosition.x < -0.5f || viewportPosition.x > 1.5f)
                Destroy(gameObject);
        }

        lineRenderer.SetPosition(0, backPosition);
        lineRenderer.SetPosition(1, frontPosition);

        // Destroy(gameObject);

        // Debug.Log(backPosition);
        // Debug.Log(frontPosition);


        // update back position
        transform.position += speed * Time.deltaTime * transform.right;
    }

    public void Initialize(string _type, float _speed, float _length, int _damage, Color _color)
    {
        type = _type;
        speed = _speed;
        length = _length;
        damage = _damage;
        color = _color;
    }


}
