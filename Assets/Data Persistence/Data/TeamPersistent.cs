using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class TeamPersistent 
{
    public string id;
    public string name;
    public string Conference;
    public string[] players;
    public int salaryCap = 350000;
    public int playstyle;
    public string start;
    public List<MatchPlayed> matchesPlayed;
    public TeamPersistent(string id, string name, string conf, string[] plyrs)
   {
        this.id = id;
        this.name = name;
        this.Conference = conf;
        this.players = plyrs;
        matchesPlayed = new();
        playstyle = 5;
        start = "Young";
    }

  
}
public class MatchPlayed
{
    //match id
    public string id;
    //match stats id
    public string stats;

}

