using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDefensiveStrategy : BTNode
{
    private BehaviourTree behaviourTree;
    private GameManager gm;

    public CheckDefensiveStrategy(BehaviourTree tree, GameManager _gm) : base(tree)
    {
        behaviourTree = tree;
        gm = _gm;
    }

    public override Result Execute()
    {
        //INFERIORIDAD NUMÉRICA (QUE HAYA UNA DIFERENCIA SIGNIFICATIVA, POR LO MENOS 2 UNIDADES)
        if ( (gm.GetUnitCountWithTag("PlayerUnit1") + gm.GetUnitCountWithTag("PlayerUnit2")) - (gm.GetUnitCountWithTag("EnemyUnit1") + gm.GetUnitCountWithTag("EnemyUnit2")) > 2 ) {
            gm.currentEnemyStrategy = GameManager.Strategy.Defensive;
            return Result.Success;
        }
        //BAJA CANTIDAD DE RECURSOS 
        if(gm.playerResources - gm.AIresources >= 25) {
            gm.currentEnemyStrategy = GameManager.Strategy.Defensive;
            return Result.Success;
        }
        //SI EL JUGADOR HA COMPRADO Powerup de ATAQUE
        if (gm.playerPowerMultiplier > 1) {
            gm.currentEnemyStrategy = GameManager.Strategy.Defensive;
            return Result.Success;
        }

        return Result.Failure; 
             
    }
}
