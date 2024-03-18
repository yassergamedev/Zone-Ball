using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour, IDataPersistence
{

    PlayerPersistent[] players;

   public void LoadData(GameData data)
    {
        players = new PlayerPersistent[90];
       for(int i = 0; i<data.newGameDraftPool.Length; i++)
        {
            FileDataHandler<PlayerPersistent> fl = new(Application.persistentDataPath + "/Players/", "player" + i);
            players[i] = fl.Load();
            Debug.Log(players[i].Name);
        }
    }

    public void SaveData(ref GameData data)
    {
       // data.player = player;
    }

   
}
