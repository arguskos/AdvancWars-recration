using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour
{

    public MapEntetiesManager _mapEntetiesManager;
    [Inject]
    public DiContainer Container;
    [Inject]
    public void Construct(MapEntetiesManager me)
    {
        Debug.Log("Constructed");
    }

    public void Awake()
    {
            _mapEntetiesManager = FindObjectOfType<MyNetworkManager2>().Container.Resolve<MapEntetiesManager>();
    }

    [ClientRpc]
    void RpcMoveFromServer(int x1, int y1, int x2, int y2)
    {
        //u.transform.position = new Vector3(move, 0, 0);
        //u.Pos = move;
        Debug.Log("is local player? "+isLocalPlayer);
        TileEntity t = _mapEntetiesManager.GetEntitieOfType<TileEntity>(new Vector2Int(x1,y1));
        if (t)
        {
            t.RpcMove(x2, y2,false);
        }
    }

    [Command]
    public void CmdMove(int  x1, int y1, int x2,  int y2 )
    { 
        RpcMoveFromServer(x1,y1,x2,y2);
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space) )
        {
            foreach(var player in FindObjectsOfType<Player>())
            {
                print(player.isLocalPlayer);
            }
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
	}
}
