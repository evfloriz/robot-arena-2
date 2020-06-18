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
    
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;

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

            if (hit.collider.tag == "Enemy")
            {
                if (type != "Enemy")
                {
                    EnemyHitBehavior enemy = hit.collider.GetComponent<EnemyHitBehavior>();
                    enemy.TakeDamage(damage);
                    Debug.Log("hit enemy");
                }
                else
                    hitHasOccurred = false;
            }
            else if (hit.collider.tag == "Player")
            {
                if (type != "Player")
                {
                    Debug.Log("hit player");
                }
                else
                    hitHasOccurred = false;

            }

            if (hitHasOccurred)
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

    public void Initialize(string _type, float _speed, float _length, int _damage)
    {
        type = _type;
        speed = _speed;
        length = _length;
        damage = _damage;
    }


}
