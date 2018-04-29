using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CanWalk : BaseAbility
{
    public WalkSettings Settings;

    private void Awake()
    {
        Name = "Wait";
        //Icon = Resources.Load("glass") as Texture;
    }


    public override void Action()
    {

        GetComponent<BaseUnit>().ChangeState(UnitStates.Wait);

    }

    [System.Serializable]
    public class WalkSettings : Settings 
    {
        public int Range;
        public MovemntTypes MovemntType;
    }
}
