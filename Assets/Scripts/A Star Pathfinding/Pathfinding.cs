using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour
{
    private Grid grid;
    private static Pathfinding instance;
    void Awake()
    {
        grid = GetComponent<Grid>();
        instance = this;
    }

    public static Vector2[] RequestPath(Vector2 from, Vector2 to)
    {
        return instance.FindPath(from, to);
    }

    Vector2[] FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector2[] waypoints = new Vector2[0];
        bool pathSuccess = false;
        
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet= new HashSet<Node>();
        openSet.Add(startNode);

        if (targetNode.walkable)
        {
            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
            
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost ||
                        openSet[i].fCost == currentNode.fCost &&
                        openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    sw.Stop();
                    //Debug.Log("Path found: "  + sw.ElapsedMilliseconds + " ms");
                    pathSuccess = true;
                    break;
                }
            
                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }
                
                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(
                                                         currentNode,
                                                         neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost ||
                        !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }
        }

        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        
        return waypoints;
    }

    Vector2[] RetracePath(Node startNode, Node targetNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = targetNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        //Vector2[] waypoints = SimplifyPath(path);
        //Array.Reverse(waypoints);
        List<Vector2> waypoints = new List<Vector2>();
        for (int i = 1; i < path.Count; i++)
        {
            waypoints.Add(path[i].worldPosition);
        }
        Vector2[] listOfWaypoints = waypoints.ToArray();
        Array.Reverse(listOfWaypoints);
        return listOfWaypoints;
    }

    Vector2[] SimplifyPath(List<Node> path)
    {
        List<Vector2> waypoints = new List<Vector2>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }
    
    int GetDistance(Node a, Node b)
    {
        int distanceX = Mathf.Abs(a.gridX - b.gridX);
        int distanceY = Mathf.Abs(a.gridY - b.gridY);

        if (distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }

        return 14 * distanceX + 10 * (distanceY - distanceX);

    }
    
}
