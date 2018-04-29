using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;
#if UNITY_EDITOR
using UnityEditor;
#endif
[Serializable]
public class BaseTile : Tile
{
    public int WalkSpeed;
    public bool CanWalk;

    public int MovementCostOnFoot;
    public int MovementCostOnMechFoot;
    public int MovementCostAir;
    public int MovementCostTier;
    public int MovementCostTread;
    public int MovementCostShips;
    public int MovementCostTransportWater;
    public int BaseDef;

    public Dictionary<MovemntTypes, int> MoveSpeedForType = new Dictionary<MovemntTypes, int>();

    private Highlighter.HighlighterFactory highlightFactory;

    protected void OnEnable()
    {
        MoveSpeedForType.Add(MovemntTypes.Fly, MovementCostAir);
        MoveSpeedForType.Add(MovemntTypes.OnFoot, MovementCostOnFoot);
        MoveSpeedForType.Add(MovemntTypes.MechFoot, MovementCostOnMechFoot);
        MoveSpeedForType.Add(MovemntTypes.Tier, MovementCostTier);
        MoveSpeedForType.Add(MovemntTypes.Tread, MovementCostTread);
        MoveSpeedForType.Add(MovemntTypes.Ship, MovementCostShips);
        MoveSpeedForType.Add(MovemntTypes.UnderWater, MovementCostShips);
        MoveSpeedForType.Add(MovemntTypes.TransportShip, MovementCostTransportWater );

    }

#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a RoadTile Asset
    [MenuItem("Assets/Create/BaseTile")]
    public static void CreateRoadTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Road Tile", "New Road Tile", "Asset", "Save Road Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<BaseTile>(), path);
    }

#endif
}
