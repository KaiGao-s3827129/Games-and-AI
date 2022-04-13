using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour
{
    // Start is called before the first frame update
    // public GameObject bomb;
    // private Transform bombsParent;

    public float arrivalDistance;
    public float speedMultiplier;
    private Rigidbody2D rb2d;
    private const float MINIMUM_OFFSET = 0.3f;
    private GameObject target;
    private Vector2 targetPosition;
    public float height;
    public float width; 
    public LayerMask layerMask;
    private MyGrid grid;

    public MyGrid Grid{ get { return grid; } }
    private Vector2 platformSize;
    void Start()
    {
        
        // Collider2D[] intersection = Physics2D.OverlapCircleAll(new Vector2(8.86f, -7.99f), 1f);
        // Debug.Log("collider number is " + intersection.Length);
        // UnityEditor.EditorApplication.isPlaying = false;

        grid = new MyGrid(width, height);
        grid.setLayerMask(layerMask);
        // targetPosition = new Vector2(5f, 8f);
        // Debug.Log("target position is " + targetPosition);
        rb2d = GetComponent<Rigidbody2D>();
        // bombsParent = GameObject.Find("Bombs").transform;

        InvokeRepeating("doAstar", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void doAstar() {
        target = GameObject.Find("Neo");
        targetPosition = (Vector2)target.transform.position;
        Vector2 currPos = gameObject.transform.position;
        // List<(Vector2, float)> neighbors = grid.getNeighbors(currPos);
        // Debug.Log(neighbors.Count + " neighbors found");
        Vector2 nextPosition;
        List<Vector2> posList = AstarCore();
        if (posList.Count == 0){
            // nextPosition = new Vector2(0f, 0f);
            return;
        }else{
            // nextPosition = posList[0];
            nextPosition = PathSmooth(posList, currPos);
            // Debug.DrawLine(currPos, nextPosition, Color.white, 0.1f);
        }
        
        // Debug.Log("next position is " + nextPosition);
        // Vector2 nextPosition = new Vector2(0f, 0f);
        Vector2 offset = nextPosition - currPos;

        // transform.position = Vector2.Lerp(currPos, nextPosition, 1);
        // Vector2.MoveTowards(currPos, nextPosition);

        float moveHorizontal;
        float moveVertical;
        if (offset.magnitude < MINIMUM_OFFSET)
        {
            moveHorizontal = 0.0f;
            moveVertical = 0.0f;
        }else{
            offset.Normalize();
            moveHorizontal = offset.x;
            moveVertical = offset.y;
        }

        Vector2 forceDirection = new Vector2(moveHorizontal, moveVertical);
        float distance = forceDirection.magnitude;
        forceDirection *= speedMultiplier;

        if (distance < arrivalDistance){
            forceDirection = forceDirection * (distance/arrivalDistance);
        }
        forceDirection = forceDirection - GetComponent<Rigidbody2D>().velocity;
        
        
        Debug.DrawLine(currPos, currPos + forceDirection, Color.white, 0.1f);
        // Debug.DrawLine(currPos, nextPosition + forceDirection, Color.white, 0.1f);
        // rb2d.AddForce(forceDirection * speedMultiplier);
        rb2d.AddForce(forceDirection);
        // UnityEditor.EditorApplication.isPlaying = false;
    }

    Vector2 PathSmooth(List<Vector2> oldPath, Vector2 currPos){
        Vector2 nextPos = oldPath[0];
        foreach(Vector2 pos in oldPath){
            Vector2 circleCastDir = pos - currPos;
            RaycastHit2D circlecastHit = Physics2D.CircleCast(currPos, 1.5f, circleCastDir, circleCastDir.magnitude, layerMask);
            if (circlecastHit.collider == null){
                nextPos = pos;
            }else{
                return nextPos;
            }
        }

        return nextPos;
    }

    List<Vector2> AstarCore(){
        PriorityQueue<(Vector2, List<Vector2>, float)> pQ = new PriorityQueue<(Vector2, List<Vector2>, float)>();
        HashSet<Vector2> visited = new HashSet<Vector2>();
        List<Vector2> path = new List<Vector2>();
        Vector2 position = gameObject.transform.position;
        float cost = 0f;
        pQ.Enqueue((position, path, cost),cost);

        int counter = 0;
        // while (!pQ.isEmpty()){
        while (!pQ.isEmpty()){
            (Vector2, List<Vector2>, float) element = pQ.DeQueue();
            position = element.Item1;
            path = element.Item2;
            cost = element.Item3;
            
            // Debug.Log("Distance is " + Vector2.Distance(position, targetPosition));
            if (Vector2.Distance(position, targetPosition) < 1.2f){
                // Debug.Log("return the path");
                return path;
            }

            List<(Vector2, float)> neighbors = grid.getNeighbors(position);
            foreach ((Vector2, float) neighbor in neighbors){
                if (!visited.Contains(neighbor.Item1)){
                    visited.Add(neighbor.Item1);
                    Vector2 neighborPosition = neighbor.Item1;
                    float neighborCost = neighbor.Item2;
                    float heuristicCost = Vector2.Distance(neighborPosition, targetPosition);
                    float currentCost = cost + neighborCost;
                    float totalCost = currentCost + heuristicCost;
                    // float totalCost = currentCost;
                    List<Vector2> newPath = new List<Vector2>(path);
                    newPath.Add(neighborPosition);
                    element = (neighborPosition, newPath, currentCost);
                    pQ.Enqueue(element, totalCost);
                }
            }
            counter ++;
        }
        Debug.Log("return empty");
        return new List<Vector2>();
        
    }
}
