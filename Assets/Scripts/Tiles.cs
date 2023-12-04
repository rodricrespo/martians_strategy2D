using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    private SpriteRenderer rend;
    public Sprite[] tileGraphics;
    public float hoverAmount;
    public LayerMask obstacleLayer;
    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        int randTile = Random.Range(0, tileGraphics.Length);
        rend.sprite = tileGraphics[randTile];
    }
    private void OnMouseEnter()
    {
        transform.localScale += Vector3.one * hoverAmount;
    }
    private void OnMouseExit()
    {
        transform.localScale -= Vector3.one * hoverAmount;
    }
    public bool IsClear()
    {
        Collider2D obstacle = Physics2D.OverlapCircle(transform.position,0.2f,obstacleLayer);
        if(obstacle != null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}

