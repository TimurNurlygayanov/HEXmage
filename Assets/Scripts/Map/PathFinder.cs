using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// useful video:
// https://www.youtube.com/watch?v=alU04hvz6L4
//


public class PathFinder
{
    public List<PathNode> full_path;

    private const int MOVING_BASIC_COST = 10;

    private HexGrid grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;

    public PathFinder(HexGrid grid)
    {
        this.grid = grid;
    }

    public List<PathNode> findPath(PathNode start_node, PathNode end_node)
    {
        openList = new List<PathNode> { start_node };
        closedList = new List<PathNode>();

        for (int i = 0; i < grid.width; i++)
        {
            for (int j = 0; j < grid.height; j++)
            {
                PathNode node = grid.gridArray[i, j];

                if (node != null)
                {
                    PathNode pathNode = node.GetComponent<PathNode>();

                    pathNode.gCost = int.MaxValue;
                    pathNode.calculateFCost();
                    pathNode.cameFromNode = null;
                }
            }
        }

        start_node.gCost = 0;
        start_node.hCost = calculateDistanceCost(start_node, end_node);
        start_node.calculateFCost();

        while (openList.Count > 0)
        {
            var current = getTheLowestFCostNode(openList);

            if (current == end_node)
            {
                // Reached the final node:
                full_path = getRoute(end_node);
                return full_path;
            }

            openList.Remove(current);
            closedList.Add(current);

            PathNode current_path_node = current.GetComponent<PathNode>();
            foreach (var neighbor in getNeighbors(current))
            {
                if (neighbor != null && !closedList.Contains(neighbor))
                {
                    PathNode neighbor_path_node = neighbor.GetComponent<PathNode>();
                    int tentativeGCost = current_path_node.gCost + calculateDistanceCost(current_path_node, neighbor_path_node);

                    if (tentativeGCost < neighbor_path_node.gCost)
                    {
                        neighbor_path_node.cameFromNode = current_path_node;
                        neighbor_path_node.gCost = tentativeGCost;
                        neighbor_path_node.hCost = calculateDistanceCost(neighbor_path_node, end_node);
                        neighbor_path_node.calculateFCost();

                        if (!openList.Contains(neighbor))
                        {
                            openList.Add(neighbor);
                        }
                    }
                }
            }
        }

        // Out of nodes in the openList
        return null;
    }

    public void drawPath()
    {
        if (full_path != null)
        {
            foreach(PathNode node in full_path)
            {
                grid.gridArray[node.x, node.z].GetComponent<Renderer>().material.color = Color.magenta;
            }
        }
    }

    public List<PathNode> getNeighbors(PathNode node)
    {
        List<PathNode> neighbors = new List<PathNode>();
        PathNode neighbor;

        if (node != null)
        {
            // PathNode node = node_object.GetComponent<PathNode>();

            if (node.z % 2 == 0) /// bug here
            {
                neighbor = grid.gridArray[node.x, node.z + 1];
                if (neighbor != null && neighbor.status != "blocked")
                {
                    neighbors.Add(neighbor);
                }

                neighbor = grid.gridArray[node.x + 1, node.z];
                if (neighbor != null && neighbor.status != "blocked")
                {
                    neighbors.Add(neighbor);
                }

                neighbor = grid.gridArray[node.x - 1, node.z + 1];
                if (neighbor != null && neighbor.status != "blocked")
                {
                    neighbors.Add(neighbor);
                }

                neighbor = grid.gridArray[node.x - 1, node.z];
                if (neighbor != null && neighbor.status != "blocked")
                {
                    neighbors.Add(neighbor);
                }

                neighbor = grid.gridArray[node.x, node.z - 1];
                if (neighbor != null && neighbor.status != "blocked")
                {
                    neighbors.Add(neighbor);
                }

                neighbor = grid.gridArray[node.x - 1, node.z - 1];
                if (neighbor != null && neighbor.status != "blocked")
                {
                    neighbors.Add(neighbor);
                }
            }
            else
            {
                neighbor = grid.gridArray[node.x + 1, node.z + 1];
                if (neighbor != null && neighbor.status != "blocked")
                {
                    neighbors.Add(grid.gridArray[node.x + 1, node.z + 1]);
                }

                neighbor = grid.gridArray[node.x + 1, node.z];
                if (neighbor != null && neighbor.status != "blocked")
                {
                    neighbors.Add(grid.gridArray[node.x + 1, node.z]);
                }

                neighbor = grid.gridArray[node.x + 1, node.z - 1];
                if (neighbor != null && neighbor.status != "blocked")
                {
                    neighbors.Add(grid.gridArray[node.x + 1, node.z - 1]);
                }

                neighbor = grid.gridArray[node.x, node.z - 1];
                if (neighbor != null && neighbor.status != "blocked")
                {
                    neighbors.Add(grid.gridArray[node.x, node.z - 1]);
                }

                neighbor = grid.gridArray[node.x - 1, node.z];
                if (neighbor != null && neighbor.status != "blocked")
                {
                    neighbors.Add(grid.gridArray[node.x - 1, node.z]);
                }

                neighbor = grid.gridArray[node.x, node.z + 1];
                if (neighbor != null && neighbor.status != "blocked")
                {
                    neighbors.Add(grid.gridArray[node.x, node.z + 1]);
                }
            }
        }

        
        foreach(PathNode n in neighbors)
        {
            n.GetComponent<Renderer>().material.color = Color.black;
        }

        return neighbors;
    }

    private List<PathNode> getRoute(PathNode end_node)
    {
        List<PathNode> path = new List<PathNode>();

        PathNode current_node = end_node.GetComponent<PathNode>();

        if (current_node.status != "blocked")
        {
            path.Add(current_node);
        }

        while (current_node.cameFromNode != null)
        {
            path.Add(current_node.cameFromNode);
            current_node = current_node.cameFromNode;
        }

        path.Reverse();

        return path;
    }

    private int calculateDistanceCost(PathNode a, PathNode b)
    {
        int distance = 0;

        if (a.x == b.x) {
            distance = Mathf.Abs(b.z - a.z);
        }
        else if (a.z == b.z) {
            distance = Mathf.Abs(b.x - a.x);
        } else
        {
            int dx = Mathf.Abs(b.x - a.x);
            int dz = Mathf.Abs(b.z - a.z);

            if (a.z < b.z) {
                distance = dx + dz - (int) Mathf.Ceil(dx / 2.0f);
            } else {
                distance = dx + dz - (int) Mathf.Floor(dx / 2.0f);
            }
        }

        Debug.Log(distance);

        return distance * MOVING_BASIC_COST;
    }

    private PathNode getTheLowestFCostNode(List<PathNode> pathNodeList)
    {
        if (pathNodeList.Count <= 0)
        {
            return null;
        }

        var lowest = pathNodeList[0];

        for (int i = 0; i < pathNodeList.Count; i++)
        {
            PathNode i_node = pathNodeList[i];

            if (i_node.fCost < lowest.fCost)
            {
                lowest = pathNodeList[i];
            }
        }

        return lowest;
    }
}
