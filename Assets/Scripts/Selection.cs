using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public enum SelectionMode
{
    Free,
    Cicle
}

public class Selection : MonoBehaviour
{
    private PlayerInputState _playerInputState;
    private MapManager _currentMap;
    private MapEntetiesManager _entetieManager;
    private PathDrower _pathDrower;
    private BaseUnit _currentSelection;
    private List<Vector2Int> _currentlyAlloweMoveTiles = null;
    private SelectionMode _selectionMode = SelectionMode.Free;
    private int _currentSelectionIndex = 0;
    private GameState _gameState;
    private TileEntity.Factory _facTest;


    public Vector2Int Position { get; private set; }
    public Action OnSelectionChanged;



    [Inject]
    void Constructor(TileEntity.Factory factory, GameState gS, MapEntetiesManager me, PlayerInputState ps, MapManager mp, PathDrower pathDrower)
    {
        _facTest = factory;

        _gameState = gS;
        _entetieManager = me;
        _playerInputState = ps;
        _pathDrower = pathDrower;
        _currentMap = mp;


        ReAddActions();
    }

    public void Start()
    {
     //   _facTest.Create(typeof(Infantry));
       //_facTest.Create();
        //  _facTest.Create(FindObjectOfType<Infantry>());
    }

    public void SetMode(SelectionMode mode, List<Vector2Int> allowedTiles = null)
    {
        Debug.Assert(!(mode == SelectionMode.Cicle && allowedTiles == null), "allowed tiles cannot be null with circle mode");

        _selectionMode = mode;
        _currentlyAlloweMoveTiles = allowedTiles;
        _currentSelectionIndex = 0;
        if (mode == SelectionMode.Cicle)
        {
            Position = allowedTiles[0];
        }
        UpdatePosition();
    }

    public void ReAddActions()
    {
        _playerInputState.IsMovedAction = Move;
        _playerInputState.IsConfirmedAction = Confirm;
        _playerInputState.IsCancelAction = Cancel;
    }

    public void Move(int x, int y)
    {

        //transform.position += new Vector3(x*_currentMap._settings.TileSize.x, 0, y* _currentMap._settings.TileSize.x) ;

        Vector2Int old = Position;
        switch (_selectionMode)
        {
            case SelectionMode.Free:
                Position += new Vector2Int(x, y);
                break;
            case SelectionMode.Cicle:
                _currentSelectionIndex++;
                if (_currentSelectionIndex > _currentlyAlloweMoveTiles.Count - 1)
                {
                    _currentSelectionIndex = 0;
                }
                Position = _currentlyAlloweMoveTiles[_currentSelectionIndex];
                break;
            default:
                break;
        }

        Vector3Int size = _currentMap.GetBorders();
        if (Position.x < 0)
            Position = new Vector2Int(0, Position.y);
        else if (Position.x > size.x / 2)
            Position = new Vector2Int(size.x / 2, Position.y);
        if (Position.y > 0)
            Position = new Vector2Int(Position.x, 0);
        else if (Position.y < -size.y / 2)
            Position = new Vector2Int(Position.x, -size.y / 2);

        if (_currentlyAlloweMoveTiles != null && !_currentlyAlloweMoveTiles.Contains(Position))
        {
            Position = old;
        }

        if (_currentSelection != null && _currentSelection.State is UnitMoveState)
        {
            _pathDrower.ReDrawPath(_currentSelection.Position, Position);
        }

        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.position = _currentMap.GetTilePosition(Position.x, Position.y);
        if (OnSelectionChanged != null)
        {
            OnSelectionChanged();
        }
    }

    public void UnSelect()
    {

        _currentSelection = null;
        _currentlyAlloweMoveTiles = null;

    }


    public void Cancel()
    {
        if (_currentSelection)
        {
            Position = _currentSelection.Position;
            transform.position = _currentMap.GetTilePosition(Position.x, Position.y);
            _currentSelection = null;
            _pathDrower.ClearPath();
            _currentlyAlloweMoveTiles = null;
        }
    }
    public void Confirm()
    {
        TileEntity t = _entetieManager.GetEntitieOfType<BaseUnit>(Position);
        if (t != null && t is BaseUnit && t.Player == _gameState.CurrentPlayerTurn)
        {
            _currentSelection = t as BaseUnit;
            _currentSelection.Select();
            _currentlyAlloweMoveTiles = _currentMap.GetAvailableTilesForUnit(_currentSelection).ToList();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {

        }
    }
}

