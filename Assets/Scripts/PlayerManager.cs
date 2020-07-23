using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance = null;

    private bool hover = false;
    private bool dash = false;
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        
    }

    void test()
    {
        Debug.Log("test");
    }

    public void ObtainHover()
    {
        hover = true;
        Debug.Log("hover feet acquired");
    }

    public void ObtainDash()
    {
        dash = true;
        Debug.Log("dash core acquired");
    }
}
