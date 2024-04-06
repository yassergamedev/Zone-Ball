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
    public string currentSeason;
    public List<string> seasons;


    public GameData(string id, DateTime date,string crnt)
    {
        this.id = id;
        this.date = date;
        currentSeason = crnt;
        seasons = new List<string>() { crnt };
   


    }

   

}
