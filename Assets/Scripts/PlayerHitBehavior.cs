using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHitBehavior : MonoBehaviour
{
    private int maxHealth = 3;
    private int health;

    private int lives = 3;

    private float invincibleTimer = 0.0f;
    private float invincibleTime = 0.5f;
    private bool isInvincible = false;

    private SpriteRenderer spriteRenderer;

    public Image[] hearts;
    public Image livesTextImage;

    public Sprite[] numbers;

    private GenericHit hit;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = maxHealth;

        hit = GetComponent<GenericHit>();
    }

    // Update is called once per frame
    void Update()
    {
        // handle game over
        if (lives == 0)
            Destroy(gameObject);
        
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

        
        // display canvas information
        DisplayHearts();
        DisplayLives();
        
        // Debug.Log(isInvincible);
    }

    void DisplayHearts()
    {
        for (int i=0; i<maxHealth; i++)
        {
            if (i<health)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;
        }
    }

    void DisplayLives()
    {
        if (lives > 0)
            livesTextImage.sprite = numbers[lives-1];
        else
            Debug.Log("Lives out of range");
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            isInvincible = true;
            health -= damage;
            // Debug.Log(health);
            if (health <= 0)
            {
                lives--;
                health = maxHealth;
            }
        }

    }

}
