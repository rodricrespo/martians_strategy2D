using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckNormalStrategy : BTNode
{
    private BehaviourTree behaviourTree;
    private GameManager gm;

    public CheckNormalStrategy(BehaviourTree tree, GameManager _gm) : base(tree)
    {
        behaviourTree = tree;
        gm = _gm;
    }

    public override Result Execute()
    {
        gm.currentEnemyStrategy = GameManager.Strategy.Normal;
        return Result.Success; 
             
    }
}
