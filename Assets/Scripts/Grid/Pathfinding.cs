using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Pathfinding : MonoBehaviour
{
    public static Grid grid;

    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    //A*
    public static List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos)
    {

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);


        Heap<Node> openSet = new Heap<Node>(grid.maxSize);
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, currentNode);
            }

            foreach (Node neighbour in grid.GetNeighours(currentNode))
            {
                if (neighbour.hasTree || !neighbour.walkable || closedSet.Contains(neighbour) || neighbour.hasUnit)
                    continue;

                // Añadir la influencia al cálculo del nuevo costo de movimiento
                float newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.influenceCost;

                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = (int)newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        return new List<Vector3>();
    }

    public static List<Vector3> RetracePath(Node startNode, Node endNode)
    {
        List<Vector3> path = new List<Vector3>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.worldPosition);
            currentNode = currentNode.parent;
        }

        //Como path es una ruta que empieza en el nodo final, revertirla
        path.Reverse();

        return path;
    }

    public static int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
        {
            return 2000 * dstY + 10 * (dstX - dstY);
        }

        return 2000 * dstX + 10 * (dstY - dstX);
    }

    public static IEnumerator MoveUnit(Unit unit, Vector3 targetPosition, float moveSpeed)
    {
        Node startNode = grid.NodeFromWorldPoint(unit.transform.position);
        Node targetNode = grid.NodeFromWorldPoint(targetPosition);

        if (startNode != null && targetNode != null)
        {
            List<Vector3> path = FindPath(unit.transform.position, targetPosition);

            foreach (Vector3 waypoint in path)
            {
                while (Vector3.Distance(unit.transform.position, waypoint) > 0.1f)
                {
                    // Mueve gradualmente la unidad hacia el waypoint
                    unit.transform.position = Vector3.MoveTowards(unit.transform.position, waypoint, Time.deltaTime * moveSpeed);
                    yield return null;
                }
            }

            // Reinicia el estado después de llegar al destino
            unit.hasMoved = true;
            unit.isSelected = false;
        }
    }
}
