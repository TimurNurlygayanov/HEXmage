using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid
{
    public int width;
    public int height;

    public PathNode[,] gridArray;

    public HexGrid(int width, int height)
    {
        this.width = width;
        this.height = height;

        gridArray = new PathNode[width, height];
    }
}
