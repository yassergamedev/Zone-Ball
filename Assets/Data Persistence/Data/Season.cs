using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Season
{
    //using id's
    public string id;
    public string date;
    public int week;
    public string league;
    public string playOffs;
    public string[] firstRoundRookieDraft;
    public string[] secondRoundRookieDraft;
    public string[] bestEastPlayers;
    public string[] bestWestPlayers;
    public string[] bestOfThreeMatches;
    //Conference
    public string winner;

    public Season(string id, string date, string league)
    {
        this.id = id;
        this.date = date;
        week = 1;
        this.league = league;
        this.playOffs = "";
        this.bestEastPlayers = new string[] { };
        this.bestWestPlayers = new string[] { };
        this.bestOfThreeMatches = new string[] { };
        this.winner ="";
    }
}


