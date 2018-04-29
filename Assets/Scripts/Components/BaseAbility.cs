using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;                        
[System.Serializable]
public abstract class BaseAbility : MonoBehaviour
{


    public bool Selectable { get; protected set; }
    public string Name { get; protected set; }
    public Sprite Icon { get; private set; }
    public BaseUnit Unit { get; protected set; }

    private IActionMenu _menu;


    [Inject]
    public void Construct(BaseUnit unit, Sprite icon,IActionMenu menu)
    {
        Unit = unit;
        _menu = menu;
        Icon = icon;
    }

    public virtual void Action()
    {

    }


    public virtual Sprite GetIcon()
    {
        return Icon;
    }
    
    public virtual bool CanBeActivated()
    {
        return true;
    }

    [System.Serializable]
    public class Settings
    {
   
    }
}
