using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarSimp : MonoBehaviour
{
    // Start is called before the first frame update
    public float speedMultiplier;
    private Rigidbody2D rb2d;
    private const float MINIMUM_MOUSE_OFFSET = 0.4f;
    private Vector2 targetPosition;
    public float height;
    public float width; 
    public LayerMask layerMask;
    public GameObject target;
    private MyGrid grid;
    void Start()
    {
        // Collider2D[] intersection = Physics2D.OverlapCircleAll(new Vector2(8.86f, -7.99f), 1f);
        // Debug.Log("collider number is " + intersection.Length);
        // UnityEditor.EditorApplication.isPlaying = false;

        grid = new MyGrid(width, height);
        grid.setLayerMask(layerMask);
        targetPosition = (Vector2)target.transform.position;
        // targetPosition = new Vector2(4f, 6f);
        Debug.Log("target position is " + targetPosition);
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        Vector2 currPos = gameObject.transform.position;
        Vector2 nextPosition;
        List<Vector2> posList = AstarCore();
        if (posList.Count == 0){
            nextPosition = new Vector2(0f, 0f);
        }else{
            Debug.Log("number of node in list is " + posList.Count);
            nextPosition = posList[0];
        }
        Debug.Log("next position is " + nextPosition);
        Vector2 offset = nextPosition - currPos;
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
    }

    List<Vector2> AstarCore(){
        PriorityQueue<(Vector2, List<Vector2>, float)> pQ = new PriorityQueue<(Vector2, List<Vector2>, float)>();
        HashSet<Vector2> visited = new HashSet<Vector2>();
        List<Vector2> path = new List<Vector2>();
        Vector2 position = gameObject.transform.position;
        float cost = 0f;
        pQ.Enqueue((position, path, cost),cost);

        int counter = 0;
        while (counter < 800){
            (Vector2, List<Vector2>, float) element = pQ.DeQueue();
            position = element.Item1;
            path = element.Item2;
            cost = element.Item3;

            Debug.Log("Distance is " + Vector2.Distance(position, targetPosition));
            if (Vector2.Distance(position, targetPosition) < 2f){
                Debug.Log("return the path");
                return path;
            }

            if (!visited.Contains(position)){
                visited.Add(position);
            }

            List<(Vector2, float)> neighbors = grid.getNeighbors(position);
            foreach ((Vector2, float) neighbor in neighbors){
                Vector2 neighborPosition = neighbor.Item1;
                float neighborCost = neighbor.Item2;
                float heuristicCost = Vector2.Distance(neighborPosition, targetPosition);
                float currentCost = cost + neighborCost;
                float totalCost = currentCost + heuristicCost;
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
