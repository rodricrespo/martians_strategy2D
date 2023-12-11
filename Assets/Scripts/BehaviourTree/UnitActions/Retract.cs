using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retract : BTNode
{
    private Unit unit;
    private BehaviourTree behaviourTree;
    private GameManager gm;
    private List<Vector3> path;
    private Node currentNode;
    private Grid grid;

    public Retract(BehaviourTree t, Unit _unit, Grid g, GameManager _gm) : base(t)
    {
        unit = _unit;
        behaviourTree = t;
        grid = g;
        gm = _gm;
    }

    public override Result Execute()
    {
        Debug.Log("RETIRADA ENEMIGA");
        Vector3 leftmostPosition = grid.GetLeftmostPosition(); // Obtener la posición más izquierda del grid
        Node leftmostNode = grid.NodeFromWorldPoint(leftmostPosition);

        if (leftmostNode != null && leftmostNode.walkable)
        {
            // Mover la unidad al nodo más izquierda
            unit.MoveToNode(leftmostNode);
            return Result.Success;
        }

        return Result.Failure;
    }
}
