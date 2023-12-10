using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyPowerup : BTNode
{
    private GameManager gm;
    private GameObject gameLogicObject;

    public BuyPowerup(BehaviourTree t) : base(t)
    {
        gameLogicObject = GameObject.Find("GameLogic");
        gm = gameLogicObject.GetComponent<GameManager>();
    }

    public override Result Execute()
    {
        //Debug.Log("ENTRO EN BUY POWERUP");
        if (gm.AIresources < 50) return Result.Failure;
        else {
            if (gm.currentEnemyStrategy == GameManager.Strategy.Aggressive) SetAttackPowerup();  //Si está en estrategia agresiva
            if (gm.currentEnemyStrategy == GameManager.Strategy.Defensive) SetLockPowerup();     //Para estategia defensiva
            if (gm.currentEnemyStrategy == GameManager.Strategy.Normal) SetResourcesPowerup();
            gm.AIresources -= 50;
            return Result.Success;
        }
    }

    private void SetAttackPowerup()
    {
        Vector3 randomPosition = gm.grid.GetRandomWalkablePositionInHalf(-gm.grid.gridWorldSize.x / 4);
        Node node = gm.grid.NodeFromWorldPoint(randomPosition);
        if (randomPosition != Vector3.zero)
        {
            // Crear una instancia de gm.powerupPrefab en la posición obtenida
            GameObject powerupInstance = Instantiate(gm.enemyPowerup1Prefab, randomPosition, Quaternion.identity);
            gm.AIpowerMultiplier += 1;
            node.walkable = false;
        }
    }

    
    private void SetResourcesPowerup()
    {
        //Debug.Log("compro POWERUP");
        Vector3 randomPosition = gm.grid.GetRandomWalkablePositionInHalf(-gm.grid.gridWorldSize.x / 4);
        Node node = gm.grid.NodeFromWorldPoint(randomPosition);
        if (randomPosition != Vector3.zero)
        {
            // Crear una instancia de gm.powerupPrefab en la posición obtenida
            GameObject powerupInstance = Instantiate(gm.enemyPowerup2Prefab, randomPosition, Quaternion.identity);
            gm.AIresourcesMultiplier += 1;
            node.walkable = false;
        }
    }

    private void SetLockPowerup()
    {
        Vector3 randomPosition = gm.grid.GetRandomWalkablePositionInHalf(-gm.grid.gridWorldSize.x / 4);
        Node node = gm.grid.NodeFromWorldPoint(randomPosition);
        if (randomPosition != Vector3.zero)
        {
            // Crear una instancia de gm.powerupPrefab en la posición obtenida
            GameObject powerupInstance = Instantiate(gm.enemyPowerup3Prefab, randomPosition, Quaternion.identity);
            gm.playerResourcesMultiplier = 0;   //Anula el conseguir recursos
            gm.playerPowerMultiplier = 1;
            node.walkable = false;
        }
    }
    
}
