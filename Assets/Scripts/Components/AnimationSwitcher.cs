using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AnimationSwitcher : MonoBehaviour {

    private Animator _anim;
    private TileEntity _entity;

    [Inject]
    public void Construct(Animator  anim, TileEntity tileEntity)
    {
        _anim = anim;
        _entity = tileEntity; 
    }

    public void SetAnim()
    {
        _anim.SetInteger("Team", _entity.Team);
        _anim.SetTrigger("Reset");
    }

    private void Start()
    {
        SetAnim();
    }
}
