using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode: MonoBehaviour
{
    public int x;
    public int z;

    public int gCost = 0;
    public int hCost = 0;
    public int fCost = 0;

    public PathNode cameFromNode;

    public string status = "free";  // free, blocked, effect_fire ?

    public Material original_material;

    public PathNode(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public void calculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + ", " + z;
    }
}
