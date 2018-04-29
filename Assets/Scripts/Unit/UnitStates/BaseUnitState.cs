using System;
using UnityEngine;
using System.Collections;
using Zenject;
using UnityEngine.Networking;

public abstract class BaseUnitState : IDisposable
{

    public virtual void Update()
    {

    }

    public virtual void Start()
    {
        // optionally overridden
    }

    public virtual void Dispose()
    {
        // optionally overridden
    }

}