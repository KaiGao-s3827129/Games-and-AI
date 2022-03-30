using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform player;

    public float maxY;
    public float minY;
    public float maxX;
    public float minX;
    public float cameraZ;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float cameraY = Mathf.Clamp(player.position.y, minY, maxY);
        float cameraX = Mathf.Clamp(player.position.x, minX, maxX);
        transform.position = new Vector3(cameraX, cameraY, cameraZ);
    }
}