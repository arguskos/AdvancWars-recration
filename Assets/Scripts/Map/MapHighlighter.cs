using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHighlighter  {
    private Highlighter.HighlighterFactory _highlighterFactory;
    private MapManager _mapManager;
    private List<GameObject> _highlighted = new List<GameObject>();
    public MapHighlighter(Highlighter.HighlighterFactory hf, MapManager mm)
    {
        _highlighterFactory = hf;
        _mapManager = mm;
    }
    public void ClearHighlightedTiles()
    {
        foreach(var h in _highlighted)
        {
            GameObject.Destroy(h);
        }
    }
    public void HighlightTile(Vector2Int pos)
    {
        GameObject highlighter = _highlighterFactory.Create();
        highlighter.transform.position = _mapManager.GetTilePosition(pos.x,pos.y) + new Vector3(0, 0, 0);
        _highlighted.Add(highlighter);
    }
    public void HighlightAvailableTiles(BaseUnit unit)
    {
        CanWalk w = unit.GetComponent<CanWalk>();
        foreach (Vector2Int tilePos in _mapManager.GetAvailableTilesInRangeForMovemntType(unit.Position.x,unit.Position.y,w.Settings.MovemntType, w.Settings.Range))
        {
            HighlightTile(tilePos);
        }
    }
}
