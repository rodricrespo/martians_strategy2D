using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : BTNode
{
    private Unit unit;
    private BehaviourTree behaviourTree;

    public Attack(BehaviourTree t, Unit _unit) : base(t)
    {
        behaviourTree = t;
        unit = _unit; 
    }

    public override Result Execute(){

        Unit enemyUnit = (Unit)behaviourTree.Blackboard["enemyUnit"];

        if (enemyUnit != null)
            {
                enemyUnit.health -= unit.attackPower;   //SE LE QUITA VIDA (DEPENDE DE QUIEN LE ATAQUE)
                return Result.Success;
            }
        else return Result.Failure;
            
    }
}
