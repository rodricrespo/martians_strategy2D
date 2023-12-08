using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool isSelected;
    public bool hasMoved;

    private GameManager gm;
    private GameObject gameLogicObject;

    void Start()
    {
        gameLogicObject = GameObject.Find("GameLogic");
        gm = gameLogicObject.GetComponent<GameManager>();

        isSelected = false;
        hasMoved = false;
    }

    
    void Update()
    {

    }

    private void OnMouseDown() //selecciona o deselecciona unidad
    {
        if (gm.currentTurn == 1){
            if (!isSelected) {  //Selecciona
                gm.selectedUnit = this;
                isSelected = true;
                Debug.Log(this.ToString() + " Seleccionado");
                //if(this.tag == "`PlayerUnit1") GetWalkableTilesUnit1();
            }

            else{   //Deselecciona
                gm.selectedUnit = null;
                isSelected = false;
                Debug.Log(this.ToString() + " Deseeleccionado");
            }
        }
        else return;
    }

    public void ApplyInfluenceToGrid(Grid grid)
    {
        // Obtener el nodo correspondiente a la posición de la unidad
        Node unitNode = grid.NodeFromWorldPoint(transform.position);

        // Resetear la influencia en el nodo y en los nodos vecinos
        ResetInfluenceInGrid(grid);

        // Establecer la influencia en el nodo y en los nodos vecinos
        if (unitNode != null)
        {
            unitNode.influenceCost = 10f; // Valor de influencia 
            foreach (Node neighbour in grid.GetNeighours(unitNode))
            {
                neighbour.influenceCost = 5f; // Valor de influencia en nodos vecinos
            }
        }
    }

    private void ResetInfluenceInGrid(Grid grid)
    {
        foreach (Node node in grid.grid)
        {
            node.influenceCost = 0f; // Resetear la influencia en todos los nodos
        }
    }
}
