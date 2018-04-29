using UnityEngine;
using Zenject;
public class UnitMoveState : BaseUnitState
{
    MapManager _mapManagaer;
    MapHighlighter _mapHighlighter;
    BaseUnit _baseUnit;
    PlayerInputState _playerInputState;
    Selection _selection;
    PathDrower _pathDrower;
    Player _player;

    UnitMoveState(Player p, BaseUnit baseUnit, MapManager mP, MapHighlighter mH, PlayerInputState pS, Selection selection, PathDrower pathDrower)
    {
        _player = p;
        _pathDrower = pathDrower;
        

        _baseUnit = baseUnit;
        _mapHighlighter = mH;
        _mapManagaer = mP;
        _playerInputState = pS;
        _selection = selection;

        _playerInputState.IsCancelAction += Cancel;
        _playerInputState.IsConfirmedAction = Confirm;



    }


    public void Confirm()
    {

        foreach (var player in GameObject.FindObjectsOfType<Player>())
        {
            player.CmdMove(_baseUnit.Position.x, _baseUnit.Position.y, _selection.Position.x, _selection.Position.y);
        }

        //_baseUnit.Move(_selection.Position.x,_selection.Position.y);

        Dispose();

        _baseUnit.ChangeState(UnitStates.ShowActionMenu);
    }
    public override void Start()
    {
        //_baseUnit = holder;
        _mapHighlighter.HighlightAvailableTiles(_baseUnit);
        _selection.SetMode(SelectionMode.Free);
        _pathDrower.ReDrawPath(_baseUnit.Position,_selection.Position);
    }
    public void Cancel()
    {
        _mapHighlighter.ClearHighlightedTiles();
        _playerInputState.IsConfirmedAction = _selection.Confirm;
        _pathDrower.ClearPath();
        //_playerInputState.IsCancelAction = null;
        //_pathDrower.ClearPath();
        //_selection.UnSelect(_baseUnit.Position);
    }
    public override void Dispose()
    {
        Cancel();
        //_baseUnit.ChangeState(UnitStates.Active);
    }
    public class Factory : Factory<UnitMoveState>
    {
    }
}
