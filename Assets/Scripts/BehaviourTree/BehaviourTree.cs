using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : MonoBehaviour
{
    [HideInInspector]public BTNode planningRoot; 
    [HideInInspector]public BTNode unitRoot;    
    [HideInInspector]public BTNode strategyRoot;    

    public Dictionary<string, Unit> Blackboard { get; set; }
    public Dictionary<string, object> Blackboard2 { get; set; }

    void Awake()
    {
        InitializeBlackboard(); //Para usar datos dentro del árbol -> target, posiciones....
    }

    public IEnumerator RunBehavior(BTNode root)
    {  
        BTNode.Result result = root.Execute();

        while (result == BTNode.Result.Running)
        {
            yield return null;
            result = root.Execute();
        }
    }

    private void InitializeBlackboard()
    {
        Blackboard = new Dictionary<string, Unit>();
        Blackboard2 = new Dictionary<string, object>();
    }

    //LOS ARBOLES DE LAS UNDIADES ESTÁN EN GameManager PORQUE ALLI HAY REFERENCIAS A Unit
    //LOS ARBOLES DE ESTRATEGIA TMB PORQUE LAS REFERENCIAS A GM AQUI NO FUNCIONAN

}