using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPowerupInRange : BTNode
{
    private GameManager gm;
    private BehaviourTree behaviourTree;
    private GameObject powerup;
    private Unit unit;

    public CheckPowerupInRange(BehaviourTree t, GameManager _gm, Unit _unit, GameObject gameObject) : base(t)
    {
        gm = _gm;  
        behaviourTree = t;
        unit = _unit;
        powerup = gameObject;  
    }

    public override Result Execute()
    {
        // Verificar si el powerup existe
        if (powerup != null)
        {
            if (IsWithinRange(powerup.transform.position, unit.transform.position, unit.unitRange)){
                Debug.Log("enemigos cerca del powerup");
                behaviourTree.Blackboard2["PowerupObject"] = powerup;
                return Result.Success;
            }
            return Result.Failure;
        }
        else
        {
            return Result.Failure;
        }
    }

    private bool IsWithinRange(Vector3 pointA, Vector3 pointB, float range)
    {
        float distance = Vector3.Distance(pointA, pointB);
        return distance <= range;
    }
}
