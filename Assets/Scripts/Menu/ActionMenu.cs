using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
public class ActionMenu : MonoBehaviour, IActionMenu
{
    private PlayerInputState _inputState;
    private Image _pointer;
    private MenuItemFactory _actionMenuFactory;
    private int _currentlySelected = 0;
    private List<BaseAbility> _abilities;
    private GameObject MenuItemHolder;
    private Vector2 _initPosition;
    private Selection _selection;

    [Inject]
    public void Construct(Selection selection, Image pointer, PlayerInputState iS, MenuItemFactory menuItemFactory)
    {
        _pointer = pointer;
        _selection = selection;
        _inputState = iS;
        _actionMenuFactory = menuItemFactory;
    }

    protected void Awake()
    {
        MenuItemHolder = transform.Find("Spacer").gameObject;
        _initPosition = MenuItemHolder.GetComponent<RectTransform>().anchoredPosition;
    }

    public void ShowMenu(List<BaseAbility> abilities)
    {
        _abilities = abilities;
        _currentlySelected = 0;
        gameObject.SetActive(true);
        _pointer.rectTransform.anchoredPosition = new Vector2(_pointer.rectTransform.anchoredPosition.x, 105.7f);
        float height = 0;
        for (int i = 0; i < _abilities.Count; i++)
        {
            if (_abilities[i].CanBeActivated())
            {
                IMenuItem item = _actionMenuFactory.Create();
                MenuItemHolder.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, -item.GetRect().height / 2);
                height += 5;
                item.SetAbilitie(_abilities[i]);
            }
          
        }
        _inputState.IsMovedAction = MoveHelper;
    }
    public void MoveHelper(int x, int y)
    {
        if (y < 0)
        {
            MoveSelectionUp();
        }
        else if (y > 0)
        {
            MoveSelectionDown();
        }
    }
    public void HideMenu()
    {
        gameObject.SetActive(false);
        foreach (Transform t in MenuItemHolder.transform)
        {
            Destroy(t.gameObject);
        }
        MenuItemHolder.GetComponent<RectTransform>().anchoredPosition = _initPosition;
    }

    private void UpdatePointerPosition()
    {
        //TOTDO remove magin numbers
        _pointer.rectTransform.anchoredPosition = new Vector2(_pointer.rectTransform.anchoredPosition.x, 105.7f - (19 + 10) * _currentlySelected);

    }

    public void MoveSelectionUp()
    {
        _currentlySelected++;
        if (_currentlySelected > _abilities.Count - 1)
        {
            _currentlySelected = 0;
        }
        UpdatePointerPosition();
    }

    public void MoveSelectionDown()
    {
        _currentlySelected--;
        if (_currentlySelected < 0)
        {
            _currentlySelected = _abilities.Count - 1;
        }
        UpdatePointerPosition();

    }

    public void Confirm()
    {

        _abilities[_currentlySelected].Action();
        HideMenu();
    }
    public void Cancel()
    {
        HideMenu();
        _selection.ReAddActions();

    }


}
