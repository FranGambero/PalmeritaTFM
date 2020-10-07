using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ElJardin.Util
{
    public class Pathfinding
    {
        #region Cons
        const int MOVE_STRAIGHT_COST = 10;
        const int MOVE_DIAGONAL_COST = 14;
        #endregion
        
        #region Accesors
        MapManager grid;
        List<Node> openList;
        List<Node> closedList;
        #endregion
        
        public Pathfinding(MapManager map)
        {
            grid = map;
        }
        
        #region Pathfinding

        public List<Node> FindPath(int startX, int startY, int endX, int endY)
        {
            var startNode = grid.GetNode(startX, startY);
            var endNode = grid.GetNode(endX, endY);
            openList = new List<Node> { startNode };
            closedList = new List<Node>();

            for (var i = 0; i < grid.rows; i++)
            {
                for (var j = 0; j < grid.columns; j++)
                {
                    var pathNode = grid.GetNode(i, j);
                    pathNode.GCost = int.MaxValue;
                    pathNode.CalculateFCost();
                    pathNode.CameFromNode = null;
                }
            }

            startNode.GCost = 0;
            startNode.HCost = CalculateDistance(startNode, endNode);
            startNode.CalculateFCost();
            while (openList.Count > 0)
            {
                var currentNode = GetLowestFCostNode(openList);
                if (currentNode == endNode)
                {
                    //END
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (var neighbourNode in GetNeighbourList(currentNode))
                {
                    if (closedList.Contains(neighbourNode))
                        continue;

                    var tentativeGCost = currentNode.GCost + CalculateDistance(currentNode, neighbourNode);
                    if (tentativeGCost < neighbourNode.GCost)
                    {
                        neighbourNode.CameFromNode = currentNode;
                        neighbourNode.GCost = tentativeGCost;
                        neighbourNode.HCost = CalculateDistance(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();
                        
                        if (!openList.Contains(neighbourNode))
                            openList.Add(neighbourNode);
                    }
                }
            }
            return null;
        }

        List<Node> CalculatePath(Node endNode)
        {
            var path = new List<Node>();
            path.Add(endNode);
            var currentNode = endNode;
            while (currentNode.CameFromNode != null)
            {
                path.Add(currentNode.CameFromNode);
                currentNode = currentNode.CameFromNode;
            }
            path.Reverse();
            return path;
        }

        List<Node> GetNeighbourList(Node currentNode)
        {
            var neighbourList = new List<Node>();
            if (currentNode.row - 1 >= 0)
            {
                //Left
                neighbourList.Add(grid.GetNode(currentNode.row - 1, currentNode.column));
                //Left Down
                if (currentNode.column - 1 >= 0)
                    neighbourList.Add(grid.GetNode(currentNode.row - 1, currentNode.column - 1));
                //Left Up
                if (currentNode.column + 1 < grid.columns)
                    neighbourList.Add(grid.GetNode(currentNode.row - 1, currentNode.column + 1));
            }
            if (currentNode.row + 1 < grid.rows)
            {
             //Right
             neighbourList.Add(grid.GetNode(currentNode.row + 1, currentNode.column));
             //Right Down
             if (currentNode.column - 1 >= 0)
                 neighbourList.Add(grid.GetNode(currentNode.row + 1, currentNode.column - 1));
             //Right Up
             if (currentNode.column + 1 < grid.columns)
                 neighbourList.Add(grid.GetNode(currentNode.row + 1, currentNode.column + 1));
            }
            //Down
            if (currentNode.column - 1 >= 0) neighbourList.Add(grid.GetNode(currentNode.row, currentNode.column - 1));
            //Up
            if (currentNode.column + 1 < grid.columns)
                neighbourList.Add((grid.GetNode(currentNode.row, currentNode.column + 1)));
            
            return neighbourList;
        }

        Node GetLowestFCostNode(List<Node> pathNodeList)
        {
            var lowestFCostNode = pathNodeList[0];
            foreach (var node in pathNodeList)
            {
                if (node.FCost < lowestFCostNode.FCost)
                    lowestFCostNode = node;
            }
            return lowestFCostNode;
        }

        int CalculateDistance(Node a, Node b)
        {
            var xDistance = Mathf.Abs(a.row - b.row);
            var yDistance = Mathf.Abs(a.column - b.column);
            var remaining = Mathf.Abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }
        #endregion
    }
}