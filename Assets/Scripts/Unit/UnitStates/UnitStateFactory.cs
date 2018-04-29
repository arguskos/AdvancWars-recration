using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Zenject;
using ModestTree;

public enum UnitStates
{
    Active,
    Move,
    ShowActionMenu,
    Wait,
    Fire
}

public class UnitStateFactory
{
    readonly UnitMoveState.Factory _moveState;
    readonly UnitActiveState.Factory _activeState;
    readonly ActionMenuState.Factory _actionMenuState;
    readonly WaitState.Factory _waitState;
    readonly FireState.Factory _fireState;

    public UnitStateFactory(
         UnitMoveState.Factory mS,
         UnitActiveState.Factory aS,
        ActionMenuState.Factory actionMenu,
        WaitState.Factory wS,
       FireState.Factory fS

         )
    {
        _actionMenuState = actionMenu;
        _moveState = mS;
        _activeState = aS;
        _waitState = wS;
        _fireState = fS;


        //_movingFactory = movingFactory;
        //_deadFactory = deadFactory;
    }

    public BaseUnitState CreateState(UnitStates state)
    {
        switch (state)
        {
            case UnitStates.Active:
                return _activeState.Create();
            case UnitStates.Move:
                return _moveState.Create();
            case UnitStates.ShowActionMenu:
                return _actionMenuState.Create();
            case UnitStates.Wait:
                return _waitState.Create();
            case UnitStates.Fire:
                return _fireState.Create();
        }

        throw Assert.CreateException();
    }
}
