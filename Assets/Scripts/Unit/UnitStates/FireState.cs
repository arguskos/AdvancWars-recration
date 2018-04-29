using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class FireState : BaseUnitState
{
    private readonly CanFire _fireComponent;

    private readonly MapEntetiesManager _entetiesManager;
    private readonly Selection _selection;

    FireState(CanFire cf, Selection selection, PlayerInputState iS, MapEntetiesManager entetiesManager)
    {
        _selection = selection;
        _fireComponent = cf;
        _entetiesManager = entetiesManager;

        iS.IsCancelAction = Cancel;
        iS.IsConfirmedAction = Confirm;
    }

    public override void Start()
    {
        List<Vector2Int> positions = new List<Vector2Int>();
        foreach (BaseUnit b in _entetiesManager.GetAllEnemiesInRange(_fireComponent.GetComponent<BaseUnit>(), _fireComponent))
        {
            positions.Add(b.Position);
            Debug.Log(b);
        }
        _selection.SetMode(SelectionMode.Cicle, positions);

    }

    public override void Dispose()
    {

    }

    public void Confirm()
    {
        //Get references 
        BaseUnit other = _entetiesManager.GetEntitieOfType<BaseUnit>(_selection.Position) as BaseUnit;

        Damageable dThis = _fireComponent.Unit.GetPropertyOfType<Damageable>() as Damageable;
        CanFire fThis = _fireComponent;

        Damageable dOther = other.GetPropertyOfType<Damageable>() as Damageable;
        CanFire fOther = other.GetAbilitieOfType<CanFire>() as CanFire;

        if (!(fThis && dOther))
        {
            Debug.LogWarning("Cant attck because one of units doesnt have necessary components");
            return;
        }

        // Calclulate damage
        int baseDamade = _fireComponent.Settings.DamageToBaseUnitTypeDict[fOther.Settings.Type].BaseDamage;

        float damagePercent = DamageCalculator.OriginalFormula(
            baseDamade,
            dThis.Health,
            0,
            dOther.Health
            );
        dOther.ModifyHealth(-(int)Mathf.Floor(damagePercent / 10));

        //Attack back if can 
        if (fOther)
        {
            baseDamade = fOther.Settings.DamageToBaseUnitTypeDict[fThis.Settings.Type].BaseDamage;

            damagePercent = DamageCalculator.OriginalFormula(
               baseDamade,
               dOther.Health,
               0,
               dThis.Health
               );

            dThis.ModifyHealth(-(int)Mathf.Floor(damagePercent / 10));
        }


        fThis.GetComponent<BaseUnit>().ChangeState(UnitStates.Wait);

    }

    public void Cancel()
    {
        Debug.Log("cancel");
        _fireComponent.GetComponent<BaseUnit>().ChangeState(UnitStates.ShowActionMenu);
    }
    public class Factory : Factory<FireState>
    {

    }
}
