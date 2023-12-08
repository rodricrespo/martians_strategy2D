using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    private GameManager gm;
    private Grid grid;
    private GameObject pGrid;

    void Start()
    {
        gm = GetComponent<GameManager>();

        pGrid = GameObject.Find("PGrid");
        grid = pGrid.GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
