using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public int playerResources = 0;
    public int AIresources = 0;
    public int AIHouses = 0;
    public int playerHouses = 0;
    public int playerResourcesMultiplier = 1;
    public int playerPowerMultiplier = 1;
    public int AIresourcesMultiplier = 1;
    public int AIpowerMultiplier = 1;
    public int currentTurn = 0;
    public BehaviourTree behaviourTree;
    public GameObject enemySpaceshipPrefab;
    public GameObject enemyPowerup1Prefab;
    public GameObject enemyPowerup2Prefab;
    public GameObject enemyPowerup3Prefab;
    public GameObject playerSpaceshipPrefab;
    public GameObject playerPowerup1Prefab;
    public GameObject playerPowerup2Prefab;
    public GameObject playerPowerup3Prefab;
    public Grid grid;
    public Unit selectedUnit = null;
    public Pathfinding pathfinding;
    
    void Start()
    {
        currentTurn = 1;
        UpdateNodesWithUnits(); //Primera pasada para determinar si hay nodos vecinos con unidades enemigas
    }

    
    void Update()
    {
        if (currentTurn == 2) return;

        if (Input.GetKeyDown(KeyCode.Space)) EndTurn();

        foreach (Unit unit in FindObjectsOfType<Unit>()) {
            unit.CheckDeath();
        }

        Debug.Log(AIresourcesMultiplier);
    }

    public void EndTurn() {
        if (currentTurn == 1) {
            currentTurn = 2;
            StartCoroutine(AITurn());
            StopCoroutine(AITurn());
        }
        else currentTurn = 1;
        UpdateResources();
        UpdateNodesWithUnits();
        ResetUnits();
        ResetTiles();
    }

    public IEnumerator AITurn() //Las acciones que se tienen que hacer en el turno de la IA
    {
        StartCoroutine(behaviourTree.RunBehavior(behaviourTree.planningRoot));
        yield return new WaitForSeconds(.5f);   //Esperamos un determinado tiempo y luego vamos con las unidades
        StopCoroutine(behaviourTree.RunBehavior(behaviourTree.planningRoot));

        foreach (Unit enemyUnit in FindObjectsOfType<Unit>()) //No se puede hacer antes porque tenemos que hacer lo del AI planning
        {
            if (enemyUnit.tag == "EnemyUnit1"){
                //saber la estrategia del enemigo (neutral, defensiva, agresiva) y luego generar el árbol en consecuencia
                enemyUnit.unitRoot = 
                    new Selector( behaviourTree, new  BTNode[] { 
                                                                 new Sequencer (behaviourTree, new BTNode[] {
                                                                                                                new CheckEnemiesInRange(behaviourTree, enemyUnit),
                                                                                                                new MoveEnemy(behaviourTree, enemyUnit, grid),
                                                                                                                new Attack(behaviourTree, enemyUnit, this)
                                                                                                            }
                                                                               ),
                                                                new Sequencer (behaviourTree, new BTNode[] {
                                                                                                                new MoveEnemy(behaviourTree, enemyUnit, grid) //A una posición cercana al enemigo
                                                                                                            }
                                                                
                                                                              )
                                                            
                                                                                         
                                                               }      
                                );

                StartCoroutine(behaviourTree.RunBehavior(enemyUnit.unitRoot));
                
                yield return new WaitForSeconds(1.75f);
            }
        }

        EndTurn();
    }

    public void UpdateResources() {
        playerResources += 5 * playerResourcesMultiplier;
        AIresources += 5 * AIresourcesMultiplier;
    }

    public void ResetUnits() {
        Unit[] units = FindObjectsOfType<Unit>();
        foreach (Unit unit in units) {
            unit.isSelected = false;
            unit.hasMoved = false;
            unit.playerTarget = null;
        }
    }

    public void ResetTiles() {  
        Tiles[] tiles = FindObjectsOfType<Tiles>();
        foreach (Tiles tile in tiles)
        {
            tile.Reset();
        }
    }

    public void UpdateNodesWithUnits()
    {
        Node[,] gridNodes = grid.grid;

        for (int x = 0; x < grid.gridSizeX; x++)
        {
            for (int y = 0; y < grid.gridSizeY; y++)
            {
                Node node = gridNodes[x, y];

                // Verifica si hay una unidad en el nodo
                Unit unitInNode = FindUnitInNode(node);
                node.unit = unitInNode;
            }
        }
    }

    private Unit FindUnitInNode(Node node)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(node.worldPosition, grid.nodeRadius);

        foreach (var collider in colliders)
        {
            Unit unit = collider.GetComponent<Unit>();
            if (unit != null)
            {
                return unit;
            }
        }

        return null;
    }

    public int GetEnemyUnitCount()
    {
        // Obtener todas las unidades con la etiqueta "EnemyUnit1"
        Unit[] enemyUnits = FindObjectsOfType<Unit>().Where(unit => unit.tag == "EnemyUnit1").ToArray();
        return enemyUnits.Length;
    }


    
}
