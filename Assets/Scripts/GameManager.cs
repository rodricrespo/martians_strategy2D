using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{

    public enum Strategy
    {
        Normal,
        Aggressive,
        Defensive
    }
    public int powerupPrice = 35;   
    public int spaceshipPrice = 10;
    public int spaceshipPrice2 = 20;
    public int playerResources = 0;
    public int AIresources = 0;
    public int playerResourcesMultiplier = 1;
    public int playerPowerMultiplier = 1;
    public int AIresourcesMultiplier = 1;
    public int AIpowerMultiplier = 1;
    public int currentTurn = 0;
    public BehaviourTree behaviourTree;
    public GameObject enemySpaceshipPrefab;
    public GameObject enemySpaceshipPrefab2;
    public GameObject enemyPowerup1Prefab;
    public GameObject enemyPowerup2Prefab;
    public GameObject enemyPowerup3Prefab;
    public GameObject playerSpaceshipPrefab;
    public GameObject playerSpaceshipPrefab2;
    public GameObject playerPowerup1Prefab;
    public GameObject playerPowerup2Prefab;
    public GameObject playerPowerup3Prefab;
    public Grid grid;
    public Unit selectedUnit = null;
    public Pathfinding pathfinding;
    public Strategy currentEnemyStrategy = Strategy.Normal;
    
    void Start()
    {
        currentTurn = 1;
        UpdateNodesWithUnits(); //Primera pasada para determinar si hay nodos vecinos con unidades enemigas
        powerupPrice = 35;
        spaceshipPrice = 10;
        spaceshipPrice2 = 20;
    }

    
    void Update()
    {
        if (currentTurn == 2) return;

        if (Input.GetKeyDown(KeyCode.Space)) EndTurn();

        foreach (Unit unit in FindObjectsOfType<Unit>()) {
            unit.CheckDeath();
        }

        // foreach (Powerup p in FindObjectsOfType<Powerup>()) {
        //     p.CheckPowerupDeath();
        // }

        Debug.Log(currentEnemyStrategy);
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

        //1. DECIDIR ESTARTEGIA

        behaviourTree.strategyRoot = new Repeater ( 
                                        behaviourTree, new Selector (behaviourTree, new BTNode[] {  
                                                                                            new CheckAgressiveStrategy(behaviourTree, this),
                                                                                            new CheckDefensiveStrategy(behaviourTree, this),
                                                                                            new CheckNormalStrategy(behaviourTree, this),
                                                                                        }
                                                                   )
                                    );




        StartCoroutine(behaviourTree.RunBehavior(behaviourTree.strategyRoot));
        yield return new WaitForSeconds(.5f);   
        StopCoroutine(behaviourTree.RunBehavior(behaviourTree.strategyRoot));


        //2. PLANIFICAR UNIDADES

        behaviourTree.planningRoot = new Repeater  ( 
                                                    behaviourTree, new Selector (behaviourTree, new BTNode[] { new SaveMoney(behaviourTree),
                                                                                                               new Repeater( behaviourTree, new Selector(behaviourTree, new BTNode[] { 
                                                                                                                                                                                    new BuyPowerup(behaviourTree),
                                                                                                                                                                                    new BuySpaceship2(behaviourTree),
                                                                                                                                                                                    new BuySpaceship(behaviourTree)
                                                                                                    
                                                                                                                                                                                  }
                                                                                                                                                      )
                                                                                                                         )
                                                                                                        }
                                                                               )
                                                    );

        StartCoroutine(behaviourTree.RunBehavior(behaviourTree.planningRoot));
        yield return new WaitForSeconds(.5f);   
        StopCoroutine(behaviourTree.RunBehavior(behaviourTree.planningRoot));


        //3. MOVER UNIDADES
        foreach (Unit enemyUnit in FindObjectsOfType<Unit>()) //No se puede hacer antes porque tenemos que hacer lo del AI planning
        {
            
            if (enemyUnit.tag == "EnemyUnit1" || enemyUnit.tag == "EnemyUnit2"){
                if (currentEnemyStrategy==Strategy.Normal || currentEnemyStrategy==Strategy.Aggressive){
                    enemyUnit.unitRoot = 
                        new Repeater(behaviourTree, new Selector( behaviourTree, new  BTNode[] { 
                                                                                                  new Repeater (behaviourTree, new Sequencer (behaviourTree, new BTNode[] {
                                                                                                                                                                            new CheckEnemiesInRange(behaviourTree, enemyUnit),
                                                                                                                                                                            new MoveEnemy(behaviourTree, enemyUnit, grid, this),
                                                                                                                                                                            new Attack(behaviourTree, enemyUnit, this)
                                                                                                                                                                          }
                                                                                                                                             )
                                                                                                                ),
                                                                                                        new Sequencer (behaviourTree, new BTNode[] {    //Sobra el sequencer...
                                                                                                                                                        new MoveEnemy(behaviourTree, enemyUnit, grid, this) //A una posición cercana al enemigo  
                                                                                                                                                    }
                                                                                                        
                                                                                                                    )
                                                                
                                                                                            
                                                                                                }      
                                                                )
                                    );
                }
                else{
                    enemyUnit.unitRoot = new Repeater(behaviourTree, new Selector( behaviourTree, new  BTNode[] {
                                                                                                                    new Repeater( behaviourTree, new Selector (behaviourTree, new BTNode[]{
                                                                                                                                                                                            new Repeater (behaviourTree, new Sequencer (behaviourTree, new BTNode[] {
                                                                                                                                                                                                                                                                        new CheckPowerupInRange(behaviourTree, this, enemyUnit),
                                                                                                                                                                                                                                                                        new CheckEnemiesInRange(behaviourTree, enemyUnit),
                                                                                                                                                                                                                                                                        new Attack(behaviourTree, enemyUnit, this)
                                                                                                                                                                                                                                                                    }
                                                                                                                                                                                                                                        )
                                                                                                                                                                                                        ),
                                                                                                                                                                                            new MoveEnemy(behaviourTree, enemyUnit, grid, this) //MOVER HACIA POWERUP
                                                                                                                                                                                        }
                                                                                                                                                            )
                                                                                                                                 ),
                                                                                                                    new Repeater( behaviourTree, new Sequencer (behaviourTree, new BTNode[] {
                                                                                                                                                                                                new Retract (behaviourTree, enemyUnit, grid, this), //REPLIEGUE
                                                                                                                                                                                                new CheckEnemiesInRange(behaviourTree, enemyUnit),  //SOLO ATACARÁ ENEMIGOS SI ESTÁ REPLEGADO
                                                                                                                                                                                                new Attack(behaviourTree, enemyUnit, this)
                                                                                                                                                                                            }
                                                                                                                                                                )
                                                                                                                                                                    
                                                                                                                                )
                                                                                                                }
                                                                                 )
                                                     );
                }
                // else // Strategy.Aggressive
                // {
                //     enemyUnit.unitRoot = new Repeater(behaviourTree, new Selector( behaviourTree, new  BTNode[] {
                //                                                                                                     new Repeater( behaviourTree, new Selector (behaviourTree, new BTNode[]{
                //                                                                                                                                                                                 new CheckPowerupInRange(behaviourTree, this, enemyUnit),
                //                                                                                                                                                                                 new AttackPowerup(behaviourTree, enemyUnit, this)
                //                                                                                                                                                                           }
                //                                                                                                                                               )
                //                                                                                                                 ),
                //                                                                                                                 new MoveEnemy(behaviourTree, enemyUnit, grid, this), //MOVER HACIA POWERUP
                //                                                                                                     new Repeater (behaviourTree, new Sequencer (behaviourTree, new BTNode[]{
                //                                                                                                                                                                                 new CheckEnemiesInRange(behaviourTree, enemyUnit),
                //                                                                                                                                                                                 new Attack(behaviourTree, enemyUnit, this)
                //                                                                                                                                                                             }
                //                                                                                                                                                )
                                                                                                                                                
                //                                                                                                                  ), new MoveEnemy(behaviourTree, enemyUnit, grid, this)
                //                                                                                                 }
                //                                                                  )
                //                                      );
                // }

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
                Powerup p = FindPowerupInNode(node);
                node.unit = unitInNode;
                node.powerup = p;
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

    private Powerup FindPowerupInNode(Node node)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(node.worldPosition, grid.nodeRadius);

        foreach (var collider in colliders)
        {
            Powerup p = collider.GetComponent<Powerup>();
            if (p != null)
            {
                return p;
            }
        }

        return null;
    }

    public int GetUnitCountWithTag(string tag)
    {
        // Obtener todas las unidades con la etiqueta especificada
        Unit[] units = FindObjectsOfType<Unit>().Where(unit => unit.tag == tag).ToArray();
        return units.Length;
    }


    
}
