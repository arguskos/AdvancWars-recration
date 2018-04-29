using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class UnitNetworkSpawner : NetworkBehaviour
{

    public int Team;
    public int Player;

    public GameObject InfantryPrefab;
    public static UnitNetworkSpawner Instance;
    [Inject]
    private TileEntity.Factory _factory;

    private void Awake()
    {
        Instance = this;
    }

    public void Spawn()
    {

        //if (isServer&& hasAuthority )
        //{
        //    //Debug.Log("SPPAWWWNing");
        //    //var p = _factory.Create(typeof(Infantry));
        //    //NetworkServer.Spawn(p.gameObject);
        //}

    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Player " + " connected from " + player.ipAddress + ":" + player.port);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
