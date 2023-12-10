using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Grid grid;  // Referencia al script Grid
    public Sprite[] tiles;
    public float tileScaleFactor = 1.0f; // Para cambiar el tamaño de las tiles
    public GameObject plantPrefab;
    public GameObject enemySpaceshipPrefab;
    public GameObject playerSpaceshipPrefab;


    public int maxPlants = 10;
    public int maxEnemySpaceships = 3;
    public int maxPlayerSpaceships = 3;

    private Node node;

    void Start()
    {
        GenerateMap();
        GeneratePlants();
        GenerateEnemySpaceships();
        GeneratePlayerSpaceships();
    }

    void GenerateMap()
    {
        for (int x = 0; x < grid.gridSizeX; x++)
        {
            for (int y = 0; y < grid.gridSizeY; y++)
            {
                int randomTileIndex = Random.Range(0, tiles.Length);
                Vector2 position = grid.grid[x, y].worldPosition;

                CreateTile(randomTileIndex, position, grid.nodeDiameter, tileScaleFactor);
            }
        }
    }

    void CreateTile(int tileIndex, Vector2 position, float tileSize, float scaleFactor)
    {
        GameObject tileObject = new GameObject("Tile");
        tileObject.transform.position = position;

        node = Pathfinding.grid.NodeFromWorldPoint(tileObject.transform.position);
        node.walkable = true;

        SpriteRenderer tileRenderer = tileObject.AddComponent<SpriteRenderer>();
        tileRenderer.sprite = tiles[tileIndex];

        // Agregar el script Tile
        Tiles tileScript = tileObject.AddComponent<Tiles>();

        // Agregar Collider2D
        Collider2D tileCollider = tileObject.AddComponent<BoxCollider2D>();

        // Ajustar la escala del objeto directamente
        tileObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1.0f);
        tileObject.transform.position = new Vector3(position.x, position.y, 0.01f);
    }

    void GeneratePlants()
    {
        for (int i = 0; i < maxPlants; i++)
        {
            Vector3 randomPosition = grid.GetRandomWalkablePosition();
            
            if (randomPosition != Vector3.zero)
            {
                Instantiate(plantPrefab, randomPosition, Quaternion.identity);

                // Marcar la posición como no caminable
                Node node = grid.NodeFromWorldPoint(randomPosition);
                if (node != null)
                {
                    node.walkable = false;
                    node.hasPlant = true;
                }
            }
        }
    }

    void GenerateEnemySpaceships()
    {
        for (int i = 0; i < maxEnemySpaceships; i++)
        {
            Vector3 randomPosition = grid.GetRandomWalkablePositionInHalf(-grid.gridWorldSize.x / 4); // Mitad izquierda
            if (randomPosition != Vector3.zero)
            {
                Instantiate(enemySpaceshipPrefab, randomPosition, Quaternion.identity);

                Node node = grid.NodeFromWorldPoint(randomPosition);
                if (node != null)
                {
                    node.walkable = false;
                    node.hasUnit = true;
                }
            }
        }
    }

    void GeneratePlayerSpaceships()
    {
        for (int i = 0; i < maxPlayerSpaceships; i++)
        {
            Vector3 randomPosition = grid.GetRandomWalkablePositionInHalf(grid.gridWorldSize.x / 4); // Mitad derecha
            if (randomPosition != Vector3.zero)
            {
                Instantiate(playerSpaceshipPrefab, randomPosition, Quaternion.identity);

                Node node = grid.NodeFromWorldPoint(randomPosition);
                if (node != null)
                {
                    node.walkable = false;
                    node.hasUnit = true;
                }
            }
        }
    }
}