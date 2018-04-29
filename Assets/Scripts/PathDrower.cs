using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PathDrower
{
    [Tooltip("Should correspond with path type enum")]
    public List<Vector2Int> Path = new List<Vector2Int>();
    private MapManager _mapManager;
    private PathSectionVisuals.Factory _pathSectoinFactory;
    private List<PathSectionVisuals> _visualsList = new List<PathSectionVisuals>();


    public PathDrower(MapManager mM, PathSectionVisuals.Factory factory)
    {
        _pathSectoinFactory = factory;
        _mapManager = mM;

        Path.Add(Vector2Int.zero);
        Path.Add(new Vector2Int(0, -1));
        Path.Add(new Vector2Int(1, -1));
        Path.Add(new Vector2Int(1, -2));
        Path.Add(new Vector2Int(2, -2));
        Path.Add(new Vector2Int(2, -3));
        Path.Add(new Vector2Int(2, -4));




    }

    public enum PathTypes
    {
        Vertical,
        Horrizontal,
        ArrowVerticaUp,
        ArrowVerticalDown,
        ArrowHorizontalLeft,
        ArrowHorizontalRight,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight

    }
    public PathTypes GetPathTypeOnSection(Vector2Int one, Vector2Int two, bool end, Vector2Int dir)
    {

        PathTypes ret = PathTypes.UpLeft;
        if (end)
        {
            if (one.x != two.x)
            {

                if (one.x > two.x)
                {
                    ret = PathTypes.ArrowHorizontalLeft;
                }
                if (one.x < two.x)
                {
                    ret = PathTypes.ArrowHorizontalRight;
                    //ret = PathTypes.ArrowVerticalDown;
                }
            }
            else
            {
                if (one.y > two.y)
                {
                    ret = PathTypes.ArrowVerticalDown;

                    //ret = PathTypes.ArrowHorizontalRight;
                }
                if (one.y < two.y)
                {
                    ret = PathTypes.ArrowVerticaUp;
                }
            }

        }
        else
        {

            if (dir != Vector2Int.zero && dir != two - one)
            {
                dir = two - one;
                Debug.Log(dir);
                if (dir.x == 1)
                {
                    ret = PathTypes.UpLeft;

                }
                else if (dir.x == -1)
                {

                    ret = PathTypes.UpRight;
                    //ret = PathTypes.DownRight;
                }
                if (two.x < Path[0].x)
                {

                    if (dir.y == -1)
                    {

                        ret = PathTypes.UpRight;
                        //ret = PathTypes.UpRight;
                    }
                    else
                    {
                        ret = PathTypes.DownRight;
                    }
                }
                else
                {
                    if (dir.y == -1)
                    {

                        ret = PathTypes.UpLeft;
                        //ret = PathTypes.UpRight;
                    }
                    else
                    {
                        ret = PathTypes.DownLeft;
                    }
                }
            }
            else
            {
                if (one.x == two.x)
                {
                    ret = PathTypes.Vertical;
                }
                else if (one.y == two.y)
                {
                    ret = PathTypes.Horrizontal;
                }
            }
        }



        return ret;
    }

    private List<PathTypes> GetPath()
    {
        List<PathTypes> ret = new List<PathTypes>();
        Vector2Int direction = Vector2Int.zero;
        for (int i = 0; i < Path.Count - 1; i++)
        {

            bool end = false;
            ret.Add(GetPathTypeOnSection(Path[i], Path[i + 1], end, direction));
            direction = Path[i + 1] - Path[i];
            if (i == Path.Count - 2)
            {
                Path.Add(Path[Path.Count - 1] + direction);
                end = true;
                ret.Add(GetPathTypeOnSection(Path[i + 1], Path[i + 2], end, direction));
                return ret;
            }
        }

        return ret;
    }

    public void ClearPath()
    {
        foreach (PathSectionVisuals p in _visualsList)
        {
            GameObject.Destroy(p.gameObject);
        }
        _visualsList.Clear();
    }

    public void ReDrawPath(Vector2Int start, Vector2Int end)
    {
        ClearPath();
        Path = _mapManager.GetShortestPath(start, end);
        Path.Reverse();
        List<PathTypes> types = GetPath();
        for (int i = 0; i < Path.Count-1; i++)
        {
            Vector3 pos = _mapManager.GetTilePosition(Path[i].x, Path[i].y);
            PathSectionVisuals ps = _pathSectoinFactory.Create(types[i]);
            _visualsList.Add(ps);
            ps.transform.position = pos;
        }
    }

}
