using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public bool isSelected;
    public bool hasMoved;
    public float moveSpeed = 5f;
    public BTNode unitRoot;
    public int unitRange = 3; 
    public int health = 10;
    public int maxHealth = 10;
    public int attackPower = 5;
    public bool canAttack;
    public Unit playerTarget;

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
        canAttack = false;
        playerTarget = null;

        if (this.tag == "EnemyUnit1") {
            unitRoot = bt.unitRoot;
            unitRange = 3;
            attackPower = 5;
        }

        if(this.tag == "PlayerUnit1") {
            unitRange = 3;
            attackPower = 5;
        }

        // Encuentra el Healthbar como nieto del objeto Unit
        healthbar = GetComponentInChildren<Healthbar>();
    }

    
    void Update()
    {
        healthbar.UpdateHealthbar(health, maxHealth);
    }

    public void MoveUnit(Node targetNode)   //Se usa en Tile.cs porque se tiene que clickar una Tile, es para el Player
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
                    if(this.tag == "PlayerUnit1") {
                        GetWalkableTiles(); 
                        CheckEnemyUnitsInNeighbours();
                    }
                }
            }

            else{   //Deselecciona
                gm.selectedUnit = null;
                gm.ResetTiles();
                isSelected = false;
                playerTarget = null;
                //Debug.Log(this.ToString() + " Deseeleccionado");
            }
        }
        else return;
    }

    public void ApplyInfluenceToGrid(Grid grid)
    {
        // Obtener el nodo correspondiente a la posición de la unidad
        Node unitNode = grid.NodeFromWorldPoint(transform.position);

        // Resetear la influencia en todos los nodos
        ResetInfluenceInGrid(grid);

        // Establecer la influencia en el nodo y en los nodos adyacentes
        if (unitNode != null)
        {
            // Parámetros de influencia
            float baseInfluence = 10f;
            float influenceDecayRate = 2f;

            // Aplicar influencia inicial en el nodo actual
            unitNode.influenceCost += baseInfluence;

            // Crear una lista de nodos para visitar
            Queue<Node> nodesToVisit = new Queue<Node>();
            nodesToVisit.Enqueue(unitNode);

            while (nodesToVisit.Count > 0)
            {
                // Obtener el siguiente nodo de la cola
                Node currentNode = nodesToVisit.Dequeue();

                // Obtener los nodos vecinos
                List<Node> neighbours = grid.GetNeighbours(currentNode);

                foreach (Node neighbour in neighbours)
                {
                    if (neighbour != null && !nodesToVisit.Contains(neighbour))
                    {
                        // Calcular la distancia entre el nodo actual y el vecino
                        float distance = Vector3.Distance(currentNode.worldPosition, neighbour.worldPosition);

                        // Ajustar la influencia en función de la distancia
                        float influenceValue = baseInfluence - influenceDecayRate * distance;

                        // Establecer la influencia en el vecino
                        neighbour.influenceCost += Mathf.Max(influenceValue, 0f);

                        // Añadir el vecino a la lista de nodos a visitar si no ha sido visitado
                        if (!nodesToVisit.Contains(neighbour))
                        {
                            nodesToVisit.Enqueue(neighbour);
                        }
                    }
                }
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
            CheckEnemyUnitsInNeighbours();
            //if (!canAttack) gm.ResetTiles();
            gm.ResetTiles();

            //ApplyInfluenceToGrid(grid); // Aplicar influencia a la cuadrícula después de moverse
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
    public void CheckDeath()
    {
        if (health <= 0) {
            //Actualizar nodo
            Node deathNode = Pathfinding.grid.NodeFromWorldPoint(transform.position);
            if (deathNode != null)
            {
                deathNode.walkable = true;
                deathNode.hasUnit = false;
            }

            StartCoroutine(DestroyAfterDelay(0.1f));
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    public void CheckEnemyUnitsInNeighbours()
    {
        // Nodo actual de la unidad
        Node currentNode = grid.NodeFromWorldPoint(transform.position);

        // Verifica si el nodo actual no es nulo
        if (currentNode != null)
        {
            List<Node> neighbours = grid.GetNeighbours(currentNode);

            foreach (Node neighbour in neighbours)
            {
                // Verifica si hay una unidad en el nodo y si la etiqueta es "EnemyUnit1"
                if (neighbour != null && neighbour.hasUnit && neighbour.unit != null && neighbour.unit.tag == "EnemyUnit1")
                {
                    Debug.Log("TIENE ENEMIGOS VECINOS");
                    canAttack = true;
                    playerTarget = neighbour.unit;
                }
            }
        }
    }

    public void AttackEnemyUnit(Unit enemyUnit){
        enemyUnit.health -= this.attackPower;
    }
}
