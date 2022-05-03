
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ClickableTilemap : MonoBehaviour
{
    public delegate void OnCellClick(Vector3Int cellPos);
    public OnCellClick OnTileCellClick
    {
        get;
        set;
    }

    public ImwalkableTile imwalkableTile;
    public WalkableTile walkableTile;

    public Tilemap mTilemap;


    private void Awake()
    {
        mTilemap = GetComponent<Tilemap>();
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        // 鼠标点击位置 屏幕空间 -> 世界空间
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 鼠标点击位置 世界空间 -> Tilemap格子坐标
        Vector3Int cellPos = mTilemap.WorldToCell(mouseWorldPos);
        // 格子坐标超出 tilemap 物体边界
        if (!mTilemap.cellBounds.Contains(cellPos)) return;
        // 点击格子，在水和陆地互换
        TileBase tileBase = mTilemap.GetTile(cellPos);
        if (!(tileBase is WalkableTile) && !(tileBase is ImwalkableTile)) return;

        bool isWalkable = false;
        TileBase targetTile = imwalkableTile;
        if (tileBase is ImwalkableTile)
        {
            isWalkable = true;
            targetTile = walkableTile;
        }
        mTilemap.SetTile(cellPos, targetTile);
        OnTileCellClick?.Invoke(cellPos);
    }

    /// <summary>
    /// 根据 Tile 关卡网格，生成实际地块对象数据
    /// </summary>
    public Dictionary<Vector3Int, TileNode> GenerateTileModels()
    {
        Dictionary<Vector3Int, TileNode> results = new Dictionary<Vector3Int, TileNode>();
        BoundsInt.PositionEnumerator it = mTilemap.cellBounds.allPositionsWithin;
        it.Reset();
        while (it.MoveNext())
        {
            Vector3Int current = it.Current;
            TileNode model = new TileNode
            {
                nodeType = NodeType.Road,
                position = new Vector3Int(current.x, current.y, 0),
                cost = 1
            };
            TileBase tileBase = mTilemap.GetTile(current);
            if (tileBase is ImwalkableTile)
            {
                model.nodeType = NodeType.Water;
                model.cost = 99999;
            }
            results.Add(model.position, model);
        }

        foreach (var item in results)
        {
            TileNode model = item.Value;
            Vector3Int modelPos = model.position;
            
            // 右
            model.AddNeighBor(results, new Vector3Int(modelPos.x + 1, modelPos.y, 0));
            // 上
            model.AddNeighBor(results, new Vector3Int(modelPos.x, modelPos.y + 1, 0));
            // 下
            model.AddNeighBor(results, new Vector3Int(modelPos.x, modelPos.y - 1, 0));
            // 左
            model.AddNeighBor(results, new Vector3Int(modelPos.x - 1, modelPos.y, 0));
            
        }
        return results;
    }
}