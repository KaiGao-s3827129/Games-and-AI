using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrid
{
    private float width;
    private float height;
    private int obstacleLayerMask;
    public MyGrid(float width, float height){
        this.width = width;
        this.height = height;
    }
    
    public float getWidth(){
        return width;
    }

    public float getHeight(){
        return height;
    }

    public void setLayerMask(LayerMask obstacleLayerMask){
        this.obstacleLayerMask = obstacleLayerMask;
    }

    public List<(Vector2, float)> getNeighbors(Vector2 currPos){
        List<(Vector2, float)> allNeighbors = new List<(Vector2, float)>();
        allNeighbors.Add((new Vector2(currPos.x-2f, currPos.y+2f), 2.8f)); 
        allNeighbors.Add((new Vector2(currPos.x, currPos.y+2f), 2f)); 
        allNeighbors.Add((new Vector2(currPos.x+2f, currPos.y+2f), 2.8f)); 
        allNeighbors.Add((new Vector2(currPos.x-2f, currPos.y), 2f)); 
        allNeighbors.Add((new Vector2(currPos.x+2f, currPos.y), 2f)); 
        allNeighbors.Add((new Vector2(currPos.x-2f, currPos.y-2f), 2.8f)); 
        allNeighbors.Add((new Vector2(currPos.x, currPos.y-2f), 2f)); 
        allNeighbors.Add((new Vector2(currPos.x+2f, currPos.y-2f), 2.8f)); 

        List<(Vector2, float)> validNeighbors = new List<(Vector2, float)>();
        foreach ((Vector2, float) pos in allNeighbors){
            if (pos.Item1.x < -width || pos.Item1.x > width) {
                continue;
            }
            if (pos.Item1.y < -height || pos.Item1.y > height) {
                continue;
            }
            if (hasObstacleAtPosition(pos.Item1)){
                continue;
            }
            validNeighbors.Add(pos);
        }
        return validNeighbors;
    }

    public bool hasObstacleAtPosition(Vector2 position){
        // Collider[] intersection = Physics.OverlapSphere(position, 0.02f, obstacleLayerMask);
        Collider2D[] intersection = Physics2D.OverlapCircleAll(position, 0.8f, (int)obstacleLayerMask);
        // Debug.Log("layermask is " + obstacleLayerMask);
        // Collider2D[] intersection = Physics2D.OverlapCircleAll(position, 2f, obstacleLayerMask);
        if (intersection.Length == 0){
            // Debug.Log("no collision");
            return false;
        }else{
            return true;
        }
        // return !(intersection.Length == 0);
    }
}
