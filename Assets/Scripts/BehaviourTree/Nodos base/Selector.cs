using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : BTNode
{
    public List<BTNode> Children { get; }  //La lista de hijos

    private int actualNode = 0;

    public Selector(BehaviourTree tree, BTNode[] nodes) : base(tree)
    {
        Children = new List<BTNode>(nodes); //Cosntructor
    }


    public override Result Execute()    //Resumen:  si un nodo hijo falla, pasa al siguiente y así sucesivamente hasta encontrar un nodo que tenga éxito o hasta que todos los nodos hayan fallado
    {
        if (actualNode < Children.Count)
        {
            Result result = Children[actualNode].Execute();


            if (result == Result.Running) return Result.Running;
            else if (result == Result.Failure){
                actualNode++;
                if (actualNode < Children.Count)
                    return Result.Running;
                else {
                    actualNode = 0;
                    return Result.Failure;
                }
            }
            else return Result.Success;
            
        }
        return Result.Failure;
    }
}

