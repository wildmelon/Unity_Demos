
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    Road, Water
}

public class TileNode
{
    public NodeType nodeType = NodeType.Road;
    public Vector3Int position;
    public HashSet<TileNode> neighbors = new HashSet<TileNode>();
    public int cost;

    public void AddNeighBor(Dictionary<Vector3Int, TileNode> totals, Vector3Int neightborPos)
    {
        if (totals.TryGetValue(neightborPos, out TileNode neightModel))
        {
            neighbors.Add(neightModel);
        }
    }
}
