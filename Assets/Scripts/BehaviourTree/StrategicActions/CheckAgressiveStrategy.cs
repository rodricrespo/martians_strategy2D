using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAgressiveStrategy : BTNode
{
    private BehaviourTree behaviourTree;
    private GameManager gm;

    public CheckAgressiveStrategy(BehaviourTree tree, GameManager _gm) : base(tree)
    {
        behaviourTree = tree;
        gm = _gm;
    }

    public override Result Execute()
    {
        // //SUPERIORIDAD NUMÉRICA (QUE HAYA UNA DIFERENCIA SIGNIFICATIVA, POR LO MENOS 2 UNIDADES)
        if ((gm.GetUnitCountWithTag("EnemyUnit1") + gm.GetUnitCountWithTag("EnemyUnit2")) - (gm.GetUnitCountWithTag("PlayerUnit1") + gm.GetUnitCountWithTag("PlayerUnit2")) > 2 ) {
            gm.currentEnemyStrategy = GameManager.Strategy.Aggressive;
            return Result.Success;
        }
        // MÁS CANTIDAD DE RECURSOS QUE EL CONTRARIO
        if(gm.AIresources - gm.playerResources >= 25) {
            gm.currentEnemyStrategy = GameManager.Strategy.Aggressive;
            return Result.Success;
        }

        //QUE LOS ENEMIGOS TENGAN UNA MEDIA DE VIDA BAJA

        //SI EL JUGADOR HA COMPRADO Powerup de BLOQUEO (LOCK)
        if (gm.AIresourcesMultiplier == 0) {
            gm.currentEnemyStrategy = GameManager.Strategy.Aggressive;
            return Result.Success;
        }
        return Result.Failure; 
             
    }
}
