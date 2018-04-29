using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Zenject;


//// just a marker interface, to get the poo type
//public interface ISettingsProvider<SettingsType> { }

//// Extension method to get the correct type of excrement
//public static class IPooProviderExtension
//{
//    public static SettingsType StronglyTypedExcrement<SettingsType>(
//        this ISettingsProvider<SettingsType> iPooProvider)
//        where SettingsType : BaseUnit.Settings
//    {
//        BaseUnit animal = iPooProvider as BaseUnit;
//        if (null == animal)
//        {
//            Debug.LogError("shoud be of type base unit");
//        }
//        return (SettingsType)animal._settings;
//    }
//}

public abstract class BaseUnit : TileEntity, ISelectable 
{

    public virtual Settings Setting { get; set; }

    public BaseUnitState State { get; protected set; }
    public List<BaseAbility> Abilities { get; private set; }
    public List<BaseUnitProprty> Properties { get; private set; }

    public Action OnChangeHealth;
    public bool WalkedThisTurn = false;

    private UnitStateFactory _stateFactory;
    private Vector2Int _previousPosition;

    private SpriteRenderer _spriteRenderer;


    [Inject]
    public virtual void Construct(List<BaseUnitProprty> properties, SpriteRenderer renderer,List<BaseAbility> allAbilities, UnitStateFactory sF)
    {

        _spriteRenderer = renderer;

        Abilities = allAbilities;
        AssignSettings();
        _stateFactory = sF;
        Properties = properties;
    }


    public BaseUnitProprty GetPropertyOfType<T>() where T : BaseUnitProprty
    {
        foreach (BaseUnitProprty b in Properties)
        {
            if (b.GetType() == typeof(T))
            {
                return b;
            }
        }
        return null;
    }


    public BaseAbility GetAbilitieOfType<T>() where T : BaseAbility
    {
        foreach (BaseAbility b in Abilities)
        {
            if (b.GetType() == typeof(T))
            {
                return b;
            }
        }
        return null;
    }


    protected override void Start()
    {
        base.Start();

        ChangeState(UnitStates.Active);
        _previousPosition = Position;
    }


    /// <summary>
    /// Searches for the field named _setting 
    /// retrives all fields from it is of type Base ability
    /// assinges componets of type base ability with a correct setting for this class
    /// This is super ugly 
    /// but I don't know  a better way for this yet
    /// </summary>
    private void AssignSettings()
    {
        Type t = this.GetType();
        FieldInfo[] fields = (t.GetField("_settings", BindingFlags.NonPublic | BindingFlags.Instance).FieldType.GetFields());
        foreach (FieldInfo setting in fields)
        {
            foreach (BaseAbility abilitie in Abilities)
            {
                FieldInfo[] allFields = abilitie.GetType().GetFields();

                foreach (FieldInfo abilitieField in allFields)
                {

                    //Searching for field of correspoding type
                    if (
                         abilitieField.FieldType == setting.FieldType
                        )
                    {
                        //print(abilitieField.FieldType);
                        //print(setting.FieldType);
                        object unitValue = setting.GetValue(t.GetField("_settings", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this));
                        abilitieField.SetValue(abilitie, unitValue);
                        break;
                    }


                }
            }
        }
    }

    //public Vector2Int GetWalkableTiles()
    //{
    //    //add tiles around if you have points for them
    //    //
    //}

    public void Select()
    {
        ChangeState(UnitStates.Move);
    }


    public override void RpcMove(int x, int y,bool teleport)
    {
        _previousPosition = Position;
        base.RpcMove(x, y,teleport);
    }

    public void CancelPreviosMove()
    {
        EntetiesManager.MoveEntetie(_previousPosition, this);
        Position = _previousPosition;
    }

    public void ChangeState(UnitStates state)
    {
        if (State != null)
        {
            State.Dispose();
            State = null;
        }

        State = _stateFactory.CreateState(state);
        State.Start();
    }



    public virtual void CmdDeactivate()
    {
        Deactivate();
    }
    public void Deactivate()
    {
        _spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
        WalkedThisTurn = true;

    }
    public virtual void Activate()
    {
        _spriteRenderer.color = new Color(1f,1f, 1f);

    }


    [System.Serializable]
    public class Settings
    {
        //public int MaxHealth;
        public Damageable.Settings DamageableSettings; 
        public Supplyable.Settings SupplySettings;
    }

}

