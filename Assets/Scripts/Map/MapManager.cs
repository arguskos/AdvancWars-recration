using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;
public class MapManager : IMap
{
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Vector3 CenterOffset;

    private Tilemap _tilemap;
    private Highlighter.HighlighterFactory _highlighterFactory;

    //public Dictionary<Vector2Int, TileEntity> TileEntities
    //{ get; private set; }



    public MapManager(Highlighter.HighlighterFactory hf, Tilemap tilemap)
    {
        _highlighterFactory = hf;
        _tilemap = tilemap;
        GetShortestPath(new Vector2Int(0, 0), new Vector2Int(4, -3));
        //HighlightInRadius(3, -2, 4);
        //AStartSearch(new Vector2Int(0, 0), new Vector2Int(10, -4));
        //CenterOffset = new Vector3(grid.cellSize.x / 2, 0f, grid.cellSize.y / 2);
        //grid = gd;
    }
    //[Inject]

    public Vector3Int GetBorders()
    {
        return _tilemap.size;
    }

    public List<Vector2Int> GetShortestPath(Vector2Int start, Vector2Int end)
    {

        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, int> costSoFar = new Dictionary<Vector2Int, int>();
        PriorityQueue<Vector2Int> frontier = new PriorityQueue<Vector2Int>();
        frontier.Enqueue(start, 0);
        //start.SetMat(Frontier);

        cameFrom[start] = start;
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            Vector2Int current = frontier.Dequeue();
            if (current == end)
            {
                break;
            }

            foreach (Vector2Int neighbor in GetFreeNeighbors(current))
            {
                int newCost = costSoFar[current] + 0;//g.Cost(current, neighbor);
                if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                {
                    costSoFar[neighbor] = newCost;
                    int priority = newCost + MapManager.Heuristic(neighbor, end);
                    frontier.Enqueue(neighbor, priority);
                    //neighbor.SetMat(Frontier);
                    cameFrom[neighbor] = current;

                    //_currentMap.HighlightTile(current.x, current.y);
                }
            }
        }
        Vector2Int current2 = end;
        List<Vector2Int> total = new List<Vector2Int>() { current2 };
        while (cameFrom.ContainsKey(current2) && current2 != start)
        {
            current2 = cameFrom[current2];
            total.Add(current2);

        }
        return total;
    }
    static public int Heuristic(Vector2Int a, Vector2Int b)
    {
        return (int)Mathf.Floor(Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y));
    }


    private bool IsTileWalkable(Vector2Int point)
    {
        return GetTile(point).CanWalk;
    }

    public Vector2Int GetClossestTileFromPos(Vector3 position)
    {
        Vector3Int pos = _tilemap.WorldToCell(position);
        return  new Vector2Int(pos.x,pos.y) ;
    }

    public IEnumerable<Vector2Int> GetFreeNeighbors(Vector2Int tile)
    {

        var t = GetTile(tile - new Vector2Int(1, 0)) as BaseTile;
        if (t)
            yield return tile - new Vector2Int(1, 0);
        t = GetTile(tile - new Vector2Int(-1, 0)) as BaseTile;
        if (t)
            yield return tile - new Vector2Int(-1, 0);
        t = GetTile(tile - new Vector2Int(0, 1)) as BaseTile;
        if (t)
            yield return tile - new Vector2Int(0, 1);
        t = GetTile(tile - new Vector2Int(0, -1)) as BaseTile;
        if (t)
            yield return tile - new Vector2Int(0, -1);

    }

    //public void HighlightTile(int x, int y)
    //{
    //    GameObject highlighter = _highlighterFactory.Create();
    //    highlighter.transform.position = GetTilePosition(x, y) + new Vector3(0, 0, 0);
    //}

    private void GetWalkableTiles(int startX, int starty, MovemntTypes movementType, int radius, HashSet<Vector2Int> s)
    {
        int speedOnCurrentTile = GetTile(startX, starty).MoveSpeedForType[movementType];
        if (!s.Contains(new Vector2Int(startX, starty))
            && speedOnCurrentTile != -1
            )
        {
            //HighlightTile(startX, starty);
            s.Add(new Vector2Int(startX, starty));
        }

        if (radius > speedOnCurrentTile && speedOnCurrentTile != -1)
        {
            foreach (var tile in GetFreeNeighbors(new Vector2Int(startX, starty)))
            {
                GetWalkableTiles(tile.x, tile.y, movementType, radius - speedOnCurrentTile, s);
            }
        }
    }

    private void GetAllTilesInRadiusRecursionHelper(int startX, int starty, int radius, HashSet<Vector2Int> s)
    {
        if (radius == 0)
            return;
        if (!s.Contains(new Vector2Int(startX, starty)))
        {
            //HighlightTile(startX, starty);
            s.Add(new Vector2Int(startX, starty));
        }
        else
        {
            return;
        }

        foreach (var tile in GetFreeNeighbors(new Vector2Int(startX, starty)))
        {
            GetAllTilesInRadiusRecursionHelper(tile.x, tile.y, radius - 1, s);
        }
    }

    public HashSet<Vector2Int> GetAvailableTilesInRange(int startX, int startY, int radius)
    {
        HashSet<Vector2Int> s = new HashSet<Vector2Int>();
        GetAllTilesInRadiusRecursionHelper(startX, startY, radius + 1, s);
        return s;
    }


    public HashSet<Vector2Int> GetAvailableTilesForUnit(BaseUnit unit)
    {
        CanWalk w = unit.GetComponent<CanWalk>();
        return GetAvailableTilesInRangeForMovemntType(unit.Position.x, unit.Position.y, w.Settings.MovemntType, w.Settings.Range);
    }

    public HashSet<Vector2Int> GetAvailableTilesInRangeForMovemntType(int startX, int startY, MovemntTypes movemntType, int radius)
    {
        HashSet<Vector2Int> s = new HashSet<Vector2Int>();
        GetWalkableTiles(startX, startY, movemntType, radius + 1, s);
        return s;
    }


    public BaseTile GetTile(int x, int y)
    {
        return _tilemap.GetTile(new Vector3Int(x, y, 0)) as BaseTile;
    }
    public BaseTile GetTile(Vector2Int pos)
    {
        return GetTile(pos.x, pos.y) as BaseTile;
    }
    public Vector3 GetTilePosition(int x, int y)
    {
        Vector3Int temp = new Vector3Int(x, y, 0);
        // return grid.CellToWorld(temp) + CenterOffset;
        return _tilemap.GetCellCenterWorld(temp);
    }
    // Update is called once per frame


}
