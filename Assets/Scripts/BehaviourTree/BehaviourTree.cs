using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree : MonoBehaviour
{
    public BTNode planningRoot; //Nos da el árbol para plantear estrategias
    private BTNode unitRoot;    //Nos da el árbol para unidades

    //Puede haber más árboles...

    public Dictionary<string, object> Blackboard { get; set; }

    void Awake()
    {
        InitializeBlackboard();
        CreatePlanningRoot();
        // CreateUnitRoot(); 
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
        Blackboard = new Dictionary<string, object>();
        Blackboard.Add("WorldBounds", new Rect(0, 0, 5, 5));
    }

    private void CreatePlanningRoot()
    {
        BTNode planningTree = new Repeater(
            this,
            new Sequencer(
                this,
                new BTNode[] {
                    //VA LA ESTRUCTURA DE NODOS
                }
            )
        );

        planningRoot = planningTree;
    }

}