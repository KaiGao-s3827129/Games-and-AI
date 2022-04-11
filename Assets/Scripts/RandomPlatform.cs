using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class RandomPlatform : MonoBehaviour
{
    //Random create platfrom
    public GameObject platform;
    // public float spawnTime = 2.5f;
    public float yMin, yMax, xMin, xMax;
    public LayerMask layerMask;
    private Transform platformParent;
    private List<Vector2> platformList = new List<Vector2>();
    public GameObject skillBox;
    public GameObject weaponBox;
    public int boxCount = 2;
    public int platformCount = 20;
    //Create Minion
    private FlockingMinionState flockingMinionState;
    public int startingCount = 5;
    public FlockingMinionMovement agentPrefab;
    public Minion minionAgentPrefab;
    public float AgentDensity = 5f;

    public int leaderMinionNumber;
    public int followMinionNumber;
    public List<string> leaderMinions;

    void Start()
    {
        leaderMinions = new List<string>();
        platformParent = GameObject.Find("Platforms").transform;
        leaderMinionNumber = 0;
        platformSpawn();
        InvokeRepeating("boxSpawn", 5f, 10f);
        Destroy(skillBox, 2f);
        Destroy(weaponBox, 2f); 
        InvokeRepeating("createMinion", 0, 10f);
    }

    void platformSpawn()
    {
        float y;
        float x;
        Vector2 pos;
        GameObject clone = new GameObject("dummy");
        Vector2 platformSize;

        for (int i = 0; i < platformCount; i++)
        {
            do
            {
                Destroy(clone);
                y = Random.Range(yMin, yMax);
                x = Random.Range(xMin, xMax);
                pos = new Vector2(x, y);
                clone = Instantiate(platform, pos, transform.rotation);
                platformSize = clone.GetComponent<Collider2D>().bounds.size;
                clone.transform.SetParent(platformParent);
                Debug.Log("create a platform on: " + pos);
            }
            while (hasObstacleAtPosition(pos, platformSize, layerMask));
            platformList.Add(pos);
            clone = new GameObject("dummy");

        }
    }

    bool hasObstacleAtPosition(Vector2 position, Vector2 size, LayerMask layerMask)
    {
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
            Debug.Log("create a skill box on:" + boxPos + " and platform pos is:" + platformPos);
            Instantiate(skillBox, boxPos, transform.rotation);
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
            Debug.Log("create a skill box on:" + boxPos + " and platform pos is:" + platformPos);
            Instantiate(weaponBox, boxPos, transform.rotation);
        }

    }

    void createMinion()
    {
        if(leaderMinions.Count<=1){
            float x, y;
            Vector2 boxPos;
            int platformNo;

            List<int> selectedPlatforms = new List<int>();
            do
            {
                platformNo = Random.Range(1, platformCount);
            } while (selectedPlatforms.Contains(platformNo));
            selectedPlatforms.Add(platformNo);
            Vector2 platformPos = platformList[platformNo];
            x = Random.Range(platformPos.x - 10f + 4f, platformPos.x + 10f - 4f);
            y = platformPos.y + 5f;
            boxPos = new Vector2(x, y);
            leaderMinionNumber++;
            leaderMinions.Add("Minion" + leaderMinionNumber);
            Minion minAgent = Instantiate(
            minionAgentPrefab,
            boxPos,
                    Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                    transform
            );
            minAgent.name = "Minion" + leaderMinionNumber;
            boxPos.y +=1;
            for (int i = followMinionNumber; i < followMinionNumber+2; i++)
            {
                boxPos.x+=5f;
                FlockingMinionMovement newAgent = Instantiate(
                    agentPrefab,
                    boxPos,
                    Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                    transform
                    );
                newAgent.name = "Flocking " + i;
                
            }
            followMinionNumber +=2;
            boxPos.x -= 10f;
            for (int i = followMinionNumber; i < followMinionNumber+2; i++)
            {
                boxPos.x-=5f;
                FlockingMinionMovement newAgent = Instantiate(
                    agentPrefab,
                    boxPos,
                    Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                    transform
                    );
                newAgent.name = "Flocking " + i;
                
            }
            followMinionNumber +=2;
            boxPos.x+=10f;
            boxPos.y+=5f;
            FlockingMinionMovement newUpAgent = Instantiate(
                    agentPrefab,
                    boxPos,
                    Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                    transform
                    );
                newUpAgent.name = "Flocking " + followMinionNumber;
            followMinionNumber++;
        }
            
    }
}
