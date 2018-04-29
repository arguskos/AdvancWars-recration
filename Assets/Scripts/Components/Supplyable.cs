using System;
using UnityEngine;
using Zenject;

public class Supplyable : BaseUnitProprty
{
    public int Supply { get; private set; }



    public void Start()
    {
        Supply = GetComponent<BaseUnit>().Setting.SupplySettings.MaxSupply;
    }

    [Serializable]
    public class Settings
    {
        public int MaxSupply;
    }


}
