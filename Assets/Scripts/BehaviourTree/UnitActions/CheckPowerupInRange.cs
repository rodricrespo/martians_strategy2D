using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPowerupInRange : BTNode
{
    private GameManager gm;
    private BehaviourTree behaviourTree;
    private Unit unit;
    private Powerup powerup;

    public CheckPowerupInRange(BehaviourTree t, GameManager _gm, Unit _unit) : base(t)
    {
        gm = _gm;  
        behaviourTree = t;
        unit = _unit;
    }

    public override Result Execute()
    {
        Powerup[] allPowerups = GameObject.FindObjectsOfType<Powerup>();

        foreach (Powerup p in allPowerups)
        {
            // Verificar estrategia defensiva y que tenga powerup activo
            if (gm.currentEnemyStrategy==GameManager.Strategy.Defensive && p.tag == "EnemyPowerup" && IsWithinRange(p.transform.position, unit.transform.position, unit.unitRange))
            {
                Debug.Log("enemigos cerca del powerup");
                behaviourTree.Blackboard2["PowerupObject"] = (Powerup)powerup;
                return Result.Success;
            }

            // Verificar estrategia agresiva y que tenga el jugador tenga powerup de ataque activo
            if (gm.currentEnemyStrategy==GameManager.Strategy.Defensive && p.tag == "PlayerPowerup1" && IsWithinRange(p.transform.position, unit.transform.position, unit.unitRange)){
                behaviourTree.Blackboard2["PowerupObject"] = (Powerup)powerup;
                return Result.Success;
            }

            if (IsWithinRange(p.transform.position, unit.transform.position, unit.unitRange)){
                behaviourTree.Blackboard2["PowerupObject"] = (Powerup)powerup;
                return Result.Success;
            }
        }
        return Result.Failure;
    }

    private bool IsWithinRange(Vector3 pointA, Vector3 pointB, float range)
    {
        float distance = Vector3.Distance(pointA, pointB);
        return distance <= range;
    }
}
