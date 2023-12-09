using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    public bool hasMoved = false;
    public BTNode unitRoot;
    public int enemyUnitRange;

    private GameManager gm;
    private Grid grid;
    private GameObject pGrid;
    private GameObject gameLogic;
    private BehaviourTree bt;

    void Start()
    {
        gm = GetComponent<GameManager>();

        pGrid = GameObject.Find("PGrid");
        grid = pGrid.GetComponent<Grid>();

        gameLogic = GameObject.Find("GameLogic");
        bt = gameLogic.GetComponent<BehaviourTree>();

        unitRoot = bt.unitRoot;

        if(this.tag == "EnemyUnit1") enemyUnitRange = 3;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(enemyUnitRange);
    }
}
