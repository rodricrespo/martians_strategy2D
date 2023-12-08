using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int playerResources = 0;
    public int AIresources = 0;
    public int AIHouses = 0;
    public int playerHouses = 0;
    public int playerResourcesMultiplier = 1;
    public int AIresourcesMultiplier = 1;
    public int currentTurn = 0;
    public BehaviourTree behaviourTree;
    public GameObject enemySpaceshipPrefab;
    public GameObject playerSpaceshipPrefab;
    public Grid grid;
    public Unit selectedUnit = null;

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

        foreach (Unit unit in enemyUnits)
        {
            
            // HACER EL ARBOL DE LAS UNDIADES AQUI, O EN BehaviourTree
            //StartCoroutine(behaviorTree.RunBehavior(unit.unitRoot));
            
            yield return new WaitForSeconds(1.75f);
        }

        EndTurn();
    }

    public void UpdateResources() {
        playerResources += 5 * playerResourcesMultiplier;
        AIresources += 5 * AIresourcesMultiplier;
    }
}
