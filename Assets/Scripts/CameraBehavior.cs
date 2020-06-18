using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraBehavior : MonoBehaviour
{
    private Transform playerTransform;
    private TilemapRenderer background;
    private float verticalExtent;
    private float horizontalExtent;

    private Camera mainCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        background = GameObject.FindWithTag("Background").GetComponent<TilemapRenderer>();

        Debug.Log(background.bounds.extents);

        mainCamera = Camera.main;

        verticalExtent = background.bounds.extents.y - mainCamera.orthographicSize;
        horizontalExtent = background.bounds.extents.x - mainCamera.orthographicSize * mainCamera.aspect;

        Debug.Log(verticalExtent);
        Debug.Log(horizontalExtent);
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            Mathf.Clamp(playerTransform.position.x, -horizontalExtent, horizontalExtent),
            Mathf.Clamp(playerTransform.position.y, -verticalExtent, verticalExtent),
            transform.position.z);
    }
}
