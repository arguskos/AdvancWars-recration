using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class UnitInstaller : MonoInstaller
{


    [Inject]
    Settings _settings = null;

    public override void InstallBindings()
    {
        Container.Bind<UnitStateFactory>().AsSingle();
        Container.Bind<TileEntity>().FromComponentInHierarchy();
        Container.Bind<BaseUnit>().FromComponentInHierarchy();

        if (GetComponent<BaseUnit>())
        {
            BindStates();
        }

        BindUnityComponents();


       // Container.Bind<Damageable>().FromComponentInHierarchy();
        Container.Bind<SpriteRenderer>().FromComponentInNewPrefab(_settings.Health).WhenInjectedInto<HealthDrawer>();
        Container.Bind<HealthDrawer>().FromComponentInHierarchy();

    }

    private void BindStates()
    {

        Container.BindFactory<UnitActiveState, UnitActiveState.Factory>().WhenInjectedInto<UnitStateFactory>();
        Container.BindFactory<UnitMoveState, UnitMoveState.Factory>();//.WithArguments(gameObject.GetComponent<BaseUnit>());
        Container.BindFactory<ActionMenuState, ActionMenuState.Factory>();//.WithArguments(gameObject.GetComponent<BaseUnit>());
        Container.BindFactory<WaitState, WaitState.Factory>();
        Container.BindFactory<FireState, FireState.Factory>();
    }

    private void BindUnityComponents()
    {
        Container.Bind<BaseAbility>().FromComponentInHierarchy();
        Container.Bind<BaseUnitProprty>().FromComponentInHierarchy();

        Container.Bind<Animator>().FromComponentInHierarchy().WhenInjectedInto<AnimationSwitcher>();
        Container.Bind<AnimationSwitcher>().FromComponentInHierarchy();
        Container.Bind<SpriteRenderer>().FromComponentInHierarchy();
        Container.BindInstances(GetComponents<BaseAbility>());
        Container.BindInstances(GetComponents<BaseUnitProprty>());
    }


    [Serializable]
    public class Settings
    {
        public GameObject Health;


    }
}
