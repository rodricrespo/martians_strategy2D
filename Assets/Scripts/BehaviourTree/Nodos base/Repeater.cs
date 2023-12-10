using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeater : BTNode // Repite la accion de su hijo <-- Sin esto los nodos de acción solo realizan la accion una vez
{
    public BTNode Child {get;set;}

    public Repeater(BehaviourTree tree, BTNode child) : base(tree){
        Child = child;
    }

    public override Result Execute() {
        return Child.Execute();
    }
}
