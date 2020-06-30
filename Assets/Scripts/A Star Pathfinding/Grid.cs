using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class Grid : MonoBehaviour
{
   public bool displayGrid;
   public Vector2 gridWorldSize;
   public LayerMask unwalkableMask;
   public float nodeRadius;
   private Node[,] grid;

   private float nodeDiameter;
   private int gridSizeX;
   private int gridSizeY;
   void Awake()
   {
      nodeDiameter = nodeRadius * 2;
      gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
      gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
      CreateGrid();
   }
   void CreateGrid()
   {
      grid = new Node[gridSizeX, gridSizeY];
      Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridWorldSize.x / 2 - Vector2.up * gridWorldSize.y / 2;
      
      for (int x = 0; x < gridSizeX; x++)
      {
         for (int y = 0; y < gridSizeY; y++)
         {
            Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y *  nodeDiameter + nodeRadius);
            bool walkable = (Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask) == null);
            grid[x, y] = new Node(walkable, worldPoint, x, y);
         }
      }
   }

   public Node NodeFromWorldPoint(Vector2 worldPosition)
   {
      float percentX = (worldPosition.x - transform.position.x) / gridWorldSize.x + 0.5f - (nodeRadius / gridWorldSize.x);
      float percentY = (worldPosition.y - transform.position.y) / gridWorldSize.y + 0.5f - (nodeRadius / gridWorldSize.y);﻿
      percentX = Mathf.Clamp01(percentX);
      percentY = Mathf.Clamp01(percentY);

      int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
      int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
      return grid[x, y];
   }

   public List<Node> GetNeighbours(Node node)
   {
      List<Node> neighbours = new List<Node>();
      
      for (int x = -1; x <= 1; x++){
         for (int y = -1; y <= 1; y++){
            if (x == 0 && y == 0)
            {
               continue;
            }

            int checkX = node.gridX + x;
            int checkY = node.gridY + y;

            if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
            {
               neighbours.Add(grid[checkX, checkY]);
            }
         }
      }
      return neighbours;
   }
   
   private void OnDrawGizmos()
   {
      Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));
      if (grid != null && displayGrid)
      {
         foreach (Node node in grid)
         {
            Gizmos.color = (node.walkable) ? Color.white : Color.red;
            Gizmos.DrawCube(node.worldPosition, Vector2.one * (nodeDiameter - 0.03f));
         }
      }
   }
}
