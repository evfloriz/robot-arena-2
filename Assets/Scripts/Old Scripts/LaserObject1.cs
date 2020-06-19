using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserObject1 : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Camera mainCamera;

    private float length = 1.0f;
    private float speed = 1.0f;
    private int damage = 30;

    private Vector3 backPosition;
    private Vector3 frontPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;

        mainCamera = Camera.main;

        backPosition = transform.position;
        frontPosition = backPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float drawLength = Mathf.Min(length, Mathf.Abs(frontPosition.x - backPosition.x));

        Debug.Log(drawLength);

        // raycast from back position
        RaycastHit2D hit = Physics2D.Raycast(backPosition, transform.right, drawLength);
        Debug.DrawRay(backPosition, drawLength * transform.right, Color.blue);

        
        frontPosition = transform.position;

        if (Mathf.Abs(frontPosition.x - backPosition.x) > length)
            backPosition = frontPosition - length * transform.right;
        
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Enemy")
            {
                EnemyHitBehavior enemy = hit.collider.GetComponent<EnemyHitBehavior>();
                enemy.TakeDamage(damage);
                Debug.Log("hit enemy");
            }
            else if (hit.collider.tag == "Player")
            {
                Debug.Log("hit player");
            }
            
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

    public void Initialize(float _speed, float _length, int _damage)
    {
        speed = _speed;
        length = _length;
        damage = _damage;
    }
}
