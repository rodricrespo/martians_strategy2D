using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : BTNode
{
    private Unit unit;
    private BehaviourTree behaviourTree;
    private GameManager gm;
    private List<Vector3> path;
    private Node currentNode;
    private Grid grid;

    public MoveEnemy(BehaviourTree t, Unit _unit, Grid g) : base(t)
    {
        unit = _unit;
        behaviourTree = t;
        grid = g;
    }

    public override Result Execute()
    {
        // Obtener la información del enemigo del Blackboard
        Unit enemyUnit = (Unit)behaviourTree.Blackboard["enemyUnit"];

        //obtener el target node y moverse
        Node targetNode = grid.NodeFromWorldPoint(enemyUnit.transform.position);

        // Verificar si se encontró un nodo válido
        if (targetNode != null)
        {
            // Mover la unidad hacia el nodo más cercano
            unit.MoveToNode(targetNode);
            return Result.Success;
        }
        else return Result.Failure;
        
    }
}
