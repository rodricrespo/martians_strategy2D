using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool isSelected;
    public bool hasMoved;
    public float moveSpeed = 5f;
    public BTNode unitRoot;
    public int unitRange = 3; 
    public int health = 10;
    public int maxHealth = 10;

    private GameManager gm;
    private GameObject gameLogicObject;
    private Grid grid;
    private GameObject pGrid;
    private Node currentNode;
    private List<Vector3> path;
    private BehaviourTree bt;
    private Healthbar healthbar;

    void Start()
    {
        gameLogicObject = GameObject.Find("GameLogic");
        gm = gameLogicObject.GetComponent<GameManager>();
        bt = gameLogicObject.GetComponent<BehaviourTree>();

        pGrid = GameObject.Find("PGrid");
        grid = pGrid.GetComponent<Grid>();

        isSelected = false;
        hasMoved = false;   //Resetearlo tras cambiar de turno

        if (this.tag == "EnemyUnit1") {
            unitRoot = bt.unitRoot;
            unitRange = 3;
        }

        if(this.tag == "PlayerUnit1") unitRange = 3;

        // Encuentra el Healthbar como nieto del objeto Unit
        healthbar = GetComponentInChildren<Healthbar>();
    }

    
    void Update()
    {
        healthbar.UpdateHealthbar(health, maxHealth);
    }

    public void MoveUnit(Node targetNode)
    {
        if (isSelected && !hasMoved)
        {
            if (targetNode != null && targetNode.walkable)
            {
                MoveToNode(targetNode);
            }
        }
    }
    private void OnMouseDown() //selecciona o deselecciona unidad
    {
        if (gm.currentTurn == 1){
            if (!isSelected) {  //Selecciona
                if (this.tag!="EnemyUnit1"){
                    gm.selectedUnit = this;
                    isSelected = true;
                    //Debug.Log(this.ToString() + " selected");
                    if(this.tag == "PlayerUnit1") GetWalkableTiles(); 
                }
            }

            else{   //Deselecciona
                gm.selectedUnit = null;
                gm.ResetTiles();
                isSelected = false;
                //Debug.Log(this.ToString() + " Deseeleccionado");
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
            unitNode.influenceCost += 10f; // Valor de influencia 
            foreach (Node neighbour in grid.GetNeighours(unitNode))
            {
                neighbour.influenceCost += 5f; // Valor de influencia en nodos vecinos
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

    
    public void MoveToNode(Node targetNode)
    {
        // Obtener la ruta utilizando el algoritmo de pathfinding
        if (this.tag == "EnemyUnit1") path = Pathfinding.FindEnemyPath(transform.position, targetNode.worldPosition);
        else path = Pathfinding.FindPath(transform.position, targetNode.worldPosition);

        if (path.Count > 0)
        {
            currentNode = Pathfinding.grid.NodeFromWorldPoint(transform.position);
            if (currentNode != null)
            {
                currentNode.walkable = true;
                currentNode.hasUnit = false;
            }

            StartCoroutine(StartMovementCoroutine(targetNode));
            gm.ResetTiles();
        }
    }

    private IEnumerator StartMovementCoroutine(Node targetNode)
    {
        int steps = 0;
        Node lastNode = null;

        while (steps < unitRange && path.Count > 0)
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

    void GetWalkableTiles()
    {
        if (hasMoved == true)
        {
            return;
        }

        Tiles[] tiles = FindObjectsOfType<Tiles>();
        foreach (Tiles tile in tiles)
        {
            if (Mathf.Abs(transform.position.x - tile.transform.position.x) + Mathf.Abs(transform.position.y - tile.transform.position.y) <= unitRange + 0.5f)
            {
                if (tile.isClear() == true) tile.LightUp();
            }
        }
    }

}
