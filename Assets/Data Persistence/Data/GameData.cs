using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string id;
    public DateTime date;
    public List<Season> season;
  


    public GameData(string id, DateTime date)
    {
        this.id = id;
        this.date = date;

        season = new List<Season>();

    }

   

}
