using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : MonoBehaviour
{
    [HideInInspector]public BTNode planningRoot; //Nos da el árbol para plantear estrategias
    [HideInInspector]public BTNode unitRoot;    //Nos da el árbol para unidades
    [HideInInspector]public BTNode strategyRoot;    //Nos da el árbol para unidades

    public Dictionary<string, Unit> Blackboard { get; set; }

    void Awake()
    {
        InitializeBlackboard(); //Para usar datos dentro del árbol -> target, posiciones....
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
        BTNode planningTree = new Repeater ( 
                                            this, new Selector (this, new BTNode[] {  new SaveMoney(this),
                                                                                new Repeater( 
                                                                                              this, new Selector(this, new BTNode[] { 
                                                                                                                                    new BuyPowerup(this),
                                                                                                                                    new BuySpaceship2(this),
                                                                                                                                    new BuySpaceship(this)
                                                                                                    
                                                                                                                                    }
                                                                                                                )
                                                                                            )
                                                                              }
                                                         )
                                           );

        planningRoot = planningTree;
    }

    //LOS ARBOLES DE LAS UNDIADES ESTÁN EN GameManager PORQUE ALLI HAY REFERENCIAS A Unit
    //LOS ARBOLES DE ESTRATEGIA TMB PORQUE LAS REFERENCIAS A GM AQUI NO FUNCIONAN

}