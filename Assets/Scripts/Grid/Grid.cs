using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public bool displayGridGizmos;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public Node[,] grid;
    public int maxSize;

    public float nodeDiameter;
    public int gridSizeX, gridSizeY;



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
        maxSize = gridSizeX * gridSizeY; //Lo usamos en el pathfinding
        Vector3 worldbottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldbottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)   //Se usa para el pathfinding y al crear sprites <<--- NO CAMBIAR!!!
    { 
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    
    public Vector3 GetRandomWalkablePosition() // Método para obtener una posición walkable y colocar las unidades <<--- NO CAMBIAR!!!
    {
        // Obtener todas las posiciones walkable disponibles
        List<Vector3> walkablePositions = new List<Vector3>();

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (grid[x, y].walkable)
                {
                    walkablePositions.Add(grid[x, y].worldPosition);
                }
            }
        }

        // Verificar si hay posiciones walkable disponibles
        if (walkablePositions.Count > 0)
        {
            // Seleccionar una posición aleatoria de la lista
            int randomIndex = Random.Range(0, walkablePositions.Count);
            return walkablePositions[randomIndex];
        }
        else    //<- No debería de ocurrir nunca
        {
            Debug.LogWarning("No hay posiciones walkable disponibles.");    
            return Vector3.zero; // Otra opción sería devolver null
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

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

    public void UpdateNodesWithoutUnits()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (grid[x, y].hasUnit)
                {
                    // Si el nodo tiene una unidad, actualizarlo
                    grid[x, y].hasUnit = false;
                    //grid[x, y].walkable = true;
                }
            }
        }
    }

    void OnDrawGizmos()      //<-----PARA LAS UNIDADES Y OBSTACULOS
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

        if (grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = Color.white;
                if (!n.walkable)
                    Gizmos.color = Color.red;
                if (n.hasUnit)
                    Gizmos.color = Color.blue;
                if (n.hasPlant)
                    Gizmos.color = Color.green;

                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
    // void OnDrawGizmos() //<-----PARA LA INFLUENCIA
    // {
    //     if (displayGridGizmos && grid != null)
    //     {
    //         foreach (Node node in grid)
    //         {
    //             Debug.Log(node.influenceCost);
    //             Gizmos.color = CalculateInfluenceColor(node.influenceCost);
    //             Gizmos.DrawSphere(node.worldPosition, nodeRadius * 0.5f);
    //         }
    //     }
    // }

    // Color CalculateInfluenceColor(float influence)
    // {
    //     float minInfluence = 1f;
    //     float maxInfluence = 20f;

    //     float normalizedInfluence = Mathf.Clamp01((influence - minInfluence) / (maxInfluence - minInfluence));
    //     return Color.Lerp(Color.yellow, Color.red, normalizedInfluence);
    // }

    public Vector3 GetRandomWalkablePositionInHalf(float xPos)
    {
        List<Vector3> walkablePositions = new List<Vector3>();

        // Filtra las posiciones walkable según la posición x especificada
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = grid[x, y].worldPosition;

                if ((xPos < 0 && worldPoint.x < xPos) ||
                    (xPos >= 0 && worldPoint.x >= xPos))
                {
                    if (grid[x, y].walkable)
                    {
                        walkablePositions.Add(worldPoint);
                    }
                }
            }
        }

        if (walkablePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, walkablePositions.Count);
            return walkablePositions[randomIndex];
        }
        else
        {
            Debug.LogWarning("No hay posiciones walkable disponibles en la mitad especificada.");
            return Vector3.zero;
        }
    }



}
