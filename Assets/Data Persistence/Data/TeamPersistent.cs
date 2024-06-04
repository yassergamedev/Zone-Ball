using System;
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
    public bool round1=false,round2=false;
    public int pointsScored,
        pointsAllowed,
        turnovers,
        oppTurnovers,
        wins,
        losses,
        otWins,
        otLosees,
        playoffWins,
        playoffLosses,
        championships,
        jukes,
        ptsScored,
        insidePoints,
        midPoints,
        outsidePoints,
        foulShotsMade,
        steals,
        blocks,
        fouls
        ;
    public string[] HC, DC, OC;
    public List<(string, Func<int>)> getStats()
    {

        //return the list of stats
        return new List<(string, Func<int>)> {("wins", () => wins),("losses", () => losses),("otWins", () => otWins),("otLosses", () => otLosees),
            ("playoffWins", () => playoffWins),("playoffLosses", () => playoffLosses),("championships", () => championships),
            ("blocks", () => blocks), ("steals", () => steals), ("fouls", () => fouls),("pointsScored", () => pointsScored), ("pointsAllowed", () => pointsAllowed),
                  ("jukes", () => jukes),   ("foulShotsMade", () => foulShotsMade), ("turnovers", () => turnovers), ("oppTurnovers", () => oppTurnovers), ("insidePoints", () => insidePoints), 
                  ("midPoints", () => midPoints),  ("outsidePoints", () => outsidePoints) };


    }
    public List<MatchPlayed> matchesPlayed;
    public List<MatchPlayed> playOffMatches;

    public TeamPersistent(string id, string name, string conf, string[] plyrs)
   {
        this.id = id;
        this.name = name;
        this.Conference = conf;
        this.players = plyrs;
        
        matchesPlayed = new() { };
        playOffMatches = new() { }; 
        playstyle = 5;
        start = "Young";
        HC = new string[]{ "Consistency", "Awareness" };
        OC = new string[] { "Juking", "Control" };
        DC = new string[] { "Guarding", "Pressure" };
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
    public int extraTime;
    public List<string> playersStats;
    public MatchPlayed(string opponent, bool isHome)
    {
        this.opponent = opponent;
        this.isHome = isHome;
        isPlayed = false;
        isReady = false;
    }
    public MatchPlayed()
    {
        this.opponent = "";
        this.isHome = false;
        isPlayed = false;
        isReady = false;
    }
}




