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
    public int AIresourcesMultiplier = 1;
    public int currentTurn = 0;
    public BehaviourTree behaviourTree;
    public GameObject enemySpaceshipPrefab;
    public GameObject playerSpaceshipPrefab;
    public Grid grid;
    public Unit selectedUnit = null;
    
    void Start()
    {
        currentTurn = 1;
    }

    
    void Update()
    {
        if (currentTurn == 2) return;

        if (Input.GetKeyDown(KeyCode.Space)) EndTurn();
    }

    public void EndTurn() {
        if (currentTurn == 1) {
            currentTurn = 2;
            StartCoroutine(AITurn());
            StopCoroutine(AITurn());
        }
        else currentTurn = 1;
        UpdateResources();
        ResetUnits();
        ResetTiles();
    }

    public IEnumerator AITurn() //Las acciones que se tienen que hacer en el turno de la IA
    {
        StartCoroutine(behaviourTree.RunBehavior(behaviourTree.planningRoot));
        yield return new WaitForSeconds(.5f);   //Esperamos un determinado tiempo y luego vamos con las unidades
        StopCoroutine(behaviourTree.RunBehavior(behaviourTree.planningRoot));

        foreach (EnemyUnit enemyUnit in FindObjectsOfType<EnemyUnit>()) //No se puede hacer antes porque tenemos que hacer lo del AI planning
        {
            //saber la estrategia del enemigo (neutral, defensiva, agresiva) y luego generar el árbol en consecuencia
            enemyUnit.unitRoot = new Repeater(
                behaviourTree, new Selector( behaviourTree, new  BTNode[] { 
                                                                            new Repeater( behaviourTree, new Sequencer (behaviourTree, new BTNode[] {
                                                                                                                                        new CheckEnemiesInRange(behaviourTree, enemyUnit)
                                                                                                                                        //new MoveEnemy(behaviourTree, enemyUnit),
                                                                                                                                        //new Attack(behaviourTree, enemyUnit)
                                                                                                                                    }
                                                                                                        )
                                                        
                                                                                    ) 
                                                                            }      
                                            )
            );

            StartCoroutine(behaviourTree.RunBehavior(enemyUnit.unitRoot));
            
            yield return new WaitForSeconds(1.75f);
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
        }

        EnemyUnit[] enUnits = FindObjectsOfType<EnemyUnit>();
        foreach (EnemyUnit enUnit in enUnits) {
            enUnit.hasMoved = false;
        }

    }

    public void ResetTiles() {  
        Tiles[] tiles = FindObjectsOfType<Tiles>();
        foreach (Tiles tile in tiles)
        {
            tile.Reset();
        }
    }
}
