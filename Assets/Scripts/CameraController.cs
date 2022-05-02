using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] float maxDistance = 5;
    [SerializeField] float minSpeed = 5;
    [SerializeField] [Range(0f, 0.5f)] float slowCamBound = 0.35f; 

    private float cameraSpeed = 0;
    private Transform playerTransform;
    private PlayerMovementController playerRb;
    private Camera cam;
    private Vector3 startPos;

    void Start()
    {
        cam = GetComponent<Camera>();
        startPos = transform.position;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerRb = playerTransform.GetComponent<PlayerMovementController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var relativeScreenPos = cam.WorldToViewportPoint(playerTransform.position + new Vector3(0.5f, 0, 0));
        if (playerRb.CurrentAcceleration > 0.1f)
        {
            if (relativeScreenPos.x >= 0.5f)
            {
               
                transform.position = new Vector3(playerTransform.position.x + 0.5f, startPos.y, startPos.z);
            }
            
            else
                cameraSpeed = 0;

        }


       
    }

}
