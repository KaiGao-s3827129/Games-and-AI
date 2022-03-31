using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlatform : MonoBehaviour
{
    public GameObject platform;
    // public float spawnTime;
    public float yMin, yMax, xMin, xMax;
    // private List<(float, float)> platformList = new List<(float, float)>();
    // Start is called before the first frame update
    void Start()
    {
        platformSpawn();
    }

    void platformSpawn(){
        float y;
        float x;
        for (int i = 0; i < 10; i++)
        {   
            y = Random.Range(yMin, yMax);
            x = Random.Range(xMin, xMax);
            Vector2 pos = new Vector2(x, y);
            Instantiate (platform, pos, transform.rotation);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
