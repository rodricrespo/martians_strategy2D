using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMoney : BTNode
{
    private GameManager gm;
    private GameObject gameLogicObject;

    public SaveMoney(BehaviourTree t) : base(t)
    {
        //gm = GameObject.Find("GameLogic").GetComponent<GameManager>();
        gameLogicObject = GameObject.Find("GameLogic");
        gm = gameLogicObject.GetComponent<GameManager>();
    }

    public override Result Execute()
    {
        if (gm.AIresources < 30 || gm.GetEnemyUnitCount()>=5) return Result.Failure;
        else {
            
            return Result.Success;
        }
    }
}
