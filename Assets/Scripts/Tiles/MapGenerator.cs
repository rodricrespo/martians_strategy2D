using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Grid grid;  // Referencia al script Grid
    public Sprite[] tiles;
    public float tileScaleFactor = 1.0f; // Para cambiar el tamaño de las tiles
    public GameObject plantPrefab;
    public int maxPlants = 10;

    void Start()
    {
        GenerateMap();
        GeneratePlants();
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

    void GeneratePlants()
    {
        for (int i = 0; i < maxPlants; i++)
        {
            Vector3 randomPosition = grid.GetRandomWalkablePosition();
            
            if (randomPosition != Vector3.zero) // Asegúrate de que se haya encontrado una posición válida
            {
                Instantiate(plantPrefab, randomPosition, Quaternion.identity);

                // Marcar la posición como no caminable
                Node node = grid.NodeFromWorldPoint(randomPosition);
                if (node != null)
                {
                    node.walkable = false;
                }
            }
        }
    }

    void CreateTile(int tileIndex, Vector2 position, float tileSize, float scaleFactor)
    {
        GameObject tileObject = new GameObject("Tile");
        tileObject.transform.position = position;

        SpriteRenderer tileRenderer = tileObject.AddComponent<SpriteRenderer>();
        tileRenderer.sprite = tiles[tileIndex];

        // Ajustar la escala del objeto directamente
        tileObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1.0f);
    }
}