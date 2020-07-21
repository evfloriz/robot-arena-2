using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance = null;

    private bool hoverFeet = false;
    
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

    public void ObtainHoverFeet()
    {
        hoverFeet = true;
        Debug.Log(hoverFeet);
    }
}
