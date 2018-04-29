using System;
using UnityEngine;
using Zenject;

public class SmallTank : BaseUnit
{

    public Vector2Int StartPosition;

    [Inject]
    private SmallTankSettings _settings;

    public override Settings Setting { get { return _settings; } }

    protected override void Start()
    {
        Position = StartPosition;
        base.Start();

    }

    [Serializable]
    public class SmallTankSettings : Settings
    {
        public CanWalk.WalkSettings WalkSettings;
        public CanFire.FireSettings FireSettings;
    }

}

