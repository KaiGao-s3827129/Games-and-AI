using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bomb;
    private Transform bombsParent;
    public float speedMultiplier;
    private Rigidbody2D rb2d;
    private const float MINIMUM_MOUSE_OFFSET = 0.4f;
    public GameObject target;
    private Vector2 targetPosition;
    public float height;
    public float width; 
    public LayerMask layerMask;
    private MyGrid grid;
    void Start()
    {
        // Collider2D[] intersection = Physics2D.OverlapCircleAll(new Vector2(8.86f, -7.99f), 1f);
        // Debug.Log("collider number is " + intersection.Length);
        // UnityEditor.EditorApplication.isPlaying = false;

        grid = new MyGrid(width, height);
        grid.setLayerMask(layerMask);
        targetPosition = (Vector2)target.transform.position;
        // targetPosition = new Vector2(5f, 8f);
        // Debug.Log("target position is " + targetPosition);
        rb2d = GetComponent<Rigidbody2D>();
        // bombsParent = GameObject.Find("Bombs").transform;
    }

    // Update is called once per frame
    void Update()
    {
        ri
    }

    void FixedUpdate() {
        Vector2 currPos = gameObject.transform.position;
        // List<(Vector2, float)> neighbors = grid.getNeighbors(currPos);
        // Debug.Log(neighbors.Count + " neighbors found");
        Vector2 nextPosition;
        List<Vector2> posList = AstarCore();
        if (posList.Count == 0){
            nextPosition = new Vector2(0f, 0f);
        }else{
            nextPosition = posList[0];
        }
        
        Debug.Log("next position is " + nextPosition);
        // Vector2 nextPosition = new Vector2(0f, 0f);
        Vector2 offset = nextPosition - currPos;

        // transform.position = Vector2.Lerp(currPos, nextPosition, 1);
        // Vector2.MoveTowards(currPos, nextPosition);

        float moveHorizontal;
        float moveVertical;
        if (offset.magnitude < MINIMUM_MOUSE_OFFSET)
        {
            moveHorizontal = 0.0f;
            moveVertical = 0.0f;
        }else{
            offset.Normalize();
            moveHorizontal = offset.x;
            moveVertical = offset.y;
        }

        Vector2 forceDirection = new Vector2(moveHorizontal, moveVertical);
        rb2d.AddForce(forceDirection * speedMultiplier);
        // UnityEditor.EditorApplication.isPlaying = false;
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
        while (counter < 1000 && !pQ.isEmpty()){
            (Vector2, List<Vector2>, float) element = pQ.DeQueue();
            position = element.Item1;
            path = element.Item2;
            cost = element.Item3;
            
            Debug.Log("Distance is " + Vector2.Distance(position, targetPosition));
            if (Vector2.Distance(position, targetPosition) < 1.5f){
                Debug.Log("return the path");
                return path;
            }

            if (!visited.Contains(position)){
                visited.Add(position);
                // Vector3 spawnPointOffset = new Vector3(position.x, position.y, 0.0f);
                // GameObject newBomb = Instantiate(bomb, spawnPointOffset, Quaternion.identity, bombsParent);
            }

            List<(Vector2, float)> neighbors = grid.getNeighbors(position);
            foreach ((Vector2, float) neighbor in neighbors){
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

            counter ++;
        }
        Debug.Log("return empty");
        return new List<Vector2>();
        
        
        // pQueue = util.PriorityQueue()  
        // visited = set()  # store the visited state
        // path = []
        // state = problem.getStartState()
        // cost = 0
        // pQueue.push((state, path, cost), cost)

        // while not pQueue.isEmpty():
        //     state, path, cost = pQueue.pop()

        //     if problem.isGoalState(state):
        //         return path

        //     if state not in visited:  # any state has been visited doesn't need to be visited again
        //         visited.add(state)

        //         successors = problem.getSuccessors(state)
        //         for child_state, child_path, child_cost in successors:
        //             total_cost = cost + child_cost + heuristic(child_state, problem, gameState)
        //             pQueue.push((child_state, path + [child_path], cost + child_cost), total_cost)

        // return []
    }

    
}
