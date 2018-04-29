using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CanWait : BaseAbility
{

    private BaseUnit _baseUnit;
    protected void Awake()
    {
        _baseUnit = GetComponent<BaseUnit>();
        //Action = Menu.HideMenu;
        //Menu.HideMenu()l
    }

    public override void Action()
    {
        _baseUnit.ChangeState(UnitStates.Active);
    }

    


}
