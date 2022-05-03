using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Destination : MonoBehaviour
{
    public Vector3Int cellPosition;

    public void SetCellPosition(Tilemap map, Vector3Int cellPos)
    {
        cellPosition = cellPos;
        Vector3 destPos = map.GetCellCenterWorld(cellPos);
        this.transform.localPosition = destPos;
    }

}