using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlatform : MonoBehaviour
{
    public GameObject platform;
    // public float spawnTime;
    public float yMin, yMax, xMin, xMax;
    public LayerMask layerMask;
    private Transform platformParent;
    // private List<(float, float)> platformList = new List<(float, float)>();
    
    void Start()
    {
        platformParent = GameObject.Find("Platforms").transform;
        platformSpawn();
    }

    void platformSpawn(){
        float y;
        float x;
        Vector2 pos;
        GameObject clone = new GameObject("dummy");
        Vector2 platformSize;
        
        for (int i = 0; i < 21; i++)
        {   
            do{
                Destroy(clone);
                y = Random.Range(yMin, yMax);
                x = Random.Range(xMin, xMax);
                pos = new Vector2(x, y);
                clone = Instantiate (platform, pos, transform.rotation);
                platformSize = clone.GetComponent<Collider2D>().bounds.size;
                clone.transform.SetParent(platformParent);
            }
            while (hasObstacleAtPosition(pos, platformSize, layerMask));
            clone = new GameObject("dummy");
            
        }
    }

    // private void OnCollisionEnter2D(Collision2D collision) {
        
    // }

    bool hasObstacleAtPosition(Vector2 position, Vector2 size, LayerMask layerMask){
        Vector2 checkSize = new Vector2(size.x * 2, size.y * 3);
        Collider2D[] intersection = Physics2D.OverlapBoxAll(position, checkSize, 0f, layerMask);
        return intersection.Length > 1;
    }
}
