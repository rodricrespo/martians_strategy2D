using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuySoldier : BTNode
{
    private GameManager gm;
    private GameObject gameLogicObject;
    private GameObject pGrid;
    private Grid grid;

    public BuySoldier(BehaviourTree t) : base(t)
    {
        gameLogicObject = GameObject.Find("GameLogic");
        gm = gameLogicObject.GetComponent<GameManager>();

        pGrid = GameObject.Find("PGrid");
        grid = pGrid.GetComponent<Grid>();
    }

    public override Result Execute()
    {
        if (gm.AIresources < 10) {
            return Result.Failure;
        }
        else {
            if (gm.GetUnitCountWithTag("EnemyUnit1") >= 5) return Result.Failure;
            SetEnemySoldier();
            gm.AIresources -= 10;
            return Result.Success;
        }
    }

    public void SetEnemySoldier() {
        Vector3 randomWalkablePosition = grid.GetRandomWalkablePosition(); // Obtener una posición walkable aleatoria del grid
        Node node = grid.NodeFromWorldPoint(randomWalkablePosition);

        Instantiate(gm.enemySpaceshipPrefab, randomWalkablePosition, Quaternion.identity);  //Instancia la nave en pantalla
        node.walkable = false;
    }
}
