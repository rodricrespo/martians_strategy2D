using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BTNode : MonoBehaviour     //De esta clase heredarán el resto de nodos
{
    public enum Result { Running, Failure, Success };

    public BehaviorTree Tree { get; }

    public BTNode(BehaviorTree tree)
    {
        Tree = tree;
    }

    public virtual Result Execute() //Devuelve un RESULT, lo tienen que sobrescribir las clases que hereden
    {
        return Result.Failure;  //Default
    }
}
