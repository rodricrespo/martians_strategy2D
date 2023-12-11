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

    public MoveEnemy(BehaviourTree t, Unit _unit, Grid g, GameManager _gm) : base(t)
    {
        unit = _unit;
        behaviourTree = t;
        grid = g;
        gm = _gm;
    }

     public override Result Execute()
    {
        if (gm.currentEnemyStrategy == GameManager.Strategy.Normal){

            // Verificar si 'enemyUnit' está presente en el Blackboard y es de tipo Unit
            if (behaviourTree.Blackboard!=null && behaviourTree.Blackboard.TryGetValue("enemyUnit", out Unit enemyUnit))
            {
                Node targetNode = grid.NodeFromWorldPoint(enemyUnit.transform.position);

                if (targetNode != null)
                {
                    if (unit!=null)unit.MoveToNode(targetNode);
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
                     if (unit!=null)unit.MoveToNode(randomNode);
                    return Result.Success;
                }
                else return Result.Failure; 
                
            }

        }

        else if (gm.currentEnemyStrategy == GameManager.Strategy.Defensive)
        {
            if (behaviourTree.Blackboard2 != null && behaviourTree.Blackboard2.TryGetValue("PowerupObject", out Powerup powerup))
            {
                Debug.Log("HACIA LA POSICION DEL NODO PARA DEFENDERLO");
                Node targetNode = grid.NodeFromWorldPoint(powerup.transform.position);

                if (targetNode != null)
                {
                     if (unit!=null)unit.MoveToNode(targetNode);
                    return Result.Success;
                }
                else return Result.Failure;
                
            }
            else return Result.Failure; //no hay powerup en el Blackboard2
        }

        else { //gm.currentEnemyStrategy == GameManager.Strategy.Defensive
            // if (behaviourTree.Blackboard2 != null && behaviourTree.Blackboard2.TryGetValue("PowerupObject", out Powerup powerup))
            // {
            //     Debug.Log("HACIA LA POSICION DEL NODO PARA ATACARLO");
            //     Node targetNode = grid.NodeFromWorldPoint(powerup.transform.position);

            //     if (targetNode != null)
            //     {
            //          if (unit!=null)unit.MoveToNode(targetNode);
            //         return Result.Success;
            //     }
            //     else return Result.Failure;
                
            // }
            // else
            // {
                // La clave 'PowerupObject' no está presente en el Blackboard
                Vector3 randomPosition = grid.GetRandomWalkablePosition();
                if (randomPosition != Vector3.zero)
                {
                    Node randomNode = grid.NodeFromWorldPoint(randomPosition);
                     if (unit!=null)unit.MoveToNode(randomNode);
                    return Result.Success;
                }
                else return Result.Failure;  
            //}
        } 
        
    }
}
