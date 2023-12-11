using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPowerup : BTNode
{
    private Unit unit;
    private BehaviourTree behaviourTree;
    private GameManager gm;

    public AttackPowerup(BehaviourTree t, Unit _unit, GameManager _gm) : base(t)
    {
        behaviourTree = t;
        unit = _unit; 
        gm = _gm;
    }

    public override Result Execute()
    {
        // Verifica si la clave "PowerupObject" está presente en el diccionario
        if (behaviourTree.Blackboard2.ContainsKey("PowerupObject"))
        {
            Powerup powerup = (Powerup)behaviourTree.Blackboard2["PowerupObject"];
            if (powerup != null)
            {
                if (powerup.powerupHealth <= 0)
                    return Result.Failure;

                powerup.powerupHealth -= unit.attackPower;
                // SE LE QUITA VIDA (DEPENDE DE QUIEN LE ATAQUE)
                return Result.Success;
            }
            else return Result.Failure;  
        }
        else return Result.Failure; // La clave no está presente en el diccionario
    }
}