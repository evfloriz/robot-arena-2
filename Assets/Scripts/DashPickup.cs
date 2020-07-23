using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPickup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {   
            PlayerManager playerManager = PlayerManager.instance.GetComponent<PlayerManager>();
            
            if (playerManager != null)
            {
                playerManager.ObtainDash();
            }

            Destroy(gameObject);
        }

    }
}
