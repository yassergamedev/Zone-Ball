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
    public Progress[] progress;
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
        progress = new Progress[]
        {
            new(3,false),
            new(7,false),
            new(11,false),
            new(15, false),
            new(19,false),
            new(23,false),
            new(27,false),
        };
    }

}

[System.Serializable]
public class Progress
{
    public int week;
    public bool hasProgressed;


    public void setWeek(int w)
    { week = w; }
    public void setHasProgressed(bool hasProgressed)
    {
        this.hasProgressed = hasProgressed;
    }
    public Progress(int w, bool h)
    {
        week = w;
        hasProgressed = h;
    }
}


