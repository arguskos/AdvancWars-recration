using Zenject;
using UnityEngine;
using System.Collections.Generic;

public class NumToSprite
{
    private  readonly List<Sprite> Sprites;

    public  NumToSprite(List<Sprite> sprite)
    {
        Sprites = sprite;
    }


    public  Sprite GetSprite(int n)
    {
        if (n<Sprites.Count)
        {
            return Sprites[n];
        }
        return Sprites[0];
    }
}
