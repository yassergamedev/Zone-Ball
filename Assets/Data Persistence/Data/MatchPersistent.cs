
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MatchPersistent
{
    string id;
    string date;
    bool isPlayed;
    MatchTeam[] teams;

    public MatchPersistent(string id, string date, MatchTeam[] teams)
    {
        this.id = id;
        this.date = date;
        this.teams = teams;
        isPlayed = false;
    }
    
}

[System.Serializable]
public class MatchTeam
{
    public string id;
    public int score;
    public bool isHome;
    public bool isWinner;

    public MatchTeam(string id, int score, bool isHome, bool isWinner)
    {
        this.id = id;
        this.score = score;
        this.isHome = isHome;
        this.isWinner = isWinner;
    }
}
