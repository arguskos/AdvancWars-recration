﻿using System;
using UnityEngine;
using System.Collections;
using Zenject;


// We prefer to use ScriptableObjectInstaller for installers that contain game settings
// There's no reason why you couldn't use a MonoInstaller here instead, however
// using ScriptableObjectInstaller has advantages here that make it nice for settings:
//
// 1) You can change these values at runtime and have those changes persist across play
//    sessions.  If it was a MonoInstaller then any changes would be lost when you hit stop
// 2) You can easily create multiple ScriptableObject instances of this installer to test
//    different customizations to settings.  For example, you might have different instances
//    for each difficulty mode of your game, such as "Easy", "Hard", etc.
// 3) If your settings are associated with a game object composition root, then using
//    ScriptableObjectInstaller can be easier since there will only ever be one definitive
//    instance for each setting.  Otherwise, you'd have to change the settings for each game
//    object composition root separately at runtime
//
// Uncomment if you want to add alternative game settings
//[CreateAssetMenu(menuName = "Asteroids/Game Settings")]

[CreateAssetMenu(fileName = "LevelInstaller", menuName = "Installers/LevelInstaller", order = 1)]
public class ScriptableLevelInstaller : ScriptableObjectInstaller<ScriptableLevelInstaller>
{
    //public ShipSettings Ship;
    //public AsteroidSettings Asteroid;
    //public AudioHandler.Settings AudioHandler;
    public LevelInstaller.Settings GameInstaller;
    public UnitInstaller.Settings UnitInstaller;


    public Infantry.InfantrySettinigs Infantry;
    public SmallTank.SmallTankSettings SmallTank;

    public IconsReference IconsReference;

    // We use nested classes here to group related settings together
    //[Serializable]
    //public class ShipSettings
    //{
    //    public ShipStateMoving.Settings StateMoving;
    //    public ShipStateDead.Settings StateDead;
    //    public ShipStateWaitingToStart.Settings StateStarting;
    //}

    //[Serializable]
    //public class AsteroidSettings
    //{
    //    public AsteroidManager.Settings Spawner;
    //    public Asteroid.Settings General;
    //}

    //[Serializable]
    //public class TileSettings
    //{
    //    public Vector2 TileSize;
    //}
    public override void InstallBindings()
    {

  
        Container.Bind<Infantry.InfantrySettinigs>().FromInstance(Infantry);
        Container.Bind<SmallTank.SmallTankSettings >().FromInstance(SmallTank);


        Container.BindInstance(IconsReference);



        //Container.BindInstance(SmallTank).AsTransient().WhenInjectedInto<SmallTank>();
        //Container.BindInstance(SmallTank.WalkSettings).AsTransient();
        //Container.BindInstance(SmallTank.WalkSettings).AsSingle().WhenInjectedInto<SmallTank.SmallTankSettings>();


        Container.BindInstance(GameInstaller);
        Container.BindInstance(UnitInstaller);

    }
}

