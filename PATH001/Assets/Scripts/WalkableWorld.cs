using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WalkableWorld : MonoBehaviour
{

    //public Agent mAgent;
    public ClickableTilemap mClickableTilemap;
    public Character character;
    public Destination destination;

    private Dictionary<Vector3Int, TileNode> _totalTileModels;
    Dictionary<Vector3Int, TileNode> TotalTileModels
    {
        get
        {
            if (_totalTileModels == null)
            {
                // 根据 Tile 关卡网格，生成实际地块对象数据
                _totalTileModels = mClickableTilemap.GenerateTileModels();
            }
            return _totalTileModels;
        }
    }

    private void Awake()
    {
        // 设置 Tilmap 点击回调
        mClickableTilemap.OnTileCellClick = OnTileCellClick;
        // 给角色和目标设置初始位置
        Tilemap tilemap = mClickableTilemap.mTilemap;
        character.SetCellPosition(tilemap, new Vector3Int(4, 0, 0));
        destination.SetCellPosition(tilemap, new Vector3Int(4, 2, 0));
        // 寻路
        StartCoroutine(AStar());
    }

    private void OnTileCellClick(Vector3Int cellPos)
    {
        //Vector3 cellWorldCenterPos = mClickableTilemap.mTilemap.GetCellCenterWorld(cellPos);
        //character.MoveTo(cellWorldCenterPos);
    }

    private IEnumerator BFS()
    {
        Queue<TileNode> frontier = new Queue<TileNode>();
        Dictionary<TileNode, TileNode> cameFrom = new Dictionary<TileNode, TileNode>();
        TileNode start = TotalTileModels[character.cellPosition];
        TileNode target = TotalTileModels[destination.cellPosition];
        frontier.Enqueue(start);
        cameFrom.Add(start, null);
        mClickableTilemap.mTilemap.SetColor(start.position, Color.red);
        while (frontier.Count != 0)
        {
            TileNode current = frontier.Dequeue();
            // 到达目标，提前退出
            if (current == target) break;
            foreach (TileNode neighbor in current.neighbors)
            {
                if (neighbor.nodeType == NodeType.Road && !cameFrom.ContainsKey(neighbor))
                {
                    frontier.Enqueue(neighbor);
                    cameFrom.Add(neighbor, current);
                    mClickableTilemap.mTilemap.SetColor(neighbor.position, Color.blue);
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
        while (cameFrom[target] != null)
        {
            mClickableTilemap.mTilemap.SetColor(target.position, Color.red);
            target = cameFrom[target];
        }
    }

    private IEnumerator Dijkstra()
    {
        PriorityQueue frontier = new PriorityQueue();
        Dictionary<TileNode, TileNode> cameFrom = new Dictionary<TileNode, TileNode>();
        Dictionary<TileNode, int> costSoFar = new Dictionary<TileNode, int>();

        TileNode start = TotalTileModels[character.cellPosition];
        TileNode target = TotalTileModels[destination.cellPosition];

        frontier.Push(start, 0);
        costSoFar.Add(start, 0);
        cameFrom.Add(start, null);

        while (frontier.GetCount() != 0)
        {
            TileNode current = (TileNode)frontier.Out();
            // 到达目标，提前退出
            if (current == target) break;
            foreach (TileNode neighbor in current.neighbors)
            {
                int newCost = costSoFar[current] + neighbor.cost;
                if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                {
                    frontier.Push(neighbor, newCost);
                    costSoFar[neighbor] = newCost;
                    cameFrom.Add(neighbor, current);
                    mClickableTilemap.mTilemap.SetColor(neighbor.position, Color.blue);
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
        while (cameFrom[target] != null)
        {
            mClickableTilemap.mTilemap.SetColor(target.position, Color.red);
            target = cameFrom[target];
        }
    }

    private IEnumerator Greedy()
    {
        PriorityQueue frontier = new PriorityQueue();
        Dictionary<TileNode, TileNode> cameFrom = new Dictionary<TileNode, TileNode>();

        TileNode start = TotalTileModels[character.cellPosition];
        TileNode target = TotalTileModels[destination.cellPosition];

        frontier.Push(start, 0);
        cameFrom.Add(start, null);

        while (frontier.GetCount() != 0)
        {
            TileNode current = (TileNode)frontier.Out();
            // 到达目标，提前退出
            if (current == target) break;
            foreach (TileNode neighbor in current.neighbors)
            {
                if (neighbor.nodeType == NodeType.Road && !cameFrom.ContainsKey(neighbor))
                {
                    int cost = heuristic(neighbor, target);
                    frontier.Push(neighbor, cost);
                    cameFrom.Add(neighbor, current);
                    mClickableTilemap.mTilemap.SetColor(neighbor.position, Color.blue);
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
        while (cameFrom[target] != null)
        {
            mClickableTilemap.mTilemap.SetColor(target.position, Color.red);
            target = cameFrom[target];
        }
    }

    private IEnumerator AStar()
    {
        PriorityQueue frontier = new PriorityQueue();
        Dictionary<TileNode, TileNode> cameFrom = new Dictionary<TileNode, TileNode>();
        Dictionary<TileNode, int> costSoFar = new Dictionary<TileNode, int>();

        TileNode start = TotalTileModels[character.cellPosition];
        TileNode target = TotalTileModels[destination.cellPosition];

        frontier.Push(start, 0);
        costSoFar.Add(start, 0);
        cameFrom.Add(start, null);

        while (frontier.GetCount() != 0)
        {
            TileNode current = (TileNode)frontier.Out();
            // 到达目标，提前退出
            if (current == target) break;
            foreach (TileNode neighbor in current.neighbors)
            {
                int newCost = costSoFar[current] + neighbor.cost;
                if (neighbor.nodeType != NodeType.Road) continue;
                if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                {
                    costSoFar[neighbor] = newCost;
                    frontier.Push(neighbor, newCost + heuristic(neighbor, target));
                    cameFrom.Add(neighbor, current);
                    mClickableTilemap.mTilemap.SetColor(neighbor.position, Color.blue);
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
        while (cameFrom[target] != null)
        {
            mClickableTilemap.mTilemap.SetColor(target.position, Color.red);
            target = cameFrom[target];
        }
    }

    private int heuristic(TileNode nodeA, TileNode nodeB)
    {
        // 曼哈顿距离
        return Mathf.Abs(nodeA.position.x - nodeB.position.x) + Mathf.Abs(nodeA.position.y - nodeB.position.y);
    }
}
