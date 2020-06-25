using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBehavior : MonoBehaviour
{
    private int health = 3;

    private float invincibleTimer = 0.0f;
    private float invincibleTime = 0.3f;
    private bool isInvincible = false;

    private SpriteRenderer spriteRenderer;
    private GenericHit hit;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hit = GetComponent<GenericHit>();
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
        if (hit.IsHit())
        {
            TakeDamage(hit.GetDamage());
            hit.SetHit(false);
        }

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
