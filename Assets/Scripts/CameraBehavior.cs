using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraBehavior : MonoBehaviour
{
    private Transform playerTransform;
    private TilemapRenderer background;
    
    private Vector2 bottomLeftBound;
    private Vector2 topRightBound;

    private float verticalCamExtent;
    private float horizontalCamExtent;

    private Vector3 positionToFollow;

    private Camera mainCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        background = GameObject.FindWithTag("Background").GetComponent<TilemapRenderer>();

        bottomLeftBound = background.bounds.center - background.bounds.extents;
        topRightBound = background.bounds.center + background.bounds.extents;

        // Debug.Log(bottomLeftBound);
        // Debug.Log(topRightBound);


        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
            positionToFollow = playerTransform.position;
        
        verticalCamExtent = mainCamera.orthographicSize;
        horizontalCamExtent = mainCamera.orthographicSize * mainCamera.aspect;

        // Debug.Log(verticalCamExtent);
        // Debug.Log(horizontalCamExtent);

        transform.position = new Vector3(
            Mathf.Clamp(positionToFollow.x, bottomLeftBound.x + horizontalCamExtent, topRightBound.x - horizontalCamExtent),
            Mathf.Clamp(positionToFollow.y, bottomLeftBound.y + verticalCamExtent, topRightBound.y - verticalCamExtent),
            transform.position.z);
    }
}
