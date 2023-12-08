using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool isSelected = false;

    private GameManager gm;
    private GameObject gameLogicObject;

    void Start()
    {
        gameLogicObject = GameObject.Find("GameLogic");
        gm = gameLogicObject.GetComponent<GameManager>();
    }

    
    void Update()
    {

    }

    private void OnMouseDown() //selecciona o deselecciona unidad
    {
        if (!isSelected) {  //Selecciona
            gm.selectedUnit = this;
            isSelected = true;
            Debug.Log(this.ToString() + " Seleccionado");
            //if(this.tag == "`PlayerUnit1") GetWalkableTilesUnit1();
        }

        else{   //Deselecciona
            gm.selectedUnit = null;
            isSelected = false;
            Debug.Log(this.ToString() + " Deseeleccionado");
        }
    }
}
