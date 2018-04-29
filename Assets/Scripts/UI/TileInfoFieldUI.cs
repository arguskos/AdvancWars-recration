using UnityEngine;
using UnityEngine.UI;

using Zenject;



public class TileInfoFieldUI : MonoBehaviour
{

    [SerializeField]
    private Text _text;
    [SerializeField]
    private Image _image;

    public void UpdateInfo(Sprite sp, string text)
    {
        if (_image.sprite != sp)
        {
            _image.sprite = sp;
        }
        _text.text = text;
    }


    //public void Init(string one, string two)
    //{
    //    _positionOne.gameObject.AddComponent<Text>().text= one ;
    //    _positionTwo.gameObject.AddComponent<Text>().text = two;
    //}
    //public void Init(Image one, string two)
    //{
    //    _positionOne.gameObject.AddComponent<Image>().sprite = one.sprite;
    //    _positionTwo.gameObject.AddComponent<Text>().text = two;
    //}



    public class Factory : Factory<TileInfoFieldUI>
    {
    }


}
