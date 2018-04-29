using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class CanCapture : BaseAbility
{
    private MapEntetiesManager _mapEntetiesManager;
    private bool Capturing = false;
    
    [Inject]
    public void Construct(MapEntetiesManager mEM)
    {
        _mapEntetiesManager = mEM;
    }

    protected void Awake()
    {
        Name = "Capt";
    }


    public override bool CanBeActivated()
    {
        Base building = _mapEntetiesManager.GetEntitieOfType<Base>(Unit.Position) as Base;
        bool capturable = building != null && building.Team != Unit.Team;
        return capturable;
    }


    public override void Action()
    {
        Capturing = true;
        Base building = _mapEntetiesManager.GetEntitieOfType<Base>(Unit.Position) as Base;
        building.Capture(10, Unit.Team);
        Unit.ChangeState(UnitStates.Wait);
    }

}
