using UnityEngine;
using Zenject;
public class ActionMenuState : BaseUnitState
{
    private readonly IActionMenu _actionMenu;
    private readonly BaseUnit _unit;
    private readonly PlayerInputState _inputState;

    ActionMenuState(IActionMenu aM, BaseUnit unit, PlayerInputState iS)
    {
        _actionMenu = aM;
        _unit = unit;
        _inputState = iS;
        _inputState.IsCancelAction = Cancel;
        _inputState.IsConfirmedAction = Confirm;
    }

    public override void Start()
    {
        _actionMenu.ShowMenu(_unit.Abilities); 
    }

    public override void Dispose()
    {
        _actionMenu.Cancel();
        //_unit.CancelPreviosMove();
        
    }
    public void Confirm()
    {
        _actionMenu.Confirm();
    }
    public void Cancel()
    {
        _unit.CancelPreviosMove();

        Dispose();

        _unit.ChangeState(UnitStates.Move);
    }
    public class Factory : Factory<ActionMenuState>
    {

    }
}
