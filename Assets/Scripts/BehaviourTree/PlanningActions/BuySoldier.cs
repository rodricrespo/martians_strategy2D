using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuySoldier : BTNode
{
    private GameManager gm;
    private GameObject gameLogicObject;

    public BuySoldier(BehaviourTree t) : base(t)
    {
        gameLogicObject = GameObject.Find("GameLogic");
        gm = gameLogicObject.GetComponent<GameManager>();
    }

    public override Result Execute()
    {
        return Result.Failure;
    }
}
