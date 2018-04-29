using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Zenject;
public class AStartSearchTester : MonoBehaviour
{
    [Inject]
    private MapManager _currentMap;
    private void Start()
    {
        StartCoroutine(AStartSearch(new Vector2Int(0, 0), new Vector2Int(10, -4))); 
    }

    public IEnumerator AStartSearch(Vector2Int start, Vector2Int end)
    {

        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, int> costSoFar = new Dictionary<Vector2Int, int>();
        PriorityQueue<Vector2Int> frontier = new PriorityQueue<Vector2Int>();
        frontier.Enqueue(start, 0);
        //start.SetMat(Frontier);

        cameFrom[start] = start;
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            yield return new WaitForSeconds(0.5f);
            Vector2Int current = frontier.Dequeue();
            if (current == end)
            {
                break;
            }

            foreach (Vector2Int neighbor in _currentMap.GetFreeNeighbors(current))
            {
                int newCost = costSoFar[current] + 0;//g.Cost(current, neighbor);
                if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                {
                    costSoFar[neighbor] = newCost;
                    int priority = newCost + MapManager.Heuristic(neighbor, end);
                    frontier.Enqueue(neighbor, priority);
                    //neighbor.SetMat(Frontier);
                    cameFrom[neighbor] = current;
                    //_currentMap.HighlightTile(current.x, current.y);
                }
            }
            yield return null;
        }
    }
}