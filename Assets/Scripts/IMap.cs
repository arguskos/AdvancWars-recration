using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public interface IMap  {

    BaseTile GetTile(int x, int y);
    BaseTile GetTile(Vector2Int pos);
    Vector3 GetTilePosition(int x, int y);
}
