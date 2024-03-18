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
    public List<MatchPlayed> matchesPlayed;
    public TeamPersistent(string id, string name, string conf, string[] plyrs)
   {
        this.id = id;
        this.name = name;
        this.Conference = conf;
        this.players = plyrs;
        matchesPlayed = new();
 
    }

  
}
public class MatchPlayed
{
    public string id;
    public string stats;
}

