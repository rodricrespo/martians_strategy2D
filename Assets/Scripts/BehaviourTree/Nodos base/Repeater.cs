using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeater : BTNode    //Hereda directamente de BTNode. En general es un decorador, que repite la ejecución de su hijo.
{
    public BTNode Child { get; set; }

    public Repeater(BehaviourTree tree, BTNode child) : base(tree)
    {
        Child = child;
    }

    public override Result Execute()
    {
        return Child.Execute();
    }
}


