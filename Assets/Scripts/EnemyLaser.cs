using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    private float shotTimer = 0.0f;
    private float shotDelay = 1.0f;

    public GameObject laserPrefab;
    
    private float laserSpeed = 6.0f;
    private float laserLength = 1.0f;
    private int laserDamage = 1;
    private Color laserColor = new Color(255f/255f, 163f/255f, 0f/255f);
    
    // Start is called before the first frame update
    void Start()
    {

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
        GameObject laser = Instantiate(laserPrefab, transform.position, transform.rotation);
        LaserObject laserObject = laser.GetComponent<LaserObject>();
        laserObject.Initialize(transform.parent.tag, laserSpeed, laserLength, laserDamage, laserColor);
    }
}
