using System;
using System.Collections.Generic;
using Zenject;
using UnityEngine;
public class PathSectionVisuals : MonoBehaviour
{
    private SpriteRenderer _spriteRendere;
    private PathDrower.PathTypes _type;

    public List<Sprite> Sprites;


    [Inject]
    public void Construct(List<Sprite> ls, PathDrower.PathTypes type)
    {
        Sprites = ls;
        _type = type;
    }

    protected void Awake()
    {
        _spriteRendere = GetComponent<SpriteRenderer>();
        SetVisuals(_type);
    }

    private void SetVisuals(PathDrower.PathTypes type)
    {
        int index = (int)type;
        _spriteRendere.sprite = Sprites[index];
    }

    public class Factory : Factory<PathDrower.PathTypes, PathSectionVisuals>
    {
     

    }
}
