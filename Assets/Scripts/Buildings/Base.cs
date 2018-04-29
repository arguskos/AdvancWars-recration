using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : TileEntity
{

    private int Loyalty = 20;
    public Action OnCapture;

    protected override void Start()
    {
        base.Start();
    }

    public void UnCapture()
    {
        Loyalty = 20;
    }

    public void Capture(int units, int team)
    {
        Loyalty -= units;
        if (Loyalty <= 0)
        {

            Team = team;
            //TODO change to zenject
            if (GetComponent<AnimationSwitcher>())
            {
                GetComponent<AnimationSwitcher>().SetAnim();
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
