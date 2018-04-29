using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[System.Serializable]
public class DamageInfo
{
   public string Name;
   public int BaseDamage;
}

[System.Serializable]
public class CanFire :BaseAbility  {

    [HideInInspector]
    public FireSettings Settings;


    private Damageable _damageable;
    private MapEntetiesManager _entetiesManager;

    [Inject]
    public void Construct(Damageable damageable, MapEntetiesManager entetiesManager)
    {
        _damageable = damageable; 
        _entetiesManager = entetiesManager;

    }

    private void Awake()
    {
        Name = "Fire";
        Settings.Type = GetComponent<BaseUnit>().GetType().Name;
        Settings.Init();
    }

    public override void Action()
    {
        
        GetComponent<BaseUnit>().ChangeState(UnitStates.Fire);

    }

    public override bool CanBeActivated()
    {
        var points = _entetiesManager.GetAllEnemiesInRange(GetComponent<BaseUnit>(), this);
        return points.Count > 0;
    }

    [System.Serializable]
    public class FireSettings : Settings
    {
        public int Range;
        public int Ammo;
        public string Type;

        public  List<DamageInfo> DamageToBaseUnitType = new List<DamageInfo>();
        public Dictionary<string,DamageInfo> DamageToBaseUnitTypeDict = new Dictionary<string, DamageInfo>();
          
        public void Init()
        {
            foreach (DamageInfo info in DamageToBaseUnitType)
            {

                DamageToBaseUnitTypeDict[info.Name] = info;
            }
        }
    }

}
