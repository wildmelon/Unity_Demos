
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
        // �����λ�� ��Ļ�ռ� -> ����ռ�
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // �����λ�� ����ռ� -> Tilemap��������
        Vector3Int cellPos = mTilemap.WorldToCell(mouseWorldPos);
        // �������곬�� tilemap ����߽�
        if (!mTilemap.cellBounds.Contains(cellPos)) return;
        // ������ӣ���ˮ��½�ػ���
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
    /// ���� Tile �ؿ���������ʵ�ʵؿ��������
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
            
            // ��
            model.AddNeighBor(results, new Vector3Int(modelPos.x + 1, modelPos.y, 0));
            // ��
            model.AddNeighBor(results, new Vector3Int(modelPos.x, modelPos.y + 1, 0));
            // ��
            model.AddNeighBor(results, new Vector3Int(modelPos.x, modelPos.y - 1, 0));
            // ��
            model.AddNeighBor(results, new Vector3Int(modelPos.x - 1, modelPos.y, 0));
            
        }
        return results;
    }
}