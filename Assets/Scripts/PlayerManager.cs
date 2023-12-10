using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private GameManager gm;
    private Grid grid;
    private GameObject pGrid;

    void Start() {
        gm = GetComponent<GameManager>();

        pGrid = GameObject.Find("PGrid");
        grid = pGrid.GetComponent<Grid>();
    }

    void Update() {
        if (gm.currentTurn == 1){
            if (gm.GetUnitCountWithTag("PlayerUnit1")<5) BuyUnit();
            if (gm.GetUnitCountWithTag("PlayerUnit2")<3) BuyUnit2();
        }
        else return;
    }

    private void BuyUnit() {
        // Si se pulsa la tecla 1 --> se puede cambiar a un boton en el canvas
        if (Input.GetKeyDown(KeyCode.Alpha1) && gm.playerResources>=10)
        {
            gm.playerResources -= 10;
            Vector3 randomWalkablePosition = grid.GetRandomWalkablePosition(); // Obtener una posición walkable aleatoria del grid
            Node node = grid.NodeFromWorldPoint(randomWalkablePosition);

            GameObject unitObject = Instantiate(gm.playerSpaceshipPrefab, randomWalkablePosition, Quaternion.identity);  // Instancia la nave en pantalla
            Unit myUnit = unitObject.GetComponent<Unit>();          // Obtén el componente Unit del objeto instanciado
            if (myUnit != null) myUnit.ApplyInfluenceToGrid(grid);  // Aplicar influencia sobre el grid
            
            node.walkable = false;
        }
    }

    private void BuyUnit2() {
        // Si se pulsa la tecla 1 --> se puede cambiar a un boton en el canvas
        if (Input.GetKeyDown(KeyCode.Alpha2) && gm.playerResources>=30)
        {
            gm.playerResources -= 30;
            Vector3 randomWalkablePosition = grid.GetRandomWalkablePosition(); // Obtener una posición walkable aleatoria del grid
            Node node = grid.NodeFromWorldPoint(randomWalkablePosition);

            GameObject unitObject = Instantiate(gm.playerSpaceshipPrefab2, randomWalkablePosition, Quaternion.identity);  // Instancia la nave en pantalla
            Unit myUnit = unitObject.GetComponent<Unit>();          // Obtén el componente Unit del objeto instanciado
            if (myUnit != null) myUnit.ApplyInfluenceToGrid(grid);  // Aplicar influencia sobre el grid
            
            node.walkable = false;
        }
    }

    public void BuyAttackPowerup() {    //Se usa en el HUD manager
        gm.playerResources -= 50;

        Vector3 randomPosition = grid.GetRandomWalkablePositionInHalf(grid.gridWorldSize.x / 4);
        Node node = grid.NodeFromWorldPoint(randomPosition);
        if (randomPosition != Vector3.zero)
        {
            // Crear una instancia de gm.powerupPrefab en la posición obtenida
            GameObject powerupInstance = Instantiate(gm.playerPowerup1Prefab, randomPosition, Quaternion.identity);
            gm.playerPowerMultiplier += 1;
            node.walkable = false;
        }
    }

    public void BuyResourcesPowerup(){
        gm.playerResources -= 50;
        Vector3 randomPosition = grid.GetRandomWalkablePositionInHalf(grid.gridWorldSize.x / 4);
        Node node = grid.NodeFromWorldPoint(randomPosition);
        if (randomPosition != Vector3.zero)
        {
            // Crear una instancia de gm.powerupPrefab en la posición obtenida
            GameObject powerupInstance = Instantiate(gm.playerPowerup2Prefab, randomPosition, Quaternion.identity);
            gm.playerResourcesMultiplier += 1;
            node.walkable = false;
        }
    }

    public void BuyLockPowerup(){
        gm.playerResources -= 50;
        Vector3 randomPosition = grid.GetRandomWalkablePositionInHalf(grid.gridWorldSize.x / 4);
        Node node = grid.NodeFromWorldPoint(randomPosition);
        if (randomPosition != Vector3.zero)
        {
            // Crear una instancia de gm.powerupPrefab en la posición obtenida
            GameObject powerupInstance = Instantiate(gm.playerPowerup3Prefab, randomPosition, Quaternion.identity);
            gm.AIresourcesMultiplier = 0;   //Anula el conseguir recursos
            gm.AIpowerMultiplier = 1;
            node.walkable = false;
        }
    }

}
