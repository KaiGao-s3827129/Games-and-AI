using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class RandomPlatform : MonoBehaviour
{
    public GameObject platform;
    // public float spawnTime = 2.5f;
    public float yMin, yMax, xMin, xMax;
    public LayerMask layerMask;
    private Transform platformParent;
    private List<Vector2> platformList = new List<Vector2>();

    public GameObject skillBox;
    public GameObject weaponBox;
    public int boxCount = 1;
    public int platformCount = 20;
    
    void Start()
    {
        platformParent = GameObject.Find("Platforms").transform;
        platformSpawn();
        InvokeRepeating("boxSpawn", 5f, 5f);
        // Destroy(skillBox, 2f);
        // Destroy(weaponBox, 2f);

    }

    void platformSpawn(){
        float y;
        float x;
        Vector2 pos;
        GameObject clone = new GameObject("dummy");
        Vector2 platformSize;
        
        for (int i = 0; i < platformCount; i++)
        {   
            do{
                Destroy(clone);
                y = Random.Range(yMin, yMax);
                x = Random.Range(xMin, xMax);
                pos = new Vector2(x, y);
                clone = Instantiate (platform, pos, transform.rotation);
                platformSize = clone.GetComponent<Collider2D>().bounds.size;
                clone.transform.SetParent(platformParent);
                Debug.Log("create a platform on: " + pos);
            }
            while (hasObstacleAtPosition(pos, platformSize, layerMask));
            platformList.Add(pos);
            clone = new GameObject("dummy");
            
        }
    }

    // private void OnCollisionEnter2D(Collision2D collision) {
        
    // }

    bool hasObstacleAtPosition(Vector2 position, Vector2 size, LayerMask layerMask){
        Vector2 checkSize = new Vector2(size.x * 2f, size.y * 3.5f);
        Collider2D[] intersection = Physics2D.OverlapBoxAll(position, checkSize, 0f, layerMask);
        return intersection.Length > 1;
    }

    void boxSpawn()
    {
        float x, y;
        Vector2 boxPos;
        int platformNo;
        List<int> selectedPlatforms = new List<int>();

        for (int i = 0; i < boxCount; i++)
        {
            do
            {
                platformNo = Random.Range(1, platformCount);
            } while (selectedPlatforms.Contains(platformNo));
            selectedPlatforms.Add(platformNo);
            Vector2 platformPos = platformList[platformNo];
            // x = Random.Range(platformPos.x - 5f, platformPos.x + 5f);
            x = Random.Range(platformPos.x - 10f + 4f, platformPos.x + 10f - 4f);
            y = platformPos.y + 4f;
            boxPos = new Vector2(x, y);
            Debug.Log("create a skill box on:" + boxPos +" and platform pos is:" + platformPos );
            GameObject gameObjectSkillBox = Instantiate(skillBox, boxPos, transform.rotation);
            Destroy(gameObjectSkillBox, 3f);
        }

        for (int j = 0; j < boxCount; j++)
        {
            do
            {
                platformNo = Random.Range(1, platformCount);
            } while (selectedPlatforms.Contains(platformNo));
            selectedPlatforms.Add(platformNo);
            Vector2 platformPos = platformList[platformNo];
            // x = Random.Range(platformPos.x - 5f, platformPos.x + 5f);
            x = Random.Range(platformPos.x - 10f + 4f, platformPos.x + 10f - 4f);
            y = platformPos.y + 4f;
            boxPos = new Vector2(x, y);
            Debug.Log("create a skill box on:" + boxPos +" and platform pos is:" + platformPos );
            GameObject gameObjectWeaponBox = Instantiate(weaponBox, boxPos, transform.rotation);
            Destroy(gameObjectWeaponBox, 3f);
        }

    }
    
}
