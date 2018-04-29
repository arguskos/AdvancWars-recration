using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

public interface IEntity
{

}

public enum Units
{
    Infantry,
    Tank
}


public class EntetieFactory : Factory<Infantry>
{
    DiContainer _container;
    public EntetieFactory(DiContainer container)
    {
        _container = container;
    }

    public TileEntity MyCreate<lol>() where lol : BaseUnit
    {
        return _container.Instantiate<Infantry>();
    }


}


public class CustomEnemyFactory : IFactory<IEntity>
{
    DiContainer _container;

    public CustomEnemyFactory(DiContainer container)
    {
        _container = container;
    }

    public IEntity Create<Type>() where Type : IEntity
    {
        return _container.Instantiate<Type>();

    }

    public IEntity Create()
    {
        throw new System.NotImplementedException();
    }

    public IEntity Create(TileEntity param)
    {
        throw new System.NotImplementedException();
    }
}


public abstract class TileEntity : MonoBehaviour, IEntity
{


    public int Team;
    public int Player;


    protected MapEntetiesManager EntetiesManager;
    protected MapHighlighter MapHighlighter;
    private MapManager _mapManager;



    [Inject]
    void Constrcucor(MapManager mM, MapEntetiesManager em, MapHighlighter mh)
    {
        EntetiesManager = em;
        MapHighlighter = mh;
        _mapManager = mM;

    }

    protected virtual void Start()
    {
        //if (!isServer)
        //{
        //    Destroy(this);
        //}


        Position = _mapManager.GetClossestTileFromPos(transform.position);
        RpcMove(Position.x, Position.y, true);
    }


    public virtual void RpcMove(int x, int y, bool teleport )
    {
        EntetiesManager.MoveEntetie(new Vector2Int(x, y), this, teleport);
        Position = new Vector2Int(x, y);
    }


    private void SetPosition()
    {
        transform.position = _mapManager.GetTilePosition(Position.x, Position.y);
    }

    public Vector2Int Position
    {
        get; protected set;
    }



    public class Factory : Factory<Type, BaseUnit>
    {
    }

    public class CustomFactory : IFactory<Type, BaseUnit>
    {
        private DiContainer _container;

        public CustomFactory(DiContainer container)
        {
            _container = container;
        }                                             

        public BaseUnit Create<Type>() where Type : BaseUnit 
        {
            return Create(typeof(Type));
        }

        public BaseUnit Create(Type type)
        {
            if (type.BaseType != typeof(BaseUnit))
            {
                Debug.LogError("you cant create objects that are not derrived from base unit");
                return null;
            }

            object obj = _container.Resolve(type) ;
            return _container.InstantiatePrefabForComponent(type, obj as UnityEngine.Object, null, new object[0]) as BaseUnit;
        }
    }
}
