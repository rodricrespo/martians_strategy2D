using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    public Node node;
    public Color lightColor = Color.black;
    public bool isWalkable;

    private GameManager gm;
    private GameObject gameLogicObject;
    private SpriteRenderer rend;
    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();

        gameLogicObject = GameObject.Find("GameLogic");
        gm = gameLogicObject.GetComponent<GameManager>();

        node = Pathfinding.grid.NodeFromWorldPoint(transform.position);
        isWalkable = false; // Se actualizará cuando se clicke en una undidad
    }

    public bool isClear() //Si tiene obstaculo
    {
        return node.walkable && !node.hasPlant;
    }
    public void LightUp() {
		
        rend.color = lightColor;
        isWalkable = true;
    }

    public void Reset()
    {
        rend.color = Color.white; //Se pone con su color normal
        isWalkable = false;
    }

        private void OnMouseDown()
    {
        if (gm.currentTurn == 2) return; //No nos va a hacer falta

        //Debug.Log(isWalkable);

        if (gm.selectedUnit != null && isWalkable && node.walkable)
        {
            gm.selectedUnit.MoveUnit(node);
        }
    }

}