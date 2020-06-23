using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private float shotTimer = 0.0f;
    private float shotDelay = 1.0f;

    public GameObject laserPrefab;
    
    private float laserSpeed = 6.0f;
    private float laserLength = 1.0f;
    private int laserDamage = 1;
    private Color laserColor = new Color(255f/255f, 163f/255f, 0f/255f);

    private int contactDamage = 1;

    private Transform laserSpawnTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        laserSpawnTransform = transform.Find("LaserSpawn");
        if (laserSpawnTransform == null)
            laserSpawnTransform = transform;

    }

    // Update is called once per frame
    void Update()
    {
        if (shotTimer > shotDelay)
        {
            Shoot();
            shotTimer = 0.0f;
            // Debug.Log("shooting");
        }
        
        shotTimer += Time.deltaTime;
    }

    void Shoot()
    {
        GameObject laser = Instantiate(laserPrefab, laserSpawnTransform.position, laserSpawnTransform.rotation);
        LaserObject laserObject = laser.GetComponent<LaserObject>();
        laserObject.Initialize(gameObject.tag, laserSpeed, laserLength, laserDamage, laserColor);
    }

    // handles collision with player
    void OnTriggerStay2D(Collider2D other)
    {
        // Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerHitBehavior>().TakeDamage(contactDamage);
        }

    }
}
