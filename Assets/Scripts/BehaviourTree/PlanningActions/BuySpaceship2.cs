using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuySpaceship2 : BTNode
{
    private GameManager gm;
    private GameObject gameLogicObject;
    private GameObject pGrid;
    private Grid grid;

    public BuySpaceship2(BehaviourTree t) : base(t)
    {
        gameLogicObject = GameObject.Find("GameLogic");
        gm = gameLogicObject.GetComponent<GameManager>();

        pGrid = GameObject.Find("PGrid");
        grid = pGrid.GetComponent<Grid>();
    }

    public override Result Execute()
    {
        //Debug.Log("ENTRO A COMPRAR UNA NAVE GRANDE");
        if (gm.AIresources < gm.spaceshipPrice2) {
            return Result.Failure;
        }
        else {
            if (gm.GetUnitCountWithTag("EnemyUnit2") > 2) return Result.Failure;
            SetEnemySpaceship2();
            gm.AIresources -= gm.spaceshipPrice2;
            return Result.Success;
        }
    }

    public void SetEnemySpaceship2() {
        Vector3 randomWalkablePosition = grid.GetRandomWalkablePosition(); // Obtener una posición walkable aleatoria del grid
        Node node = grid.NodeFromWorldPoint(randomWalkablePosition);

        Instantiate(gm.enemySpaceshipPrefab2, randomWalkablePosition, Quaternion.identity);  //Instancia la nave en pantalla
        node.walkable = false;
    }
}