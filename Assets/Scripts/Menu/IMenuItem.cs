using System.Collections.Generic;
using UnityEngine;
public interface IMenuItem
{
    void Select();
    void SetPosition(int x , int y);
    Rect GetRect();
    void SetAbilitie(BaseAbility ab);
}