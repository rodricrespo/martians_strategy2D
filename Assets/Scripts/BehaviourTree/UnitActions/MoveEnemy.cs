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
        string enemyName = (string)behaviourTree.Blackboard["enemyName"];
        Vector3 enemyPosition = (Vector3)behaviourTree.Blackboard["enemyPosition"];

        Debug.Log($"Info del target: {enemyName}, {enemyPosition}");

        //obtener el target node y moverse
        Node targetNode = grid.NodeFromWorldPoint(enemyPosition);

        // Verificar si se encontró un nodo válido
        if (targetNode != null)
        {
            // Mover la unidad hacia el nodo más cercano
            unit.MoveToNode(targetNode);
            return Result.Success; // Puedes considerar esto como un éxito si el movimiento se realiza correctamente
        }
        else
        {
            Debug.Log("NO LLEGA PELOTUDO");
            return Result.Failure; // Manejar el caso donde no se encontró un nodo válido (puede ser un manejo de error)
        }
    }
}
