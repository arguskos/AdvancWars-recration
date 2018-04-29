using UnityEngine;
using System.Collections.Generic;
using Zenject;
using System.Linq;

public class GameState : IInitializable
{
    private int _players;
    public int CurrentPlayerTurn = 1;
    private Dictionary<int,List<BaseUnit>> ListOfUnitsForEachPlayer = new Dictionary<int,List<BaseUnit>>();

    [Inject]
    private List<BaseUnit> _units = new List<BaseUnit>();
    public int Days;


    public GameState()
    {
        //Bad but can't do other way for some reason 
        _units =  GameObject.FindObjectsOfType<BaseUnit>().ToList();
        foreach (BaseUnit u in _units)
        {
            if (ListOfUnitsForEachPlayer.ContainsKey(u.Player))
            {
                ListOfUnitsForEachPlayer[u.Player].Add(u);
            }
            else
            {
                _players++;
                ListOfUnitsForEachPlayer[u.Player] = new List<BaseUnit>() { u };
            }
        }

    }


    public void NextPlayer()
    {

        CurrentPlayerTurn++;
        if (CurrentPlayerTurn>_players)
        {
            CurrentPlayerTurn = 1;
            Days++;
        }
        Debug.Log("Current player " + CurrentPlayerTurn);


    }

    public void Initialize()
    {
        Debug.Log("as");
    }
}
