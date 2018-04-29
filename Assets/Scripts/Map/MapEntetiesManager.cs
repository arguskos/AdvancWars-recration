using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;

public class MapEntetiesManager
{
    public Dictionary<Vector2Int, HashSet<TileEntity>> TileEntities
    { get; private set; }

    private MapManager _mapManager;

    public MapEntetiesManager(MapManager mm)
    {
        _mapManager = mm;
        TileEntities = new Dictionary<Vector2Int, HashSet<TileEntity>>();

    }

    public List<TileEntity> GetAllEnemiesInRange(BaseUnit u, CanFire f)
    {
        return GetAllEnemiesInRange(u.Position, f.Settings.Range, u.Team);
    }

    private List<TileEntity> GetAllEnemiesInRange(Vector2Int point, int range, int team)
    {
        var units = from entetie in GetAllEntetiesInRange(point, range)
                    where entetie is BaseUnit && entetie.Team != team

                    select entetie;
        return units.ToList();
    }

    private List<TileEntity> GetAllUnitsInRange(Vector2Int point, int range)
    {
        var units = from entetie in GetAllEntetiesInRange(point, range)
                    where entetie is BaseUnit
                    select entetie;
        return units.ToList();
    }

    private List<TileEntity> GetAllEntetiesInRange(Vector2Int point, int range)
    {
        HashSet<Vector2Int> tiles = _mapManager.GetAvailableTilesInRange(point.x, point.y, range);
        List<TileEntity> units = new List<TileEntity>();
        foreach (Vector2Int p in tiles)
        {
            if (TileEntities.ContainsKey(p))
            {
                foreach (TileEntity t in TileEntities[p])
                {
                    units.Add(t);
                }
            }
        }
        return units;
    }

    public void MoveEntetie(Vector2Int point, TileEntity b, bool teleport = false)
    {
        if (!teleport && TileEntities.ContainsKey(b.Position))
        {
            TileEntities[b.Position].Remove(b);
            if (TileEntities[b.Position].Count == 0)
            {
                TileEntities.Remove(b.Position);
            }
        }
        //if (TileEntities.ContainsValue(b))
        //{
        //    TileEntities.Remove(b.Position);
        //}
        bool occupied = false;
        if (TileEntities.ContainsKey(point))
        {
            foreach (TileEntity t in TileEntities[point])
            {
                if (t is BaseUnit)
                {
                    occupied = true;
                    break;
                }
            }
        }
        else
        {
            TileEntities.Add(point, new HashSet<TileEntity>() { });
        }

        if (!occupied)
        {
            TileEntities[point].Add(b);
            b.transform.position = _mapManager.GetTilePosition(point.x, point.y);
        }
        else
        {
            Debug.Log("Tile is occupied");
        }
    }

    public TileEntity GetEntitieOfType<T>(Vector2Int point) where T : TileEntity
    {
        if (TileEntities.ContainsKey(point))
        {
            foreach (var entitie in TileEntities[point])
            {
                if (entitie is T)
                {
                    return entitie as T;
                }
            }
            return null;
        }
        else
            return null;
    }

   

}
