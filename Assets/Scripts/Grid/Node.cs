using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public bool hasUnit;
    public bool hasPlant;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;
    public Unit unit;

    public int gCost;
    public int hCost;
    public float tacticalCost;
    public float influenceCost =0;
    public Node parent;
    int heapIndex;

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
        hasUnit = false;
        unit = null;
    }

    public float fCost
    {
        get
        {
            return gCost + hCost + tacticalCost + influenceCost;
        }

        // No set because we won't have to modify the fCost directly
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }

        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
