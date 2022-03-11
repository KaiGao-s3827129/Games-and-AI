using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform player;

    public float maxY;
    public float minY;
    public float cameraZ;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float cameraY = Mathf.Clamp(player.position.y, minY, maxY);
        transform.position = new Vector3(player.position.x, cameraY, cameraZ);
    }
}
