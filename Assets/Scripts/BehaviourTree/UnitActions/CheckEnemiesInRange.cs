using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemiesInRange : BTNode
{
    private Unit unit;
    private BehaviourTree behaviourTree;

    public CheckEnemiesInRange(BehaviourTree t, Unit _unit) : base(t)
    {
        unit = _unit;
        behaviourTree = t;
    }

    public override Result Execute()
    {
        Unit[] allUnits = GameObject.FindObjectsOfType<Unit>();

        foreach (Unit otherUnit in allUnits)
        {
            // Verificar si la unidad es del jugador y está dentro del rango del enemigo
            if (otherUnit.tag == "PlayerUnit1" && IsWithinRange(otherUnit.transform.position, unit.transform.position, unit.unitRange))
            {
                behaviourTree.Blackboard["enemyUnit"] = (Unit)otherUnit; // Almacenar la información del enemigo en el Blackboard

                return Result.Success;
            }
        }

        // Debug.Log("Ninguna unidad del jugador dentro del rango.");
        return Result.Failure;
    }

    // Función para verificar si un punto está dentro de un rango dado
    private bool IsWithinRange(Vector3 pointA, Vector3 pointB, float range)
    {
        float distance = Vector3.Distance(pointA, pointB);
        return distance <= range;
    }
}
