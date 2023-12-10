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
        // Verificar si 'enemyUnit' está presente en el Blackboard y es de tipo Unit
        if (behaviourTree.Blackboard.TryGetValue("enemyUnit", out Unit enemyUnit))
        {
            Node targetNode = grid.NodeFromWorldPoint(enemyUnit.transform.position);

            if (targetNode != null)
            {
                unit.MoveToNode(targetNode);
                return Result.Success;
            }
            else return Result.Failure; // Manejo de error si no se encuentra un nodo válido
        }

        else
        {
            // La clave 'enemyUnit' no está presente en el Blackboard
            Vector3 randomPosition = grid.GetRandomWalkablePosition();
            if (randomPosition != Vector3.zero)
            {
                Node randomNode = grid.NodeFromWorldPoint(randomPosition);
                unit.MoveToNode(randomNode);
                return Result.Success;
            }
            else return Result.Failure; 
            
        }
    }
}
