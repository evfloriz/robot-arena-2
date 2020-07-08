using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGate : MonoBehaviour
{    
    public int sceneNum;

    private string sceneName;

    void Start()
    {
        sceneName = "Scene " + sceneNum.ToString();

    }
    
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
