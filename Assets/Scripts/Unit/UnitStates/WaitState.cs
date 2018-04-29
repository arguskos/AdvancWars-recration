
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class WaitState : BaseUnitState
{
    private readonly Selection _selection;
    private readonly BaseUnit _unit;


    public WaitState(BaseUnit u , Selection selection)
    {
        _unit = u;
        _selection = selection;
    }

    public override void Start()
    {
        _selection.UnSelect();
        _selection.SetMode(SelectionMode.Free);
        _selection.ReAddActions();

        _unit.Deactivate();

        //Debug.Log(_unit.hasAuthority);
        //Debug.Log(_unit.isServer);
        //Debug.Log(_unit.isClient);
        //if (Network.isServer)
        //{
        //    _unit.CmdDeactivate();
        //    _unit.RpcDeactivate();
        //}
        //else
        //{
        //    _unit.CmdDeactivate();
        //    _unit.RpcDeactivate();
        //}
    }

    public class Factory : Factory<WaitState>
    {
    }
}
