using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : MonoBehaviour
{
    [HideInInspector]public BTNode planningRoot; //Nos da el árbol para plantear estrategias
    private BTNode unitRoot;    //Nos da el árbol para unidades

    //Puede haber más árboles...

    public Dictionary<string, object> Blackboard { get; set; }

    void Awake()
    {
        InitializeBlackboard(); //Para usar datos dentro del árbol
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
        //Blackboard = new Dictionary<string, object>();
        //Blackboard.Add("wb", new Rect(0, 0, 0, 0));
    }

    private void CreatePlanningRoot()
    {
        BTNode planningTree = new Repeater(
            this, new Sequencer (this, new BTNode[] { new SaveMoney(this),
                                                       new Repeater(this, new Selector(this, new BTNode[] { new BuySoldier(this)})) 
                                                     }
                                 )
        );

        planningRoot = planningTree;
    }

}