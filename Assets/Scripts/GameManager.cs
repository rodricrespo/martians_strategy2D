using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int playerResources = 0;
    public int AIresources = 0;
    public int currentTurn = 0;
    public BehaviourTree behaviourTree;

    private List<Unit> enemyUnits = new List<Unit>();
    
    void Start()
    {
        currentTurn = 1;
    }

    
    void Update()
    {
        if (currentTurn == 2) return;

        if (Input.GetKeyDown(KeyCode.Space)) EndTurn();
        
    }

    public void EndTurn() {

        if (currentTurn == 1) {
            currentTurn = 2;
            StartCoroutine(AITurn());
        }
        else currentTurn = 1;
        UpdateResources();
    }

    public IEnumerator AITurn() //Las acciones que se tienen que hacer en el turno de la IA
    {
        yield return new WaitForSeconds(.1f);

        enemyUnits.Clear(); 
        foreach (Unit unit in FindObjectsOfType<Unit>())    //Recorremos todos las unidades que sean del tipo Unit
        {
            enemyUnits.Add(unit); //Añadimos a la lista  
        }

        StartCoroutine(behaviourTree.RunBehavior(behaviourTree.planningRoot));
        yield return new WaitForSeconds(.5f);

        StopAllCoroutines();

        // Then move the troops
        foreach (Unit unit in enemyUnits)
        {
            
            // if (!unit.isBlueKing)
            // {
            //     unit.uRoot = new BTRepeater(behaviorTree, new BTSelector(behaviorTree, new BTNode[] {
            //     new BTRepeater(behaviorTree, new BTSequencer(behaviorTree, new BTNode [] { new BTCheckRange(behaviorTree, unit), new BTAttackUnit(behaviorTree, unit)})),
            //     new BTRepeater(behaviorTree, new BTSequencer(behaviorTree, new BTNode[] { new BTMoveUnit(behaviorTree, unit), new BTWaitNode(behaviorTree), new BTCheckRange(behaviorTree, unit), new BTAttackUnit(behaviorTree, unit) })) }));
            // }

            // else
            // {
            //     unit.uRoot = new BTRepeater(behaviorTree, new BTSelector(behaviorTree, new BTNode[] {
            //     new BTRepeater(behaviorTree, new BTSequencer(behaviorTree, new BTNode [] { new BTCheckRange(behaviorTree, unit), new BTAttackUnit(behaviorTree, unit)})),
            //     new BTRepeater(behaviorTree, new BTSequencer(behaviorTree, new BTNode[] { new BTFleeKing(behaviorTree, unit), new BTWaitNode(behaviorTree), new BTCheckRange(behaviorTree, unit), new BTAttackUnit(behaviorTree, unit) })) }));
            // }

            //StartCoroutine(behaviorTree.RunBehavior(unit.unitRoot));
            

            //StartCoroutine(unit.Act());
            yield return new WaitForSeconds(1.75f);
        }

        EndTurn();
    }

    public void UpdateResources() {
        playerResources += 5;
        AIresources += 5;
    }
}
