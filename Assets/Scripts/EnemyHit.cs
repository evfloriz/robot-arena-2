using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// generic script that can be attached to any enemy
// called by Laser prefab
// allows hit behavior to be unique for each enemy

public class EnemyHit : MonoBehaviour
{
    private bool isHit = false;
    private int damage = 0;
    
    public void SetHit(bool _isHit, int _damage = 0)
    {
        isHit = _isHit;
        damage = _damage;
    }

    public bool IsHit()
    {
        return isHit;
    }

    public int GetDamage()
    {
        return damage;
    }
}
