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
    public (int, bool)[] progress;
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
        week = 0;
        this.league = league;
        this.playOffs = "";
        this.bestEastPlayers = new string[] { };
        this.bestWestPlayers = new string[] { };
        this.bestOfThreeMatches = new string[] { };
        this.winner ="";
        progress = new[]
        {
            (3,false),
            (7,false),
            (11,false),
            (15,false),
            (19,false),
            (23,false),
            (27,false),
        };
    }
}


