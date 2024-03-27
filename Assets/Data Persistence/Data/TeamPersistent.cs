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
    public int pointsScored, pointsAllowed, turnovers, oppTurnovers, wins, losses;
    public string[] HC, DC, OC;
    public List<MatchPlayed> matchesPlayed;
    public TeamPersistent(string id, string name, string conf, string[] plyrs)
   {
        this.id = id;
        this.name = name;
        this.Conference = conf;
        this.players = plyrs;
        matchesPlayed = new() { };
        playstyle = 5;
        start = "Young";
        HC = new string[]{ "","" };
        OC = new string[] { "", "" };
        DC = new string[] { "", "" };
    }

  
}
[System.Serializable]
public class MatchPlayed
{
    //match id
    public string opponent;
    public bool isHome;
    //match stats id
    public bool isPlayed;
    public bool isReady;
    public bool result;
    public int score;
    public int oppScore;
    public int turnovers;
    public int oppTurnovers;
    public List<string> playersStats;
    public MatchPlayed(string opponent, bool isHome)
    {
        this.opponent = opponent;
        this.isHome = isHome;
        isPlayed = false;
        isReady = false;
    }
}




