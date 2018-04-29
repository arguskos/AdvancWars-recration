using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Zenject;
using System.Linq;
using UnityEngine.Tilemaps;


public class LevelInstaller : MonoInstaller
{
    public MapManager Map;
    public Tilemap Tilemap;
    public ActionMenu ActionMenu;
    public Selection Selection;
    public GameObject TileInfo;

    public List<Sprite> PathTextures;

    [Header("Transforms ")]
    public Transform TileInfoTransform;
    public Transform TileInfoGroups;



    [Inject]
    Settings _settings = null;

    public override void InstallBindings()
    {
        // In this example there is only one 'installer' but in larger projects you
        // will likely end up with many different re-usable installers
        // that you'll want to use in several different scenes
        //
        // There are several ways to do this.  You can store your installer as a prefab,
        // a scriptable object, a component within the scene, etc.  Or, if you don't
        // need your installer to be a MonoBehaviour then you can just simply call
        // Container.Install
        //
        // See here for more details:
        // https://github.com/modesttree/zenject#installers
        //
        //Container.Install<MyOtherInstaller>();
        // Install the main game
        //Container.Bind<AsteroidManager>().AsSingle();
        //Container.Bind<Highlighter>().To<BaseTile>();
        //Container.Bind<Highlighter.HighlighterFactory>().AsSingle().WithArguments(_settings.Highlighter);
        //Container.Bind<IHighlightable>().To<BaseTile>().AsSingle();
        //    Container.Bind<BaseTile>().AsSingle();
        //    Container.BindFactory<Highlighter, Highlighter.HighlighterFactory>()
        //.FromComponentInNewPrefab(_settings.Highlighter)
        //.UnderTransform(transform);
        //Container.BindInstance(Map);

        //Container.Bind<Vector2Int>().AsCached();

        //Container.Bind<UnitStateFactory>().AsSingle();
        //Container.BindFactory< UnitActiveState, UnitActiveState.Factory>().WhenInjectedInto<UnitStateFactory>();
        ////Container.BindFactory<MapMover, UnitMoveState,   UnitMoveState.Factory>().WhenInjectedInto<UnitStateFactory>();
        //Container.BindFactory< UnitMoveState, UnitMoveState.Factory>();

        InstallMap();


        Container.Bind<Highlighter.HighlighterFactory>().AsSingle().WithArguments(_settings.Highlighter);
        Container.Bind<Selection>().FromInstance(Selection);


        Container.Bind<PlayerInputState>().AsSingle();
        Container.BindInterfacesTo<PlayerInputHandler>().AsSingle();

        InstallHUD();


        foreach (Sprite s in PathTextures)
        {
            Container.Bind<Sprite>().FromInstance(s);
        }
        Container.BindFactory<PathDrower.PathTypes, PathSectionVisuals, PathSectionVisuals.Factory>().FromComponentInNewPrefab(_settings.PathSection).UnderTransformGroup("PathSections");
        Container.Bind<PathDrower>().AsSingle().NonLazy();


        Container.BindInstance(_settings.Icons[1]).WhenInjectedInto<CanFire>();
        Container.BindInstance(_settings.Icons[3]).WhenInjectedInto<CanWalk>();
        Container.BindInstance(_settings.Icons[5]).WhenInjectedInto<CanCapture>();

        Container.Bind<GameState>().AsSingle().NonLazy();

        InstallMisc();

        Container.BindInstance(_settings.InfantryPrefab);
        Container.BindFactory<Type, BaseUnit, TileEntity.Factory>().FromFactory<TileEntity.CustomFactory>();

        Container.Bind<Player>().FromInstance(FindObjectOfType<Player>());
        //Container.BindFactory<Infantry, EntetieFactory>().FromComponentInNewPrefab(_settings.InfantryPrefab);
        //Container.BindFactory<TileEntity, TileEntity.Factory>();
    }

    private void InstallMisc()
    {
        Container.BindInstance(_settings.Numbers).WhenInjectedInto<NumToSprite>();
        Container.Bind<NumToSprite>().AsSingle();
    }

    private void InstallMap()
    {
        Container.BindInstance(Tilemap);
        Container.Bind<MapManager>().AsSingle();
        Container.Bind<MapHighlighter>().AsSingle();
        Container.Bind<MapEntetiesManager>().AsSingle();

    }

    private void InstallHUD()
    {
        Container.Bind<IActionMenu>().To<ActionMenu>().FromInstance(ActionMenu);
        //Give this image only to action menu 
        Container.Bind<Image>().FromInstance(ActionMenu.GetComponentInChildren<Pointer>().GetComponent<Image>()).WhenInjectedInto<IActionMenu>();

        Container.Bind<IMenuItem>().To<ActionMenuItem>().AsTransient();
        // Container.BindFactory<ActionMenuItem, ActionMenuItem.Factory>().FromComponentInNewPrefab(_settings.ActionMenuItermPrefab).UnderTransform(ActionMenu.transform.Find("Spacer"));//.WithArguments(_settings.ActionMenuItermPrefab);
        Container.BindFactory<IMenuItem, MenuItemFactory>().To<ActionMenuItem>().FromComponentInNewPrefab(_settings.ActionMenuItermPrefab).UnderTransform(ActionMenu.transform.Find("Spacer"));//.WithArguments(_settings.ActionMenuItermPrefab);;

        //Container.Bind<Image>().FromInstance(TileInfo.GetComponentInChildren<Image>()).WhenInjectedInto<CurrentTileInfo>();
        //Container.Bind<Text>().FromInstance(TileInfo.GetComponentInChildren<Text>()).WhenInjectedInto<CurrentTileInfo>();
        Container.Bind<Text>().WithId("SmallText").FromInstance(_settings.SmallUIText);
        Container.Bind<Text>().WithId("MainText").FromInstance(_settings.MainUIText);

        Container.BindFactory<TileInfoFieldUI, TileInfoFieldUI.Factory>().FromComponentInNewPrefab(_settings.TileInfoFieldUI);//.WithArguments(_settings.ActionMenuItermPrefab);;
        Container.BindFactory<TileInfoGroupUI, TileInfoGroupUI.Factory>().FromComponentInNewPrefab(_settings.TileInfoGroupUI).UnderTransform(TileInfoGroups);//.WithArguments(_settings.ActionMenuItermPrefab);;

    }

    //void InstallAsteroids()
    //{
    //    // ITickable, IFixedTickable, IInitializable and IDisposable are special Zenject interfaces.
    //    // Binding a class to any of these interfaces creates an instance of the class at startup.
    //    // Binding to any of these interfaces is also necessary to have the method defined in that interface be
    //    // called on the implementing class as follows:
    //    // Binding to ITickable or IFixedTickable will result in Tick() or FixedTick() being called like Update() or FixedUpdate().
    //    // Binding to IInitializable means that Initialize() will be called on startup.
    //    // Binding to IDisposable means that Dispose() will be called when the app closes, the scene changes,
    //    // or the composition root object is destroyed.

    //    // Any time you use To<Foo>().AsSingle, what that means is that the DiContainer will only ever instantiate
    //    // one instance of the type given inside the To<> (in this example, Foo). So in this case, any classes that take ITickable,
    //    // IFixedTickable, or AsteroidManager as inputs will receive the same instance of AsteroidManager.
    //    // We create multiple bindings for ITickable, so any dependencies that reference this type must be lists of ITickable.
    //    Container.Bind<ITickable>().To<AsteroidManager>().AsSingle();
    //    Container.Bind<IFixedTickable>().To<AsteroidManager>().AsSingle();
    //    Container.Bind<AsteroidManager>().AsSingle();

    //    // The above three lines are also identical to just doing this instead:
    //    // Container.BindInterfacesAndSelfTo<AsteroidManager>();

    //    // Here, we're defining a generic factory to create asteroid objects using the given prefab
    //    // So any classes that want to create new asteroid objects can simply include an injected field
    //    // or constructor parameter of type Asteroid.Factory, then call Create() on that
    //    Container.BindFactory<Asteroid, Asteroid.Factory>()
    //        .FromComponentInNewPrefab(_settings.AsteroidPrefab)
    //        // We can also tell Zenject what to name the new gameobject here
    //        .WithGameObjectName("Asteroid")
    //        // GameObjectGroup's are just game objects used for organization
    //        // This is nice so that it doesn't clutter up our scene hierarchy
    //        .UnderTransformGroup("Asteroids");
    //}

    //void InstallMisc()
    //{
    //    Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
    //    Container.Bind<LevelHelper>().AsSingle();

    //    Container.BindInterfacesTo<AudioHandler>().AsSingle();

    //    Container.Bind<ExplosionFactory>().AsSingle().WithArguments(_settings.ExplosionPrefab);
    //    Container.Bind<BrokenShipFactory>().AsSingle().WithArguments(_settings.BrokenShipPrefab);
    //}

    //void InstallShip()
    //{
    //    Container.DeclareSignal<ShipCrashedSignal>();

    //    Container.Bind<ShipStateFactory>().AsSingle();

    //    // Note that the ship itself is bound using a ZenjectBinding component (see Ship
    //    // game object in scene heirarchy)

    //    Container.BindFactory<ShipStateWaitingToStart, ShipStateWaitingToStart.Factory>().WhenInjectedInto<ShipStateFactory>();
    //    Container.BindFactory<ShipStateDead, ShipStateDead.Factory>().WhenInjectedInto<ShipStateFactory>();
    //    Container.BindFactory<ShipStateMoving, ShipStateMoving.Factory>().WhenInjectedInto<ShipStateFactory>();
    //}

    //void InitExecutionOrder()
    //{
    //    // In many cases you don't need to worry about execution order,
    //    // however sometimes it can be important
    //    // If for example we wanted to ensure that AsteroidManager.Initialize
    //    // always gets called before GameController.Initialize (and similarly for Tick)
    //    // Then we could do the following:
    //    Container.BindExecutionOrder<GameController>(-20);

    //    // Note that they will be disposed of in the reverse order given here
    //}

    [Serializable]
    public class Settings
    {
        public GameObject Highlighter;
        public GameObject ActionMenuItermPrefab;
        public GameObject Infantry;
        public GameObject Menu;

        public GameObject PathSection;

        public List<Sprite> Icons;
        public List<Sprite> Numbers;

        public Text SmallUIText;
        public Text MainUIText;
        public TileInfoFieldUI TileInfoFieldUI;
        public TileInfoGroupUI TileInfoGroupUI;


        public Infantry InfantryPrefab;
    }
}
