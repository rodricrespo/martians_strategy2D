using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequencer : BTNode
{
    public List<BTNode> Children { get; }  //La lista de hijos

    private int actualNode = 0;
    public Sequencer(BehaviourTree tree, BTNode[] nodes) : base(tree)  //Cosntructor
    {
        Children = new List<BTNode>(nodes);
    }


    public override Result Execute()    //Resumen: todos los hijos han de dar SUCCESS para que la secuencia sea SUCCESS
    {
        if (actualNode < Children.Count)
        {
            Result result = Children[actualNode].Execute();
            if (result == Result.Running) Children[actualNode].Execute();
            else if (result == Result.Failure) {
                actualNode = 0;
                return Result.Failure;
            }
            else {
                actualNode++;
                if (actualNode < Children.Count)
                    return Result.Running;
                else
                {
                    actualNode = 0;
                    return Result.Success;
                }
            }
        }
        return Result.Success;
    }
}

