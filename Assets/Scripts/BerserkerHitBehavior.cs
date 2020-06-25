using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerHitBehavior : MonoBehaviour
{
    private int health = 3;

    private float invincibleTimer = 0.0f;
    private float invincibleTime = 0.3f;
    private bool isInvincible = false;

    private SpriteRenderer spriteRenderer;
    private EnemyHit enemyHit;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyHit = GetComponent<EnemyHit>();
    }

    // Update is called once per frame
    void Update()
    {
        // handle invincibility
        if (isInvincible)
        {
            invincibleTimer += Time.deltaTime;

            // blink sprite
            if (invincibleTimer % 0.1f < 0.05f)
                spriteRenderer.enabled = false;
            else
                spriteRenderer.enabled = true;
            
            // end invincibility period
            if (invincibleTimer > invincibleTime)
            {
                invincibleTimer = 0.0f;
                isInvincible = false;
                spriteRenderer.enabled = true;
            }
        }

        // handle hit
        if (enemyHit.IsHit())
        {
            TakeDamage(enemyHit.GetDamage());
            enemyHit.SetHit(false);
        }

        // Debug.Log(isInvincible);
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            isInvincible = true;
            health -= damage;
            if (health <= 0)
                Destroy(gameObject);
        }

    }

}
