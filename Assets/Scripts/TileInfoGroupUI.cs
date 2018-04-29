using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TileInfoGroupUI : MonoBehaviour
{

    [SerializeField]
    private Text _text;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private Transform _elementsTransform;



    private TileInfoFieldUI.Factory _tileInfoFiedFactory;

    private List<TileInfoFieldUI> _infoFields = new List<TileInfoFieldUI>();

    [Inject]
    public void Create(TileInfoFieldUI.Factory infoFieldFactory)
    {
        _tileInfoFiedFactory = infoFieldFactory;
    }


    public void UpdateInfo(Sprite sp, string text = "")
    {
        _image.sprite = sp;
        if (!string.IsNullOrEmpty(text))
        {
            _text.text = text;
            if (!_text.gameObject.activeSelf)
            {
                _text.gameObject.SetActive(true);
            }
        }
        else
        {
            if (_text.gameObject.activeSelf)
            {
                _text.gameObject.SetActive(false);
            }
        }
    }

    //public void UpdateOrAddFields(Graphic one , Graphic two)
    // {
    //     TileInfoFieldUI infoField =  _tileInfoFiedFactory.Create();
    //     infoField.transform.parent = _elementsTransform;
    //     infoField.UpdateField(one, two);
    // }
    public void UpdateOrAddField(Sprite sp , string text, int index)
    {
        if (index+1 > _infoFields.Count)
        {
            TileInfoFieldUI infoField = _tileInfoFiedFactory.Create();
            infoField.transform.SetParent(_elementsTransform,false);
            _infoFields.Add(infoField);
        }
        _infoFields[index].UpdateInfo(sp,text);

    }

    public void AddField(Sprite sprite, string textBig)
    {

    }

    public class Factory : Factory<TileInfoGroupUI>
    {
    }
}
