using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;

public class CurrentTileInfo : MonoBehaviour
{

    // public Transform UISpawnTransform;

    private MapManager _mapManager;
    private MapEntetiesManager _mapEntetieManager;
    private Selection _selection;

    private Text _tileInfoText;
    private Image _tileInfoSprite;

    private Text _smallText;
    private Text _mainText;

    //private TileInfoFieldUI.Factory _infoGroupFactory;

    private TileInfoGroupUI.Factory _tileInfoGroupFactory;
    private IconsReference _iconRef;

    private List<TileInfoGroupUI> _infoGroupsForEnteties = new List<TileInfoGroupUI>();

    [Inject]
    public void Construct(
        [Inject(Id = "SmallText")]
        Text smallText,
        [Inject(Id = "MainText")]
        Text mainText,
        IconsReference iconRef,
        MapEntetiesManager mapEntetieManager,
       // UIInfoGroup.Factory infoGroupFactory,
       MapManager mM, Selection selection, TileInfoGroupUI.Factory tileInfoFactory)
    {
        _iconRef = iconRef;

        _smallText = smallText;
        _mainText = mainText;

        //_infoGroupFactory = infoGroupFactory;
        _tileInfoGroupFactory = tileInfoFactory;


        _mapManager = mM;
        _mapEntetieManager = mapEntetieManager;
        _selection = selection;
        _selection.OnSelectionChanged += UpdateInfo;
    }

    private void UpdateTileInfo(string name, Sprite sprite)
    {
        _tileInfoText.text = name;
        _tileInfoSprite.sprite = sprite;
    }

    private void UpdateInfo()
    {
        UpdateUnitsInfo();
        UpdateTileInfo();
    }


    private void UpdateTileInfo()
    {
        BaseTile t = _mapManager.GetTile(_selection.Position);
        _infoGroupsForEnteties[0].UpdateInfo(t.sprite, t.name);
        _infoGroupsForEnteties[0].UpdateOrAddField(_iconRef.DefIcon, t.BaseDef.ToString(), 0);
    }

    private void AddUnitFields(TileInfoGroupUI group, BaseUnit unit)
    {
        Damageable d = unit.GetPropertyOfType<Damageable>() as Damageable;
        Supplyable s = unit.GetPropertyOfType<Supplyable>() as Supplyable;
        CanFire f = unit.GetAbilitieOfType<CanFire>() as CanFire;

        int health = 0;
        int ammo = 0;
        int supply = 0;
        if (d)
        {
            health = d.Health;
        }
        if (f)
        {
            ammo = f.Settings.Ammo;
        }
        if (s)
        {
            supply = s.Supply;
        }

        group.UpdateOrAddField(_iconRef.HeartIcon, health.ToString(), 0);
        group.UpdateOrAddField(_iconRef.AmmoIcon, ammo.ToString(), 1);
        group.UpdateOrAddField(_iconRef.SupplyIcon, supply.ToString(), 2);



    }

    private void UpdateUnitsInfo()
    {
        BaseUnit unit = _mapEntetieManager.GetEntitieOfType<BaseUnit>(_selection.Position) as BaseUnit;
        if (unit)
        {
            if (_infoGroupsForEnteties.Count < 2)
            {
                _infoGroupsForEnteties.Add(_tileInfoGroupFactory.Create());
                _infoGroupsForEnteties[1].transform.SetAsFirstSibling();
            }
            _infoGroupsForEnteties[1].gameObject.SetActive(true);
            _infoGroupsForEnteties[1].UpdateInfo(unit.GetComponent<SpriteRenderer>().sprite, unit.name);
            AddUnitFields(_infoGroupsForEnteties[1], unit);
        }
        else
        {
            if (_infoGroupsForEnteties.Count > 1)
            {
                _infoGroupsForEnteties[1].gameObject.SetActive(false);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        // _infoGroupFactory.Create().Init("One","Two");
        _infoGroupsForEnteties.Add(_tileInfoGroupFactory.Create());
        UpdateInfo();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
