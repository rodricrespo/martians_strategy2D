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
            BuyUnit();
        }
        else return;
    }

    private void BuyUnit() {
        // Si se pulsa la tecla 1 --> se puede cambiar a un boton en el canvas
        if (Input.GetKeyDown(KeyCode.Alpha1) && gm.playerResources>10)
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

}
