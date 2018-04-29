using System;
using UnityEngine;
using Zenject;

public class Infantry : BaseUnit{

    public Vector2Int StartPosition;

    [Inject]
    private InfantrySettinigs _settings;

    public override Settings Setting { get { return _settings; }  }

 
    protected override void Start()
    {
        base.Start();
        //Position = StartPosition;

    }

    [Serializable]
    public class InfantrySettinigs : Settings
    {
        public CanWalk.WalkSettings WalkSettings;
        public CanFire.FireSettings FireSettings;
    }

}

