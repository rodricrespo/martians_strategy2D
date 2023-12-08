using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool isSelected;
    public bool hasMoved;
    public float moveSpeed = 5f;

    private GameManager gm;
    private GameObject gameLogicObject;
    private Grid grid;
    private GameObject pGrid;
    private Node currentNode;
    private List<Vector3> path;

    void Start()
    {
        gameLogicObject = GameObject.Find("GameLogic");
        gm = gameLogicObject.GetComponent<GameManager>();

        pGrid = GameObject.Find("PGrid");
        grid = pGrid.GetComponent<Grid>();

        isSelected = false;
        hasMoved = false;   //Resetearlo tras cambiar de turno
    }

    
     void Update()
    {
        // Verificar clic derecho del mouse
        if (Input.GetMouseButtonDown(1))
        {
            HandleRightClick();
        }
    }

private void HandleRightClick()
{
    if (isSelected && !hasMoved)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Node targetNode = grid.NodeFromWorldPoint(mousePosition);

        if (targetNode != null && targetNode.walkable)
        {
            Debug.Log("ENTRA");
            MoveToNode(targetNode);
        }
    }
}
    private void OnMouseDown() //selecciona o deselecciona unidad
    {
        if (gm.currentTurn == 1){
            if (!isSelected) {  //Selecciona
                gm.selectedUnit = this;
                isSelected = true;
                Debug.Log(this.ToString() + " Seleccionado");
                //if(this.tag == "`PlayerUnit1") GetWalkableTilesUnit1();  //FALTA IMPLEMENTAR
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

    
    private void MoveToNode(Node targetNode)
    {
        // Obtener la ruta utilizando el algoritmo de pathfinding
        path = Pathfinding.FindPath(transform.position, targetNode.worldPosition);

        Node lastNode = null;
        if (path.Count > 0)
        {
            int steps = 0;
            if (currentNode != null)
            {
                currentNode.walkable = true;
                currentNode.hasUnit = false;
            }

            StartCoroutine(StartMovementCoroutine(targetNode));
        }
    }

    private IEnumerator StartMovementCoroutine(Node targetNode)
    {
        int steps = 0;
        Node lastNode = null;

        while (path.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[0], moveSpeed * Time.deltaTime);

            if (transform.position == path[0])
            {
                steps++;
                lastNode = Pathfinding.grid.NodeFromWorldPoint(path[0]);
                path.RemoveAt(0);
            }

            yield return null;
        }

        if (lastNode != null)
        {
            lastNode.walkable = false;
            lastNode.hasUnit = true;
        }

        currentNode = lastNode;
        hasMoved = true;
        //GetEnemies();     //<- HACE FALTA IMPLEMENTARLA
    }

}
