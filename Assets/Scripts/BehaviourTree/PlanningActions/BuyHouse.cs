using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyHouse : BTNode
{
    private GameManager gm;
    private GameObject gameLogicObject;

    public BuyHouse(BehaviourTree t) : base(t)
    {
        gameLogicObject = GameObject.Find("GameLogic");
        gm = gameLogicObject.GetComponent<GameManager>();
    }

    public override Result Execute()
    {
        Debug.Log("ENTRO EN BUY HOUSE");
        if (gm.AIresources < 100) return Result.Failure;
        else {
            gm.AIresourcesMultiplier = 2;
            gm.AIresources -= 100;
            return Result.Success;
        }
    }
}
