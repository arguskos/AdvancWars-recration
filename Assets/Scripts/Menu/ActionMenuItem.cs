using UnityEngine;
using UnityEngine.UI;
using Zenject;

public  class ActionMenuItem :MonoBehaviour, IMenuItem
{
    public  Image _image;
    public  Text _text;
    public BaseAbility Ability;
    private RectTransform _rectTranform;

    protected void Awake()
    {
        _rectTranform = GetComponent<RectTransform>();
    }

    public void Select()
    {
        Ability.Action();
    }
    public Rect GetRect()
    {
        return _rectTranform.rect;
    }
    public void SetPosition(int x, int y)
    {
        _rectTranform.anchoredPosition = new Vector2(x, y);
    }

    public void SetAbilitie(BaseAbility ab)
    {
        Ability = ab;
        _text.text = Ability.Name;
        _image.sprite = ab.Icon;

    }
}
