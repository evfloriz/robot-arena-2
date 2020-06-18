﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour
{   
    private bool isShooting = false;
    private float shotTimer = 0.0f;
    private float shotDelay = 0.2f;

    private bool shootInput = false;
    private float shootInputTimer = 0.0f;
    private float shootInputBuffer = 0.1f;

    public GameObject laserPrefab;

    private float laserSpeed = 16.0f;
    private float laserLength = 2.0f;
    private int laserDamage = 30;
    private Color laserColor = new Color(255f/255f, 0f/255f, 77f/255f);

    
    
    // Start is called before the first frame update
    void Start()
    {
        
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
        
        // shoot and set shooting to true if input and not currently shooting
        if (shootInput && isShooting == false)
        {
            Shoot();
            isShooting = true;
        }

        // wait a delay before you can shoot again
        if (isShooting)
        {
            shotTimer += Time.deltaTime;
            
            if (shotTimer > shotDelay)
            {
                shotTimer = 0.0f;
                isShooting = false;
            }
        }

        void Shoot()
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, transform.rotation);
            LaserObject laserObject = laser.GetComponent<LaserObject>();
            laserObject.Initialize(transform.parent.tag, laserSpeed, laserLength, laserDamage, laserColor);
        }
    }
        

    
}
