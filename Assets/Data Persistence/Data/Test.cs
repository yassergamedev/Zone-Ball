using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour, IDataPersistence
{

    PlayerPersistent[] players;

   public void LoadData(GameData data)
    {
     
    }

    public void SaveData(ref GameData data)
    {
       // data.player = player;
    }

   
}