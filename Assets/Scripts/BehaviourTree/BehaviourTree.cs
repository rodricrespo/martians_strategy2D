using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : MonoBehaviour
{
    [HideInInspector]public BTNode planningRoot; //Nos da el árbol para plantear estrategias
    [HideInInspector]public BTNode unitRoot;    //Nos da el árbol para unidades
    

    //Puede haber más árboles...

    public Dictionary<string, Unit> Blackboard { get; set; }

    void Awake()
    {
        InitializeBlackboard(); //Para usar datos dentro del árbol
        CreatePlanningRoot();
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
    }

    private void CreatePlanningRoot()
    {
        BTNode planningTree = new Selector (this, new BTNode[] { new SaveMoney(this),
                                                                  new Selector(this, new BTNode[] { 
                                                                                                    new BuyPowerup(this),
                                                                                                    new BuySoldier(this)
                                                                                                    
                                                                                                  }
                                                                                      )
                                                                     
                                                                }
                                            );

        planningRoot = planningTree;
    }  

}